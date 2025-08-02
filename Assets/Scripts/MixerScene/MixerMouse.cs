using Carry;
using Events;
using FruitBowlScene;
using Helper;
using Input;
using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using Scenes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mixer
{
    public class MixerMouse : MonoBehaviour, ICarrieAbleMouse, IGameplaySceneObject
    {
        [Title("References")]
        [SerializeField] private InputManagerSO inputManager;
        [SerializeField] private FloatVariable distance;
        [SerializeField] private Transform mixerMouseTracker;
        [SerializeField] private Transform mixerInteractionPlane;
        
        public GameEventICarrieAble PayloadPickedUpGameEvent;
        public GameEventICarrieAble PayloadDroppedGameEvent;
        
        [Title("Settings")] 
        [SerializeField] private float interactionRadius;

        [SerializeField] private LayerMask layerMask;

        private ICarrieAble _currentPayload;
        
        public bool HasPayload  => _currentPayload != null;

        private UnityEngine.Camera _mainCam;
        private bool _isSceneLoaded;

        public void LoadStart(float _)
        {
            inputManager.InteractPrimaryEvent += OnInteractPrimary;
            _mainCam = UnityEngine.Camera.main;
            _isSceneLoaded = true;
        }

        public void Unload()
        {
            _isSceneLoaded = false;
            inputManager.InteractPrimaryEvent -= OnInteractPrimary;
        }

        private void OnInteractPrimary()
        {
            if (ScreenToWorldHelper.IsMouseOverUI())
                return;

            if (HasPayload)
            {
                StopCarry();
                return;
            }

            if (!GetFirstOverlappingPayload(out ICarrieAble carrieAble))
                return;
            
            StartCarry(carrieAble);
        }

        public void StartCarry(ICarrieAble carry)
        {
            if (!carry.TryStartCarry(mixerMouseTracker, this))
                return;
            PayloadPickedUpGameEvent?.RaiseEvent(this, carry);
            _currentPayload = carry;
        }

        public void StopCarry()
        {
            if (_currentPayload == null)
                return;
            
            PayloadDroppedGameEvent?.RaiseEvent(this, _currentPayload);
            _currentPayload.OnStopCarry();
            _currentPayload = null;
        }
        
        private bool GetFirstOverlappingPayload(out ICarrieAble carrieAble)
        {
            carrieAble = null;
            (Ray ray, float dist) ray = GetMouseToWorldRay();
            return Physics.SphereCast(ray.ray.origin, interactionRadius, ray.ray.direction, out RaycastHit hit, ray.dist, layerMask, QueryTriggerInteraction.Ignore) 
                   && hit.transform.TryGetComponent(out carrieAble);
        }
        
        private void Update()
        {
            if (!_isSceneLoaded)
                return;
            UpdateMousePosition();
        }

        private void UpdateMousePosition()
        {
            (Ray ray, float dist) rayInfo = GetMouseToWorldRay();
            
            Debug.DrawRay(rayInfo.ray.origin, rayInfo.ray.direction * rayInfo.dist, Color.red);

            transform.position = rayInfo.ray.GetPoint(rayInfo.dist);
        }
        
        private (Ray ray, float dist) GetMouseToWorldRay()
        {
            Ray ray = ScreenToWorldHelper.GetMouseToWorldRay(_mainCam);
            return (ray, ScreenToWorldHelper.GetRayDistanceToWorldSpaceForward(ray, mixerInteractionPlane.transform.forward.SetY(0).normalized * distance.Value));
        }
    }
}
