using System;
using JohaToolkit.UnityEngine.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class AudioSettings : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        
        [SerializeField] private AudioMixerGroup masterAudioMixerGroup;
        [SerializeField] private AudioMixerGroup musicAudioMixerGroup;
        [SerializeField] private AudioMixerGroup sfxAudioMixerGroup;

        private const string ExposedMasterVolumeName = "mastervolume";
        private const string ExposedMusicVolumeName = "musicvolume";
        private const string ExposedSfxVolumeName = "sfxvolume";
        
        private void Awake()
        {
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        }

        private void Start()
        {
            SetSlider(masterVolumeSlider, masterAudioMixerGroup.audioMixer, ExposedMasterVolumeName);
            SetSlider(musicVolumeSlider, musicAudioMixerGroup.audioMixer, ExposedMusicVolumeName);
            SetSlider(sfxVolumeSlider, sfxAudioMixerGroup.audioMixer, ExposedSfxVolumeName);
        }

        private void SetSlider(Slider slider, AudioMixer mixer, string exposedVolumeName)
        {
            if (!mixer.GetFloat(exposedVolumeName, out float logarithmicVolume))
            {
                Debug.LogError("Something went wrong while getting volume from AudioMixer");
                return;
            }
            
            slider.SetValueWithoutNotify(ToLinearValue(logarithmicVolume));
        }

        private void OnDestroy()
        {
            masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
            musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            sfxVolumeSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
        }
        
        private void OnMasterVolumeChanged(float newValue)
        {
            SetVolume(masterAudioMixerGroup.audioMixer, newValue, ExposedMasterVolumeName);
        }

        private void OnMusicVolumeChanged(float newValue)
        {
            SetVolume(musicAudioMixerGroup.audioMixer, newValue, ExposedMusicVolumeName);
        }

        private void OnSfxVolumeChanged(float newValue)
        {
            SetVolume(sfxAudioMixerGroup.audioMixer, newValue, ExposedSfxVolumeName);
        }

        private void SetVolume(AudioMixer audioMixer, float linearVolume, string exposedVolumeName) => audioMixer.SetFloat(exposedVolumeName, AudioExtensions.ToLogarithmicValue(linearVolume));
    
        public float ToLinearValue(float logarithmicValue)
        {
            return Mathf.Pow(10f, logarithmicValue / 20f);
        }
    }
}
