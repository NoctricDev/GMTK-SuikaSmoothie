using System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Carry
{
    public class FollowCarry : MonoBehaviour, ICarrieAble
    {
        [Title("References")]
        [SerializeField] private Transform thisTransform;

        [Title("Settings")] 
        [SerializeField] private bool resetTransform = true;
        
        public event Action CarryStartedEvent;
        public event Action CarryStoppedEvent;

        private bool _isCarried;
        private Transform _carryTransform;
        
        
        public bool TryStartCarry(Transform carryTransform)
        {
            if (_isCarried)
                return false;
            
            _carryTransform = carryTransform;
            _isCarried = true;
            CarryStartedEvent?.Invoke();
            transform.SetParent(null);
            return true;
        }

        public void StopCarry()
        {
            if (!_isCarried)
                return;
            
            _isCarried = false;
            _carryTransform = null;
            CarryStoppedEvent?.Invoke();
        }

        private void LateUpdate()
        {
            if (!_isCarried || _carryTransform == null)
                return;
            
            thisTransform.position = _carryTransform.position;
        }
        
        public Vector3 GetPosition() => transform.position;
        public GameObject GetAttachedGameObject() => thisTransform.gameObject;
    }
}
