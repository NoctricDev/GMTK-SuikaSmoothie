using Scenes;
using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/TutorialSteps/WaitForGameplayScene", fileName = "WaitForGameplayScene_TutorialStep", order = 0)]
    public class WaitForGameplayScene_TutorialStep : TutorialStep
    {
        [SerializeField] private GameplayScenes sceneToWaitFor;
        [SerializeField] private bool waitForLoadEnd = true;
        
        public override void StartStep()
        {
            if(GameplaySceneManager.Instance.CurrentScene.Scene.Equals(sceneToWaitFor))
                NextStep();

            GameplaySceneManager.Instance.CurrentSceneChangedEvent += OnCurrentSceneChanged;
        }

        public override void OnEndStep()
        {
            GameplaySceneManager.Instance.CurrentSceneChangedEvent -= OnCurrentSceneChanged;
        }

        private void OnCurrentSceneChanged(bool loadEnd)
        {
            if (GameplaySceneManager.Instance.CurrentScene.Scene != sceneToWaitFor)
                return;
            if(loadEnd == waitForLoadEnd)
                NextStep();
        }

        private void NextStep()
        {
            TutorialManager.Instance.NextStep();
        }
    }
}