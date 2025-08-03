using System;
using JohaToolkit.UnityEngine.DataStructures;
using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using JohaToolkit.UnityEngine.Timer;
using Scenes;
using UnityEngine;

public class Timer : MonoBehaviourSingleton<Timer>
{
    [SerializeField] private FloatVariable timerVariable;
    [SerializeField] private Transform timerUI;

    [SerializeField] private float timerduration;

    private CountdownTimer _timer;
    public void StartTimer()
    {
        timerUI.gameObject.SetActive(true);
        timerVariable.Value = timerduration;
        _timer = new(timerduration.Seconds());
        _timer.TimerFinished += () =>
        {
            timerVariable.Value = 0;
            GameplaySceneManager.Instance.LoadGameplayScene(GameplayScenes.TimerStopped, true);
        };
    }

    public void Update()
    {
        if (_timer == null)
            return;
        
        _timer.Tick(Time.deltaTime);
        timerVariable.Value = (float)_timer.RemainingTime.TotalSeconds;
    }
}
