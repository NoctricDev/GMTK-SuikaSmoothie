using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/TutorialSteps/WaitForLeftClick", fileName = "WaitForLeftClick_TutorialStep", order = 0)]
    public class WaitForLeftClick_TutorialStep : TutorialStep
    {
        private CancellationTokenSource _cancellationTokenSource;
        public override async void StartStep()
        {
            _cancellationTokenSource = new();
            try
            {
                await AwaitLeftClick();
                if (_cancellationTokenSource.IsCancellationRequested)
                    return;
                TutorialManager.Instance.NextStep();
            }
            catch (OperationCanceledException e)
            {
                return;
            }
            finally
            {
                _cancellationTokenSource = null;
            }
        }
        

        public override void OnEndStep()
        {
            _cancellationTokenSource?.Cancel();
        }

        private async Awaitable AwaitLeftClick()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                if (Mouse.current.leftButton.wasReleasedThisFrame)
                    return;
                await Awaitable.NextFrameAsync(_cancellationTokenSource.Token);
            }
            
            _cancellationTokenSource.Token.ThrowIfCancellationRequested();
        }
    }
}