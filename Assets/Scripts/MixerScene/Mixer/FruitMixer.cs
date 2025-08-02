using System;
using System.Collections.Generic;
using Fruits;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MixerScene.Mixer
{
    public class FruitMixer : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private FruitCounter fruitCounter = null!;

        private void Awake()
        {
            fruitCounter.FruitCountChangedEvent += OnFruitCountChanged;
        }

        private void OnFruitCountChanged(FruitSO arg1, int arg2)
        {
            Debug.Log("Count Changed!");
            foreach ((FruitSO key, int value) in fruitCounter.FruitsInMixer)
            {
                Debug.Log("Fruit: " + key.name + ", Count: " + value);
            }
        }
    }
}
