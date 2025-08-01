#nullable enable
using Carry;
using UnityEngine;

namespace Fruits
{
    public class Fruit : MonoBehaviour
    {
        public const string FRUIT_LAYER = "Fruit";
        public const string FRUIT_MERGEREGION_TAG = "FruitMergeRegion";
        
        [HideInInspector] public bool requestedMerge;
        private bool _canMerge;
        private Rigidbody _rb = null!;
        
        public FruitType FruitType { get; private set; }
        public FruitSO FruitSO { get; private set; } = null!;

        public void Init(FruitSO fruitSO)
        {
            _rb = GetComponent<Rigidbody>();
            FruitSO = fruitSO;
            FruitType = fruitSO.FruitType;
            FollowCarry? followCarry = GetComponent<FollowCarry>();
            if (followCarry != null)
            {
                followCarry.CarryStartedEvent += OnCarryStarted;
                followCarry.CarryStoppedEvent += OnCarryStopped;
            }
        }

        private void OnCarryStarted()
        {
            _rb.isKinematic = true;
        }

        private void OnCarryStopped()
        {
            _rb.isKinematic = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!_canMerge || !other.transform.TryGetComponent(out Fruit otherFruit))
                return;
            
            if (requestedMerge || otherFruit.requestedMerge)
                return;
            
            if (!FruitMergeManager.Instance.CanMerge(FruitType, otherFruit.FruitType, out FruitSO? newFruit))
                return;
            
            requestedMerge = true;
            otherFruit.requestedMerge = true;
            FruitMergeManager.Instance.MergeFruits(this, otherFruit, newFruit);
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
