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
        [SerializeField] private float ySnapPos = 1.12f;
        
        public event Action CarryStartedEvent;
        public event Action CarryStoppedEvent;

        private bool _isCarried;
        private Transform _carryTransform;
        private float _timeSinceLastDrop;

        private bool _isMixerMouse;
        private MixerMouse _mixerMouse;
        
        public ICarrieAbleMouse MouseCarry { get; protected set; }

        private float _transitionTimer;

        private float _startY;
        private Vector3 _startDiff;
        
        public bool IsCarried { get; set; }
        
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
            
            _startY = thisTransform.position.y;
            if(_isMixerMouse)
            {
                Vector3 targetPos = _carryTransform.position;
                Vector3 diff = thisTransform.position - targetPos;
                _startDiff = Vector3.Project(diff, _mixerMouse.MixerInteractionPlane.forward);
            }

            IsCarried = true;
            return true;
        }

        public void OnStopCarry()
        {
            if (!_isCarried)
                return;

            _timeSinceLastDrop = Time.timeSinceLevelLoad;
            _isCarried = false;
            _carryTransform = null;
            IsCarried = false;
            CarryStoppedEvent?.Invoke();
        }

        private void LateUpdate()
        {
            if (!_isCarried || _carryTransform == null)
                return;
            
            _transitionTimer += Time.deltaTime / transitionDuration;
            thisTransform.position = Vector3.Lerp(thisTransform.position, GetTargetPosition(), Mathf.Clamp01(_transitionTimer));
        }

        private Vector3 GetTargetPosition()
        {
            if(!_isMixerMouse)
                return _carryTransform.position;
            
            float zeroOne = Mathf.InverseLerp(_startY, 1.12f, thisTransform.position.y);
            Vector3 projected = _startDiff * (1 - zeroOne);
            
            return _carryTransform.position + projected;
        }
        
        public Vector3 GetPosition() => transform.position;
        public GameObject GetAttachedGameObject() => thisTransform.gameObject;
        public float GetLastCarryDropTimeSinceLevelLoad() => _timeSinceLastDrop;
    }
}
