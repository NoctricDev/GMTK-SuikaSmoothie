using System;
using Events;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button startWithTimerButton;
        [SerializeField] private Button startTutorialButton;
        [SerializeField] private GameplayScenes startGameplayScene;
        [SerializeField] private GameplayScenes startTutorialGameplayScene;
        [SerializeField] private StartGameTypeVariable startGameTypeVariable;

        private void Awake()
        {
            startButton.onClick.AddListener(OnFreeplayButtonClicked);
            startWithTimerButton.onClick.AddListener(OnStartWithTimerButtonClicked);
            startTutorialButton.onClick.AddListener(OnTutorialButtonClicked);
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveListener(OnFreeplayButtonClicked);
            startWithTimerButton.onClick.RemoveListener(OnStartWithTimerButtonClicked);
            startTutorialButton.onClick.RemoveListener(OnTutorialButtonClicked);
        }

        private void OnTutorialButtonClicked()
        {
            startGameTypeVariable.Value = StartGameType.Dummy;
            startGameTypeVariable.Value = StartGameType.Tutorial;
            StartGame(startTutorialGameplayScene);
        }

        private void OnFreeplayButtonClicked()
        {
            startGameTypeVariable.Value = StartGameType.Dummy;
            startGameTypeVariable.Value = StartGameType.Freeplay;
            StartGame();
        }

        private void OnStartWithTimerButtonClicked()
        {
            Timer.Instance.StartTimer();
            startGameTypeVariable.Value = StartGameType.Dummy;
            startGameTypeVariable.Value = StartGameType.Challenge;
            StartGame();
        }

        private void StartGame() => StartGame(startGameplayScene);
        
        private void StartGame(GameplayScenes gameplayScene)
        {
            GameplaySceneManager.Instance.LoadGameplayScene(gameplayScene);
        }

        public void QuitGame()
        {
#if UNITY_WEBGL
            Screen.fullScreen = false;
#endif
            Application.Quit();
        }
    }
}
