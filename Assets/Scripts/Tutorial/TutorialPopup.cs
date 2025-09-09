using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Tutorial
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TutorialPopup : BaseTutorialPopup
    {
        [Title("References")]
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private TextMeshProUGUI contentText;
        
        [Title("Settings")]
        [SerializeField] private float fadeInDuration = 0.5f;
        [SerializeField] private Ease fadeInEase = Ease.OutSine;
        [SerializeField] private float fadeOutDuration = 0.5f;
        [SerializeField] private Ease fadeOutEase = Ease.InSine;

        [SerializeField] private Vector2 hidePos;
        [SerializeField] private Vector2 showPos;
        
        private CanvasGroup _canvasGroup;
        private Sequence _activeSequence;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void Show(TutorialText tutorialText)
        {
            headerText.text = tutorialText.HeaderText;
            contentText.text = tutorialText.ContentText;
            _canvasGroup.alpha = 0f;
            transform.localPosition = hidePos;
            _activeSequence = GetShowSequence();
            _activeSequence.Play();
        }

        public override void Hide()
        {
            _activeSequence.Kill();
            _activeSequence = GetHideSequence();
            _activeSequence.Play().onComplete += () => Destroy(gameObject);
        }
        
        private Sequence GetShowSequence()
        {
            return DOTween.Sequence()
                .Append(_canvasGroup.DOFade(1f, fadeInDuration).SetEase(fadeInEase))
                .Join(transform.DOLocalMove(showPos, fadeInDuration).SetEase(fadeInEase))
                ;
        }

        private Sequence GetHideSequence()
        {
            return DOTween.Sequence()
                .Append(_canvasGroup.DOFade(0f, fadeInDuration).SetEase(fadeOutEase))
                .Join(transform.DOLocalMove(hidePos, fadeInDuration).SetEase(fadeOutEase))
                ;
        }
    }
}