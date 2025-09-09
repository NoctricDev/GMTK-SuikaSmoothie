using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class VideoSettings : MonoBehaviour, IInitOptions
    {
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private TMP_Dropdown dropDown;

        [SerializeField] private FullScreenMode[] allowedFullscreenModes;

        public void Init()
        {
            OptionsScreenUI.Instance.VisibilityChangedEvent += OnVisibilityChanged;
            fullscreenToggle.onValueChanged.AddListener(OnToggleFullscreenValueChanged);
            
#if UNITY_WEBGL
            dropDown.gameObject.SetActive(false);
#endif
#if !UNITY_WEBGL
            dropDown.onValueChanged.AddListener(OnFullscreenModeDropdownValueChanged);
            SetupFullscreenModeOptions();
#endif
        }

#if !UNITY_WEBGL
        private void SetupFullscreenModeOptions()
        {
            dropDown.ClearOptions();
            dropDown.AddOptions(allowedFullscreenModes.Select(m => m.ToString()).ToList());

            if (allowedFullscreenModes.Contains(Screen.fullScreenMode))
            {
                dropDown.value = (int)Screen.fullScreenMode;
            }
            else
            {
                Debug.LogError("currentFullscreenMode is not in allowedFullscreenModes");
                dropDown.value = 0;
                Screen.fullScreenMode = allowedFullscreenModes[0];
            }
            
        }
#endif

        private void OnDestroy()
        {
            OptionsScreenUI.Instance.VisibilityChangedEvent -= OnVisibilityChanged;
            fullscreenToggle.onValueChanged.RemoveListener(OnToggleFullscreenValueChanged);
#if !UNITY_WEBGL
            dropDown.onValueChanged.RemoveListener(OnFullscreenModeDropdownValueChanged);
#endif
        }

        private void OnVisibilityChanged(bool isShown)
        {
            fullscreenToggle.isOn = Screen.fullScreen;
        }

        private void OnToggleFullscreenValueChanged(bool newState)
        {
            SetFullscreen(newState);
        }
        
        private void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            fullscreenToggle.isOn = isFullscreen;
        }
        
#if !UNITY_WEBGL
        private void OnFullscreenModeDropdownValueChanged(int newValue)
        {
            Screen.fullScreenMode = allowedFullscreenModes[newValue];
            SetFullscreen(true);
        }
#endif
    }
}
