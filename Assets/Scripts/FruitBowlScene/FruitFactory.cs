using Fruits;
using JetBrains.Annotations;
using UnityEngine;

namespace FruitBowlScene
{
    public static class FruitFactory
    {
        public static Fruit SpawnFruit(FruitSO fruitSO, Vector3 position, Quaternion rotation, [CanBeNull] Transform parent, bool spawnProtection)
        {
            Fruit fruit = Object.Instantiate(fruitSO.FruitPrefab, position, rotation);
            
            if (parent != null)
                fruit.transform.parent = parent;
            
            fruit.Init(fruitSO, spawnProtection);
            return fruit;
        }
    }
}