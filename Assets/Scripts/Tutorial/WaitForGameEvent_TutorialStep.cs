using System.Collections.Generic;
using JohaToolkit.UnityEngine.ScriptableObjects.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tutorial
{
        [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/TutorialSteps/WaitForGameEvent", fileName = "WaitForGameEvent_TutorialStep", order = 0)]
        public class WaitForGameEvent_TutorialStep : TutorialStep
        {
            [SerializeField] private GameEvent[] eventsToWaitFor;
            [SerializeField] private bool requireAllEvents = true;
            [SerializeField, ShowIf(nameof(requireAllEvents))] private int minCallCount = 1;
            [SerializeField, ShowIf(nameof(requireAllEvents))] private int minDifferentSendersCount = 1;
            [SerializeField, ShowIf(nameof(requireAllEvents))] private int minDifferentSendersCallCount = 1;

            private Dictionary<object, int> _senders;
            private int _callCount;
            
            public override void StartStep()
            {
                _senders = new();
                _callCount = 0;
                foreach (GameEvent gameEvent in eventsToWaitFor)
                {
                    gameEvent.Subscribe(OnGameEvent);
                }
            }

            public override void OnEndStep()
            {
                foreach (GameEvent gameEvent in eventsToWaitFor)
                {
                    gameEvent.Unsubscribe(OnGameEvent);
                }
            }

            private void OnGameEvent(object sender)
            {
                if (!_senders.TryAdd(sender, 1))
                {
                    _senders[sender]++;
                }
                _callCount++;
                
                if (!ValidateGameEventConditions())
                    return;

                TutorialManager.Instance.NextStep();
            }

            private bool ValidateGameEventConditions()
            {
                if (!requireAllEvents)
                    return true;
                
                if (_callCount < minCallCount)
                    return false;
                if (_senders.Count < minDifferentSendersCount)
                    return false;
                foreach ((object _, int value) in _senders)
                {
                    if (value < minDifferentSendersCallCount)
                        return false;
                }

                return true;
            }
        }
    }