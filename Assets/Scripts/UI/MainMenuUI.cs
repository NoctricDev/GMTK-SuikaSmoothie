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
        [SerializeField] private GameplayScenes startGameplayScene;
        [SerializeField] private StartGameTypeVariable startGameTypeVariable;

        private void Awake()
        {
            startButton.onClick.AddListener(OnFreeplayButtonClicked);
            startWithTimerButton.onClick.AddListener(OnStartWithTimerButtonClicked);
        }

        private void OnFreeplayButtonClicked()
        {
            startGameTypeVariable.Value = StartGameType.Freeplay;
            StartGame();
        }

        private void OnStartWithTimerButtonClicked()
        {
            Timer.Instance.StartTimer();
            startGameTypeVariable.Value = StartGameType.Challenge;
            StartGame();
        }

        private void StartGame()
        {
            GameplaySceneManager.Instance.LoadGameplayScene(startGameplayScene);
        }
    }
}
