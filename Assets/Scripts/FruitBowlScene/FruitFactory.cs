using Fruits;
using JetBrains.Annotations;
using UnityEngine;

namespace FruitBowlScene
{
    public static class FruitFactory
    {
        public static Fruit SpawnFruit(FruitSO fruitSO, Vector3? position, Quaternion? rotation, [CanBeNull] Transform parent)
        {
            Fruit fruit = Object.Instantiate(fruitSO.FruitPrefab);
            
            if(position != null)
                fruit.transform.position = position.Value;
            if (rotation != null)
                fruit.transform.rotation = rotation.Value;
            if (parent != null)
                fruit.transform.parent = parent;
            
            fruit.Init(fruitSO);
            return fruit;
        }
    }
}