using System;
using System.Linq;
using Fruits;
using Glasses;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MixerScene.Mixer
{
    public class FruitMixer : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private FruitCounter fruitCounter = null!;

        [SerializeField] private Slot glassSlot = null!;

        public event Action<SmoothieContent> MixerEmpty;

        private void Awake()
        {
            fruitCounter.FruitCountChangedEvent += OnFruitCountChanged;
        }

        private void OnFruitCountChanged(FruitSO fruitData, int currentCount)
        {
            return;
            Debug.Log("Count Changed!");
            foreach ((FruitSO key, int value) in fruitCounter.FruitsInMixer)
            {
                Debug.Log("Fruit: " + key.name + ", Count: " + value);
            }
        }

        public void MixerButtonPressed()
        {
            if (fruitCounter.FruitsObjectsInMixer.Count == 0)
            {
                Debug.Log("[Mixer] No fruits in mixer, cannot mix!");
                return;
            }
            Glass glassToFill = glassSlot.CurrentCarrieAble as Glass;
            if (glassToFill == null)
            {
                EmptyMixer();
            }
            else if(glassToFill.IsEmpty)
            {
                FillGlass(glassToFill);
                EmptyMixer();
            }
            else
            {
                Debug.Log("[Mixer] Glass is not empty, cannot fill it with smoothie!");
            }
        }

        public void EmptyMixer()
        {
            MixerEmpty?.Invoke(GetSmoothieContent());
            fruitCounter.EmptyMixer();
        }

        private void FillGlass(Glass glassToFill)
        {
            glassSlot.IsLocked = true;
            SmoothieContent smoothieContent = GetSmoothieContent();
            if (!glassToFill.TrySetContent(smoothieContent))
            {
                Debug.LogError("[Mixer] Failed to fill glass with smoothie content! This should have been caught earlier!");
            }
            glassSlot.IsLocked = false;
        }

        [CanBeNull]
        private SmoothieContent GetSmoothieContent()
        {
            return fruitCounter.FruitsInMixer.Count == 0 ? null : new SmoothieContent(fruitCounter.FruitsInMixer.ToDictionary(k => k.Key, v => v.Value));
        }
    }
}
