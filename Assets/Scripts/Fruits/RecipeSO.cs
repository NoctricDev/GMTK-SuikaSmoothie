using UnityEngine;

namespace Fruits
{
    [CreateAssetMenu(fileName = "FruitRecipe", menuName = "Scriptable Objects/FruitRecipe")]
    public class RecipeSO : ScriptableObject
    {
        [SerializeField] private FruitType fruitA;
        [SerializeField] private FruitType fruitB;
        [SerializeField] private FruitSO resultFruit;
        
        public FruitType FruitA => fruitA;
        public FruitType FruitB => fruitB;
        public FruitSO ResultFruit => resultFruit;
        
        public bool IsValidCombination(FruitType a, FruitType b)
        {
            return (a == fruitA && b == fruitB) || (a == fruitB && b == fruitA);
        }
    }
}