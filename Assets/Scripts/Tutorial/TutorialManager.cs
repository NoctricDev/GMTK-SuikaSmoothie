using Events;
using JohaToolkit.UnityEngine.DataStructures;
using UnityEngine;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviourSingleton<TutorialManager>
    {
        [SerializeField] private StartGameTypeVariable startGameTypeVariable;
        [SerializeField] private TutorialSteps tutorialSteps;
        private TutorialStep[] _tutorialSteps;
        private TutorialStep _currentStep;
        private int _currentStepIndex;

        protected override void Awake()
        {
            base.Awake();
            _tutorialSteps = tutorialSteps.Steps;
            startGameTypeVariable.OnValueChanged += OnStartGameTypeChanged;
        }

        private void OnDestroy()
        {
            startGameTypeVariable.OnValueChanged -= OnStartGameTypeChanged;
            _currentStep?.OnEndStep();
        }

        private void OnStartGameTypeChanged(StartGameType gameMode)
        {
            if(gameMode is StartGameType.Dummy or not StartGameType.Tutorial) 
                return;

            StartTutorial();
        }

        public void StartTutorial()
        {
            _currentStepIndex = -1;
            _currentStep = null;
            NextStep();
        }

        public void NextStep()
        {
            _currentStepIndex++;
            if (_currentStepIndex >= _tutorialSteps.Length)
            {
                EndTutorial();
                return;
            }
            _currentStep?.OnEndStep();
            _currentStep = _tutorialSteps[_currentStepIndex];
            _currentStep.StartStep();
        }

        public void EndTutorial()
        {
            
        }
    }
}
