#nullable enable
using UnityEngine;

namespace Fruits
{
    public class Fruit : MonoBehaviour
    {
        public const string FRUIT_LAYER = "Fruit";
        
        [HideInInspector] public bool requestedMerge;
        
        public FruitType FruitType { get; private set; }
        
        public void Init(FruitSO fruitSO)
        {
            FruitType = fruitSO.FruitType;
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (!other.transform.TryGetComponent(out Fruit otherFruit))
                return;
            
            if (requestedMerge || otherFruit.requestedMerge)
                return;
            
            if (!FruitMergeManager.Instance.CanMerge(FruitType, otherFruit.FruitType, out FruitSO? newFruit))
                return;
            
            requestedMerge = true;
            otherFruit.requestedMerge = true;
            FruitMergeManager.Instance.MergeFruits(this, otherFruit, newFruit);
        }
    }
}
