using Carry;
using Events;
using Helper;
using Input;
using JetBrains.Annotations;
using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FruitBowlScene
{
    public class FruitBowlMouse : MonoBehaviour
    {
        [Title("References")]
        [SerializeField, Required] private InputManagerSO inputManager;
        [SerializeField, Required] private FloatVariable dropHeight;

        [Title("Settings")] 
        [SerializeField] private float interactionRadius;
        [SerializeField] private Vector2 minMaxDistance;

        [CanBeNull] private ICarrieAble _currentPayload;
        
        public GameEventICarrieAble PayloadPickedUpGameEvent;
        public GameEventICarrieAble PayloadDroppedGameEvent;

        private UnityEngine.Camera _mainCam;
        
        private void OnEnable()
        {
            inputManager.InteractPrimaryEvent += OnInteractPrimary;
        }

        private void OnDisable()
        {
            inputManager.InteractPrimaryEvent -= OnInteractPrimary;
        }

        private void Awake()
        {
            _mainCam = UnityEngine.Camera.main;
        }

        private void OnInteractPrimary()
        {
            //if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            if(ScreenToWorldHelper.IsMouseOverUI())
            {
                return;
            }
            
            if (_currentPayload != null)
            {
                PayloadDroppedGameEvent?.RaiseEvent(this, _currentPayload);
                _currentPayload.StopCarry();
                _currentPayload = null;
                return;
            }
            
            if(!GetFirstOverlappingPayload(out ICarrieAble carrieAble))
                return;
            
            
            StartCarry(carrieAble);
        }

        public void StartCarry(ICarrieAble carry)
        {
            if (!carry.TryStartCarry(transform, out ICarrieAble activeCarrieAble))
                return;
            PayloadPickedUpGameEvent?.RaiseEvent(this, activeCarrieAble);
            _currentPayload = activeCarrieAble;
        }

        private bool GetFirstOverlappingPayload(out ICarrieAble carrieAble)
        {
            carrieAble = null;
            Collider[] overlaps = Physics.OverlapSphere(transform.position, interactionRadius);

            foreach (Collider col in overlaps)
            {
                if (!col.TryGetComponent(out carrieAble)) 
                    continue;

                return true;
            }

            return false;
        }
        
        private void Update()
        {
            UpdateMousePosition();
        }

        private void UpdateMousePosition()
        {
            Ray ray = ScreenToWorldHelper.GetMouseToWorldRay(_mainCam);
            float dist = ScreenToWorldHelper.GetRayDistanceToWorldSpaceHeight(ray, dropHeight.Value);
            float horizontalDist = (ray.GetPoint(dist).SetY(0) - _mainCam.transform.position.SetY(0)).magnitude;
            
            Debug.DrawRay(ray.origin, ray.direction * dist, Color.red);
            
            if(horizontalDist > minMaxDistance.y || horizontalDist < minMaxDistance.x)
            {
                return;
            }

            transform.position = ray.GetPoint(dist);
        }
    }
}
