using System.Collections.Generic;
using Fruits;
using JoHaToolkit.UnityEngine.CheatConsole;
using JohaToolkit.UnityEngine.DataStructures;
using UnityEngine;
using UnityEngine.Rendering;

namespace FruitBowlScene
{
    public class FruitCheats : MonoBehaviourSingleton<FruitCheats>
    {
        [SerializeField] private FruitSpawner fruitSpawner;
        [SerializeField] private SerializedDictionary<FruitType, FruitSO> _fruits;
        
        [CheatCommand("Spawn Fruit", "Spawns a fruit of the specified type")]
        public static void SpawnFruit(FruitType fruitType)
        {
            if (!Instance._fruits.TryGetValue(fruitType, out FruitSO fruitSO))
            {
                Debug.LogWarning("Fruit type not found in dictionary");
                return;
            }
            Instance.fruitSpawner.SpawnFruit(fruitSO);
        }

        [CheatCommand]
        public static void PrintAllFruitTypes()
        {
            Debug.Log("Available cheat Fruit types:");
            foreach (FruitType fruitsKey in Instance._fruits.Keys)
            {
                Debug.Log(fruitsKey.ToString());
            }
        }
    }
}
