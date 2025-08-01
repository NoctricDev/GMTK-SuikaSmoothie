using System;
using DG.Tweening;
using Scenes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI
{
    public class SceneUI : MonoBehaviour, IGameplaySceneObject
    {
        [Title("References")]
        [SerializeField] private CanvasGroup canvasGroup;
        
        [Title("Settings")]
        [SerializeField] private float fadeDuration = 0.5f;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void LoadEnd()
        {
            canvasGroup.DOKill();
            canvasGroup.alpha = 0;
            gameObject.SetActive(true);
            canvasGroup.DOFade(1, fadeDuration);
        }

        public void Unload()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(0, fadeDuration).onComplete = () =>
            {
                gameObject.SetActive(false);
            };
        }
    }
}
