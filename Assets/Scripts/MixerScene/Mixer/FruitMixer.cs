using System;
using System.Linq;
using Carry;
using Fruits;
using Glasses;
using JetBrains.Annotations;
using JohaToolkit.UnityEngine.ScriptableObjects.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MixerScene.Mixer
{
    public class FruitMixer : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private FruitCounter fruitCounter = null!;

        [SerializeField] private GameEvent mixerGlassSlotPlacedEvent;
        [SerializeField] private GameEvent mixerFruitCountChangedEvent;
        [SerializeField] private GameEvent mixerMixedEvent;

        [SerializeField] private Slot glassSlot = null!;

        public event Action<SmoothieContent> MixerEmpty;

        private void Awake()
        {
            fruitCounter.FruitCountChangedEvent += OnFruitCountChanged;
            glassSlot.SlotContentChangedEvent += OnGlassSlotChanged;
        }

        private void OnGlassSlotChanged(ICarrieAble obj, bool added)
        {
            if (!added)
                return;
            mixerGlassSlotPlacedEvent?.RaiseEvent(this);
        }

        private void OnFruitCountChanged(FruitSO fruitData, int currentCount)
        {
            if(currentCount > 0)
                mixerFruitCountChangedEvent?.RaiseEvent(this);
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
            if (glassToFill == null || !glassToFill.IsEmpty)
            {
                EmptyMixer();
                return;
            }
            FillGlass(glassToFill);
            EmptyMixer();
            mixerMixedEvent?.RaiseEvent(this);
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
