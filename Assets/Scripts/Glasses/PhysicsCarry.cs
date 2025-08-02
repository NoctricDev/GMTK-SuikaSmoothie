using Carry;
using FruitBowlScene;
using MixerScene;
using UnityEngine;

namespace Glasses
{
    public class PhysicsCarry : PhysicsBasedMouseTarget, ICarrieAble
    {
        private ICarrieAbleMouse _mouseCarry;
        private bool _isCarried;
        private float _lastDropTime;
        public bool TryStartCarry(Transform carryTransform, ICarrieAbleMouse mouseCarry)
        {
            if (_isCarried) 
                return false;
            
            SetTargetMouse(carryTransform);
            
            _isCarried = true;
            return _isCarried;
        }

        public void OnStopCarry()
        {
            _isCarried = false;
            SetTargetMouse(transform);
            _lastDropTime = Time.timeSinceLevelLoad;
        }

        public GameObject GetAttachedGameObject() => gameObject;
        public float GetLastCarryDropTimeSinceLevelLoad() => _lastDropTime;
    }
}