using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OpenUrlButton : MonoBehaviour
    {
        [SerializeField] private string url;

        private Button _button;
    
        private void Awake()
        {
            _button = GetComponent<Button>();
            if (_button == null)
            {
                Debug.LogError("OpenUrlButton requires a Button component.");
                return;
            }
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            Application.OpenURL(url);
        }
    }
}
