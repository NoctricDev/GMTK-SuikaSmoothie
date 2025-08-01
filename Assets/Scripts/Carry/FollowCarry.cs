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
        
        public event Action CarryStartedEvent;
        public event Action CarryStoppedEvent;

        private bool _isCarried;
        private Transform _carryTransform;
        
        
        public bool TryStartCarry(Transform carryTransform, [CanBeNull] out ICarrieAble carrieAble)
        {
            carrieAble = null;
            if (_isCarried)
                return false;
            
            _carryTransform = carryTransform;
            _isCarried = true;
            CarryStartedEvent?.Invoke();
            carrieAble = this;
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

        private void Update()
        {
            if (!_isCarried || _carryTransform == null)
                return;
            
            thisTransform.position = _carryTransform.position;
        }
    }
}
