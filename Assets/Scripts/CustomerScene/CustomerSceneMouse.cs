using System;
using Carry;
using Events;
using FruitBowlScene;
using Glasses;
using Helper;
using Input;
using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using Scenes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomerScene
{
    public class CustomerSceneMouse : MonoBehaviour, ICarrieAbleMouse, IGameplaySceneObject
    {
        [Title("References")]
            
        [SerializeField] private InputManagerSO inputManager;
        [SerializeField] private FloatVariable interactionDistance;
        [SerializeField] private GameEventICarrieAble PayloadPickedUpGameEvent;
        [SerializeField] private GameEventICarrieAble PayloadDroppedGameEvent;

        [Title("Settings")]
        [SerializeField, ValueDropdown(nameof(GetInteractionTags))] private string[] interactionTags;
        private string[] GetInteractionTags() => TagHelper.GetTags();
        [SerializeField] private float interactionRadius;
        [SerializeField] private LayerMask layerMask;
        
        public bool HasPayload { get; }

        private bool _isActive;

        private UnityEngine.Camera _mainCam;
        private ICarrieAble _currentPayload;

        public void LoadStart(float _)
        {
            _mainCam = UnityEngine.Camera.main;
            inputManager.InteractPrimaryEvent += OnPrimaryInteract;
            _isActive = true;
        }

        public void Unload()
        {
            _isActive = false;
            inputManager.InteractPrimaryEvent -= OnPrimaryInteract;
        }

        private void Update()
        {
            if (!_isActive)
                return;
            
            UpdateMousePosition();
        }

        private void UpdateMousePosition()
        {
            (Ray ray, float dist) rayInfo = GetMouseToWorldRay();
            
            Debug.DrawRay(rayInfo.ray.origin, rayInfo.ray.direction * rayInfo.dist, Color.green);

            transform.position = rayInfo.ray.GetPoint(rayInfo.dist);
        }

        private void OnPrimaryInteract()
        {
            if (ScreenToWorldHelper.IsMouseOverUI())
                return;

            (Ray ray, float dist) rayInfo = GetMouseToWorldRay();
            if (!Physics.SphereCast(rayInfo.ray.origin, interactionRadius, rayInfo.ray.direction, out RaycastHit hit,
                    rayInfo.dist, layerMask, QueryTriggerInteraction.Collide))
                return;
            
            if (!hit.transform.gameObject.EqualsOneOreMoreTags(interactionTags))
                return;

            if (!hit.transform.TryGetComponent(out Slot slot))
                return;

            if (_currentPayload == null && slot.HasPayload)
            {
                StartCarry(slot.RemoveSlot());
            }
            else if (_currentPayload != null && !slot.HasPayload)
            {
                ICarrieAble payload = StopCarryWrapper();
                slot.SetSlot(payload);
            }
        }

        
        
        public void StartCarry(ICarrieAble carry)
        {
            if (!carry.TryStartCarry(transform, this))
                return;
            PayloadPickedUpGameEvent?.RaiseEvent(this, carry);
            _currentPayload = carry;
        }

        private ICarrieAble StopCarryWrapper()
        {
            ICarrieAble payload = _currentPayload;
            StopCarry();
            return payload;
        }
        
        public void StopCarry()
        {
            if (_currentPayload == null)
                return;
            
            PayloadDroppedGameEvent?.RaiseEvent(this, _currentPayload);
            _currentPayload.OnStopCarry();
            _currentPayload = null;
        }
        
        private (Ray ray, float dist) GetMouseToWorldRay()
        {
            Ray ray = ScreenToWorldHelper.GetMouseToWorldRay(_mainCam);
            return (ray, ScreenToWorldHelper.GetRayDistanceToWorldSpaceForward(ray, _mainCam.transform.forward.SetY(0).normalized * interactionDistance.Value));
        }
    }
}

