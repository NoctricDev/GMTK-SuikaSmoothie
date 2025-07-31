using System;
using FruitBowlScene;
using JetBrains.Annotations;
using JohaToolkit.UnityEngine.DataStructures;
using UnityEngine;

namespace Fruits
{
    public class FruitMergeManager : MonoBehaviourSingleton<FruitMergeManager>
    {
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
            Vector3 spawnPosition = (fruitA.transform.position + fruitB.transform.position) / 2;
            Destroy(fruitA.gameObject);
            Destroy(fruitB.gameObject);
            FruitFactory.SpawnFruit(newFruit, spawnPosition, Quaternion.identity, null);
        }
    }
}
