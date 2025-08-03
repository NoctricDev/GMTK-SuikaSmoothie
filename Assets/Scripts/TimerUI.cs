using System;
using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private FloatVariable timerVariable;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        SetText(timerVariable.Value.Seconds());
        timerVariable.OnValueChanged += (newValue) => SetText(newValue.Seconds());
    }

    private void SetText(TimeSpan time)
    {
        timerText.text = time.ToString(@"mm\:ss");
    }
}
