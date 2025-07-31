using Sirenix.OdinInspector;
using UnityEngine;

namespace Fruits
{
    [CreateAssetMenu(fileName = "Fruit", menuName = "Scriptable Objects/Fruit")]
    public class FruitSO : ScriptableObject
    { 
        [SerializeField] private FruitType fruitType;
        [SerializeField, AssetSelector] private Fruit fruitPrefab;
        public Fruit FruitPrefab => fruitPrefab;
        public FruitType FruitType => fruitType;
    }
}
