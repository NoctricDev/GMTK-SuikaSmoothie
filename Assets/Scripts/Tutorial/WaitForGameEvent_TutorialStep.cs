using JohaToolkit.UnityEngine.ScriptableObjects.Events;
using UnityEngine;

namespace Tutorial
{
        [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/TutorialSteps/WaitForGameEvent", fileName = "WaitForGameEvent_TutorialStep", order = 0)]
        public class WaitForGameEvent_TutorialStep : TutorialStep
        {
            [SerializeField] private GameEvent grabEvent;
            public override void StartStep()
            {
                grabEvent.Subscribe(OnGameEvent);
            }

            public override void OnEndStep()
            {
                grabEvent.Unsubscribe(OnGameEvent);
            }

            private void OnGameEvent(object arg1)
            {
                TutorialManager.Instance.NextStep();
            }
        }
    }