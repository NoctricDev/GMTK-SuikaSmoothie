using System;
using Fruits;
using UnityEngine;

namespace FruitBowlScene
{
    public class FruitSpawner : MonoBehaviour
    {
        private void Awake()
        {
        }

        public void SpawnFruit(FruitSO fruitSO)
        {
            FruitFactory.SpawnFruit(fruitSO, transform.position, Quaternion.identity, null);
            // TODO: INIT Fruit with the FruitSO data
        }
    }
}