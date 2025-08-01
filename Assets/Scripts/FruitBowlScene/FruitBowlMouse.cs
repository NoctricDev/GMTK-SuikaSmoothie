using Carry;
using Events;
using Helper;
using Input;
using JetBrains.Annotations;
using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using Scenes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FruitBowlScene
{
    public class FruitBowlMouse : MonoBehaviour, IGameplaySceneObject, ICarrieAbleMouse
    {
        [Title("References")]
        [SerializeField, Required] private InputManagerSO inputManager;
        [SerializeField, Required] private FloatVariable dropHeight;

        [Title("Settings")] 
        [SerializeField] private float interactionRadius;
        [SerializeField] private Vector2 minMaxDistance;

        [CanBeNull] private ICarrieAble _currentPayload;
        
        public bool HasPayload => _currentPayload != null;
        
        public GameEventICarrieAble PayloadPickedUpGameEvent;
        public GameEventICarrieAble PayloadDroppedGameEvent;

        private UnityEngine.Camera _mainCam;

        private bool _isSceneLoaded;

        public void LoadEnd()
        {
            inputManager.InteractPrimaryEvent += OnInteractPrimary;
            _isSceneLoaded = true;
        }

        public void Unload()
        {
            _isSceneLoaded = false;
            StopCarry();
            inputManager.InteractPrimaryEvent -= OnInteractPrimary;
        }

        private void Awake()
        {
            _mainCam = UnityEngine.Camera.main;
        }

        private void OnInteractPrimary()
        {
            if(ScreenToWorldHelper.IsMouseOverUI())
            {
                return;
            }
            
            if (_currentPayload != null)
            {
                StopCarry();
                return;
            }
            
            if(!GetFirstOverlappingPayload(out ICarrieAble carrieAble))
                return;
            
            
            StartCarry(carrieAble);
        }

        public void StopCarry()
        {
            if (_currentPayload == null)
                return;
            
            PayloadDroppedGameEvent?.RaiseEvent(this, _currentPayload);
            _currentPayload.OnStopCarry();
            _currentPayload = null;
        }

        public void StartCarry(ICarrieAble carry)
        {
            if (!carry.TryStartCarry(transform, this))
                return;
            PayloadPickedUpGameEvent?.RaiseEvent(this, carry);
            _currentPayload = carry;
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
            if (!_isSceneLoaded)
                return;
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
