using DG.Tweening;
using Glasses;
using MixerScene.Mixer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MixerScene
{
    public class MixerVisuals : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private FruitMixer fruitMixer;
        [SerializeField] private Material mixerMaterial;
        
        [Title("Settings")]
        [SerializeField] private float fillDuration = 0.2f;
        [SerializeField] private Ease fillEase;
        [SerializeField] private float emptyDuration = 0.2f;
        [SerializeField] private Ease emptyEase;
        [SerializeField, Range(0,1)] private float fillAmount = 1f;
        
        private int _fillID = Shader.PropertyToID("_Fill");
        private int _topColorID = Shader.PropertyToID("_TopColor");
        private int _sideColorID = Shader.PropertyToID("_SideColor");
        
        private Sequence _sequence;

        private void Start()
        {
            fruitMixer.MixerEmpty += OnMixerEmpty;
        }

        private void OnMixerEmpty(SmoothieContent content)
        {
            mixerMaterial.DOKill();
            mixerMaterial.SetColor(_topColorID, content.GetTopColor());
            mixerMaterial.SetColor(_sideColorID, content.GetSideColor());
            mixerMaterial.SetFloat(_fillID, 0);
            GetSequence().Play();
        }

        private Sequence GetSequence()
        {
            _sequence = DOTween.Sequence()
                .Append(mixerMaterial.DOFloat(fillAmount, _fillID, fillDuration).SetEase(fillEase))
                .Append(mixerMaterial.DOFloat(0, _fillID, 0.2f).SetEase(emptyEase));
            return _sequence;
        }
    }
}
