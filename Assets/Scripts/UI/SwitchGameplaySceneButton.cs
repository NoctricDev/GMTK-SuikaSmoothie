using System;
using Scenes;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SwitchGameplaySceneButton : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private Button switchGameplaySceneButton;
        [SerializeField] private TextMeshProUGUI sceneNameText;
        
        [Title("Settings")]
        [SerializeField] private bool moveLeft;
        private void Start()
        {
            switchGameplaySceneButton.onClick.AddListener(OnSwitchGameplaySceneButtonClicked);
            GameplaySceneManager.Instance.CurrentSceneChangedEvent += OnCurrentSceneChanged;
        }

        private void OnCurrentSceneChanged(bool obj)
        {
            if (!obj)
                return;
            
            sceneNameText.text = GetNextScene(GameplaySceneManager.Instance.CurrentSceneName).ToString();
        }

        private GameplayScenes GetNextScene(GameplayScenes currentScene)
        {
            return GameplaySceneManager.Instance.CurrentSceneName switch
            {
                GameplayScenes.Bowl when moveLeft => GameplayScenes.Customer,
                GameplayScenes.Bowl when !moveLeft => GameplayScenes.Mixer,
                GameplayScenes.Mixer when moveLeft=> GameplayScenes.Bowl,
                GameplayScenes.Mixer when !moveLeft=> GameplayScenes.Customer,
                GameplayScenes.Customer when moveLeft=> GameplayScenes.Mixer,
                GameplayScenes.Customer when !moveLeft=> GameplayScenes.Bowl,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private void OnSwitchGameplaySceneButtonClicked()
        {
            GameplayScenes nextScene = GetNextScene(GameplaySceneManager.Instance.CurrentSceneName);
            GameplaySceneManager.Instance.LoadGameplayScene(nextScene);
        }
    }
}
