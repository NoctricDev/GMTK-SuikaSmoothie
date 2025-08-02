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
            if (!glassToFill)
            {
                // Empty mixer
            }
            else
            {
                // Fill Glass
            }
        }

        public void EmptyMixer()
        {
            fruitCounter.EmptyMixer();
        }
    }
}
