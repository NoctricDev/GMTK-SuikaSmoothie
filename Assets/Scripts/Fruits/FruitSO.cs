using Sirenix.OdinInspector;
using UnityEngine;

namespace Fruits
{
    [CreateAssetMenu(fileName = "Fruit", menuName = "Scriptable Objects/Fruit")]
    public class FruitSO : ScriptableObject
    { 
        [SerializeField] private FruitType fruitType;
        [SerializeField, AssetSelector] private Fruit fruitPrefab;
        [SerializeField] private Sprite fruitIcon;
        [SerializeField] private float difficultyRating;
        [SerializeField] private int value;
        [SerializeField] private Color _smoothieTopColor = Color.red;
        [SerializeField] private Color _smoothieSideColor = Color.red;
        public Fruit FruitPrefab => fruitPrefab;
        public FruitType FruitType => fruitType;
        public Sprite FruitIcon => fruitIcon;
        public float DifficultyRating => difficultyRating;
        public int FruitValue => value;
        
        public Color SmoothieTopColor => _smoothieTopColor;
        public Color SmoothieSideColor => _smoothieSideColor;
    }
}
