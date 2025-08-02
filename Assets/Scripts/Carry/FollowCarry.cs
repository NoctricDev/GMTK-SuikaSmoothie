using System;
using FruitBowlScene;
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
        
        public ICarrieAbleMouse MouseCarry { get; protected set; }

        private float _transitionTimer;
        
        public bool TryStartCarry(Transform carryTransform, ICarrieAbleMouse mouseCarry)
        {
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

            _transitionTimer += Time.deltaTime / transitionDuration;
            thisTransform.position = Vector3.Lerp(thisTransform.position, _carryTransform.position, Mathf.Clamp01(_transitionTimer));
        }
        
        public Vector3 GetPosition() => transform.position;
        public GameObject GetAttachedGameObject() => thisTransform.gameObject;
        public float GetLastCarryDropTimeSinceLevelLoad() => _timeSinceLastDrop;
    }
}
