using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class ReloadSceneButton : MonoBehaviour
    {
        [SerializeField] private Button reloadSceneButton;

        private void Awake()
        {
            reloadSceneButton.onClick.AddListener(OnReloadSceneButtonClicked);
        }

        private void OnReloadSceneButtonClicked()
        {
            LoadSceneManagement.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
