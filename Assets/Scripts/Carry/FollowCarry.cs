using System;
using FruitBowlScene;
using Mixer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Carry
{
    public class FollowCarry : MonoBehaviour, ICarrieAble
    {
        [Title("References")]
        [SerializeField] private Transform thisTransform;

        [Title("Settings")] 
        [SerializeField] private float transitionDuration = 0.2f;
        
        public event Action CarryStartedEvent;
        public event Action CarryStoppedEvent;

        private bool _isCarried;
        private Transform _carryTransform;
        private float _timeSinceLastDrop;

        private bool _isMixerMouse;
        private MixerMouse _mixerMouse;
        
        public ICarrieAbleMouse MouseCarry { get; protected set; }

        private float _transitionTimer;
        
        public bool TryStartCarry(Transform carryTransform, ICarrieAbleMouse mouseCarry)
        {
            _mixerMouse = mouseCarry as MixerMouse;
            if (_mixerMouse != null)
            {
                _isMixerMouse = true;
            }
            
            
            if (_isCarried)
                return false;
            MouseCarry = mouseCarry;
            _carryTransform = carryTransform;
            _isCarried = true;
            CarryStartedEvent?.Invoke();
            transform.SetParent(null);
            _transitionTimer = 0;
            return true;
        }

        public void OnStopCarry()
        {
            if (!_isCarried)
                return;

            _timeSinceLastDrop = Time.timeSinceLevelLoad;
            _isCarried = false;
            _carryTransform = null;
            CarryStoppedEvent?.Invoke();
        }

        private void LateUpdate()
        {
            if (!_isCarried || _carryTransform == null)
                return;

            Debug.Log($"{_carryTransform != null} + {_carryTransform.position}");
            
            _transitionTimer += Time.deltaTime / transitionDuration;
            thisTransform.position = Vector3.Lerp(thisTransform.position, GetTargetPosition(), Mathf.Clamp01(_transitionTimer));
        }

        private Vector3 GetTargetPosition()
        {
            if(!_isMixerMouse)
                return _carryTransform.position;

            Vector3 targetPos = _carryTransform.position;
            Vector3 diff = thisTransform.position - targetPos;
            Vector3 projected = Vector3.Project(diff, _mixerMouse.MixerInteractionPlane.forward);
            return targetPos + projected;
        }
        
        public Vector3 GetPosition() => transform.position;
        public GameObject GetAttachedGameObject() => thisTransform.gameObject;
        public float GetLastCarryDropTimeSinceLevelLoad() => _timeSinceLastDrop;
    }
}
