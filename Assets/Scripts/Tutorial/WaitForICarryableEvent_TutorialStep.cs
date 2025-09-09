using Carry;
using Events;
using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/TutorialSteps/WaitForICarryAbleGameEvent", fileName = "WaitForFruitSpawn_TutorialStep", order = 0)]
    public class WaitForICarryableEvent_TutorialStep : TutorialStep
    {
        [SerializeField] private GameEventICarrieAble grabEvent;
        public override void StartStep()
        {
            grabEvent.Subscribe(OnICarryAbleEvent);
        }

        public override void OnEndStep()
        {
            grabEvent.Unsubscribe(OnICarryAbleEvent);
        }

        private void OnICarryAbleEvent(object arg1, ICarrieAble arg2)
        {
            TutorialManager.Instance.NextStep();
        }
    }
}