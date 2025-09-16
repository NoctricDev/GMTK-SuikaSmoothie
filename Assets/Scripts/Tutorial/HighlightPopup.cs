using System;
using DG.Tweening;
using UnityEngine;

namespace Tutorial
{
    [RequireComponent(typeof(CanvasGroup))]
    public class HighlightPopup : BaseTutorialPopup
    {
        [SerializeField] private float fadeInDuration = 0.5f;
        [SerializeField] private Ease fadeInEase = Ease.OutSine;
        [SerializeField] private float fadeOutDuration = 0.5f;
        [SerializeField] private Ease fadeOutEase = Ease.InSine;
        
        CanvasGroup _canvasGroup;
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void Show(TutorialText tutorialText)
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1, fadeInDuration).SetEase(fadeInEase);
        }

        public override void Hide()
        {
            _canvasGroup.DOKill();
            _canvasGroup.DOFade(0, fadeOutDuration).SetEase(fadeOutEase).OnComplete(() => Destroy(gameObject));
        }
    }
}