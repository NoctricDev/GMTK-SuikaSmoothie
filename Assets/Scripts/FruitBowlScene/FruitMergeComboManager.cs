using Carry;
using Events;
using JohaToolkit.UnityEngine.ScriptableObjects.Events;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FruitBowlScene
{
    public class FruitMergeComboManager : MonoBehaviour
    {
        [Title("Events")]
        [SerializeField] private GameEvent fruitMergeEvent;
        [SerializeField] private GameEventICarrieAble payloadDroppedEvent;
        [SerializeField] private IntVariable currentComboCounter;
        
        private void Awake()
        {
            fruitMergeEvent.Subscribe(OnFruitMerge);
            payloadDroppedEvent.Subscribe(OnPayloadDropped);
        }

        private void OnPayloadDropped(object sender, ICarrieAble _)
        {
            currentComboCounter.Value = 0;
        }

        private void OnFruitMerge(object _)
        {
            currentComboCounter.Value++;
        }
    }
}
