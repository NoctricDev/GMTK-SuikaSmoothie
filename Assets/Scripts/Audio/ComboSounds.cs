using System;
using JohaToolkit.UnityEngine.Audio;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Audio
{
    public class ComboSounds : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private IntVariable comboCount;

        [SerializeField] private SoundDataAsset[] comboSounds;

        private void Awake()
        {
            comboCount.OnValueChanged += OnComboCountChanged;
        }

        private void OnDestroy()
        {
            comboCount.OnValueChanged -= OnComboCountChanged;
        }

        private void OnComboCountChanged(int newCount)
        {
            Debug.Log($"Play sound for combo count {comboCount} on Gameobject {gameObject.name}");
            if (newCount == 0)
                return;
            int soundToPlay = Mathf.Clamp(newCount - 1, 0, comboSounds.Length - 1);
            SoundManager.Instance.Play(comboSounds[soundToPlay]);
        }
    }
}
