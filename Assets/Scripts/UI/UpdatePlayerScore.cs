using System;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UpdatePlayerScore : MonoBehaviour
    {
        [SerializeField] private IntVariable playerScore;
        [SerializeField] private TextMeshProUGUI playerScoreText;

        private void Awake()
        {
            playerScore.OnValueChanged += UpdatePlayerScoreText;
            UpdatePlayerScoreText(playerScore.Value);
        }

        private void OnDestroy()
        {
            playerScore.OnValueChanged -= UpdatePlayerScoreText;
        }

        private void UpdatePlayerScoreText(int newScore) => playerScoreText.text = newScore.ToString();
    }
}
