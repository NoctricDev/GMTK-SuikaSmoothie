using System.Linq;
using Fruits;
using Glasses;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MixerScene.Mixer
{
    public class FruitMixer : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private FruitCounter fruitCounter = null!;

        [SerializeField] private Slot glassSlot = null!;

        private void Awake()
        {
            fruitCounter.FruitCountChangedEvent += OnFruitCountChanged;
        }

        private void OnFruitCountChanged(FruitSO fruitData, int currentCount)
        {
            Debug.Log("Count Changed!");
            foreach ((FruitSO key, int value) in fruitCounter.FruitsInMixer)
            {
                Debug.Log("Fruit: " + key.name + ", Count: " + value);
            }
        }

        public void MixerButtonPressed()
        {
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
            fruitCounter.EmptyMixer();
        }

        private void FillGlass(Glass glassToFill)
        {
            glassSlot.IsLocked = true;
            SmoothieContent smoothieContent = new(fruitCounter.FruitsInMixer.ToDictionary(k => k.Key, v => v.Value));
            if (!glassToFill.TrySetContent(smoothieContent))
            {
                Debug.LogError("[Mixer] Failed to fill glass with smoothie content! This should have been caught earlier!");
            }
            glassSlot.IsLocked = false;
        }
    }
}
