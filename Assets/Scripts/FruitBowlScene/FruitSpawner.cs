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

        public void SpawnFruit(FruitSO fruitSO, Vector3 position)
        {
            FruitFactory.SpawnFruit(fruitSO, position, Quaternion.identity, null);
            // TODO: INIT Fruit with the FruitSO data
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
    }
}