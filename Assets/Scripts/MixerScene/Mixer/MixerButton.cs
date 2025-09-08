using DG.Tweening;
using UnityEngine;

namespace MixerScene.Mixer
{
    public class MixerButton : MonoBehaviour
    {
        [SerializeField] private Transform mixerButtonTransform;
        [SerializeField] private float buttonPressDistance = 0.1f;
        [SerializeField] private float buttonPressDuration = 0.2f;
        [SerializeField] private Ease buttonPressEase = Ease.OutSine;
        [SerializeField] private float buttonReleaseDuration = 0.2f;
        [SerializeField] private Ease buttonReleaseEase = Ease.InSine;

        [SerializeField] private float buttonPressedTime = 0.1f;
        
        Sequence _buttonPressSequence;

        private Sequence CreateButtonPressSequence()
        {
            return DOTween.Sequence()
                    .Append(mixerButtonTransform.DOLocalMoveX(buttonPressDistance, buttonPressDuration).SetEase(buttonPressEase))
                    .AppendInterval(buttonPressedTime)
                    .Append(mixerButtonTransform.DOLocalMoveX(0, buttonReleaseDuration).SetEase(buttonReleaseEase))
                ;
        }

        public void OnMixerButtonPressed()
        {
            _buttonPressSequence?.Kill();
            _buttonPressSequence = CreateButtonPressSequence();
            _buttonPressSequence.Play();
        }
    }
}