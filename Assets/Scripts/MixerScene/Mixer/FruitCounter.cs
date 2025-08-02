using System;
using System.Collections.Generic;
using Fruits;
using UnityEngine;

namespace MixerScene.Mixer
{
    public class FruitCounter : MonoBehaviour
    {
        private readonly Dictionary<FruitSO, int> _fruitsInMixer = new();
        
        public IReadOnlyDictionary<FruitSO, int> FruitsInMixer => _fruitsInMixer;
        public IReadOnlyList<Fruit> FruitsObjectsInMixer => _fruitObjectsInMixer;
        private readonly List<Fruit> _fruitObjectsInMixer = new();
        
        public event Action<FruitSO, int> FruitCountChangedEvent;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Fruit fruit))
                return;
            _fruitsInMixer.Add(fruit.FruitSO, _fruitsInMixer.TryGetValue(fruit.FruitSO, out int value) ? value + 1 : 1);
            _fruitObjectsInMixer.Add(fruit);
            FruitCountChangedEvent?.Invoke(fruit.FruitSO, _fruitsInMixer[fruit.FruitSO]);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Fruit fruit))
                return;
            if (!_fruitsInMixer.TryGetValue(fruit.FruitSO, out int value)) 
                return;
            
            _fruitsInMixer[fruit.FruitSO] = value - 1;
            _fruitObjectsInMixer.Remove(fruit);
            FruitCountChangedEvent?.Invoke(fruit.FruitSO, _fruitsInMixer[fruit.FruitSO]);
            if (_fruitsInMixer[fruit.FruitSO] <= 0)
                _fruitsInMixer.Remove(fruit.FruitSO);
        }

        public void EmptyMixer()
        {
            for (int i = _fruitObjectsInMixer.Count - 1; i >= 0; i--)
            {
                Destroy(_fruitObjectsInMixer[i].gameObject);
            }
            _fruitObjectsInMixer.Clear();
            _fruitsInMixer.Clear();
        }
    }
}
