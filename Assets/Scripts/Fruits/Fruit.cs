#nullable enable
using Carry;
using FruitBowlScene;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Fruits
{
    public class Fruit : SerializedMonoBehaviour
    {
        public const string FRUIT_LAYER = "Fruit";
        public const string FRUIT_MERGEREGION_TAG = "FruitMergeRegion";
        
        [HideInInspector] public bool requestedMerge;
        private bool _canMerge;
        private bool _isCarried;
        private Rigidbody _rb = null!;
        
        public FruitType FruitType { get; private set; }
        public FruitSO FruitSO { get; private set; } = null!;
        
        public bool CanMerge => _canMerge && !requestedMerge;

        private FollowCarry? _followCarry;
        
        public void Init(FruitSO fruitSO)
        {
            _rb = GetComponent<Rigidbody>();
            FruitSO = fruitSO;
            FruitType = fruitSO.FruitType;
            _followCarry = GetComponent<FollowCarry>();
            if (_followCarry != null)
            {
                _followCarry.CarryStartedEvent += OnCarryStarted;
                _followCarry.CarryStoppedEvent += OnCarryStopped;
            }
        }

        private void OnCarryStarted()
        {
            _rb.isKinematic = true;
            _isCarried = true;
        }

        private void OnCarryStopped()
        {
            if (!_rb)
                return;
            _rb.isKinematic = false;
            _isCarried = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!CanMerge || !other.transform.TryGetComponent(out Fruit otherFruit) || !otherFruit.CanMerge)
                return;
            
            if (!FruitMergeManager.Instance.CanMerge(FruitType, otherFruit.FruitType, out FruitSO? newFruit))
                return;
            
            requestedMerge = true;
            otherFruit.requestedMerge = true;
            FruitMergeManager.Instance.MergeFruits(this, otherFruit, newFruit);
        }

        public void OnMerge()
        {
            if (_isCarried && _followCarry)
            {
                _followCarry.MouseCarry.StopCarry();
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(FRUIT_MERGEREGION_TAG))
                return;
            _canMerge = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(FRUIT_MERGEREGION_TAG))
                return;
            _canMerge = false;
        }
    }
}
