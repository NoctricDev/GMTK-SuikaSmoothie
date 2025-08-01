using Fruits;
using JetBrains.Annotations;
using JohaToolkit.UnityEngine.DataStructures;
using JohaToolkit.UnityEngine.ScriptableObjects.Events;
using UnityEngine;

namespace FruitBowlScene
{
    public class FruitMergeManager : MonoBehaviourSingleton<FruitMergeManager>
    {
        [SerializeField] private GameEvent fruitMergedGameEvent;
        [SerializeField] private RecipeSO[] recipes;

        public bool CanMerge(FruitType fruitA, FruitType fruitB, [CanBeNull] out FruitSO fruit)
        {
            foreach (RecipeSO recipe in recipes)
            {
                if(!recipe.IsValidCombination(fruitA, fruitB))
                    continue;
                fruit = recipe.ResultFruit;
                return true;
            }
            
            fruit = null;
            return false;
        }

        public void MergeFruits(Fruit fruitA, Fruit fruitB, FruitSO newFruit)
        {
            fruitMergedGameEvent?.RaiseEvent(this);
            Vector3 spawnPosition = (fruitA.transform.position + fruitB.transform.position) / 2;
            fruitA.OnMerge();
            fruitB.OnMerge();
            Destroy(fruitA.gameObject);
            Destroy(fruitB.gameObject);
            FruitFactory.SpawnFruit(newFruit, spawnPosition, Quaternion.identity, null);
        }
    }
}
