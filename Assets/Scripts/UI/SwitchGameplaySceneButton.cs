using Scenes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SwitchGameplaySceneButton : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private Button switchGameplaySceneButton;
        
        [Title("Settings")]
        [SerializeField] private GameplayScenes sceneToLoad;
        private void Awake()
        {
            switchGameplaySceneButton.onClick.AddListener(() => GameplaySceneManager.Instance.LoadGameplayScene(sceneToLoad));
        }
    }
}
