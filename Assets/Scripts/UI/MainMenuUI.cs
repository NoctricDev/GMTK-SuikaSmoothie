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

        private void Awake()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            startWithTimerButton.onClick.AddListener(OnStartWithTimerButtonClicked);
        }

        private void OnStartWithTimerButtonClicked()
        {
            Timer.Instance.StartTimer();
            OnStartButtonClicked();
        }

        private void OnStartButtonClicked()
        {
            GameplaySceneManager.Instance.LoadGameplayScene(startGameplayScene);
        }
    }
}
