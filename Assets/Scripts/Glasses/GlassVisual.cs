using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Fruits;
using JohaToolkit.UnityEngine.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Glasses
{
    public class GlassVisual : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private Glass glass;

        [SerializeField] private Shader liquidShader;
        [SerializeField] private MeshRenderer liquidRenderer;
        [SerializeField] private float glassFillTime;
        [SerializeField] private Ease glassFillEase;
        [SerializeField] private float glassEmptyTime;
        [SerializeField] private Ease glassEmptyEase;

        Material _liquidMaterial;
        private int _fillID = Shader.PropertyToID("_Fill");
        private int _topColorID = Shader.PropertyToID("_TopColor");
        private int _sideColorID = Shader.PropertyToID("_SideColor");
        
        private void Awake()
        {
            _liquidMaterial = new Material(liquidShader);
            liquidRenderer.SetMaterials(new List<Material> { _liquidMaterial });
            SetFillContent(0, Ease.Flash,0, null);
            glass.GlassContentSetEvent += (content) => SetFillContent(glassFillTime, glassFillEase, 1, content as SmoothieContent);
            glass.GlassContentClearEvent += (_) => SetFillContent(glassEmptyTime, glassFillEase, 0, null);
        }

        private void SetFillContent(float time, Ease ease, float fillAmount, SmoothieContent content)
        {
            if (content == null) 
                return;
            
            _liquidMaterial.DOFloat(fillAmount, _fillID, time).SetEase(ease).onComplete = () =>
            {
                liquidRenderer.gameObject.GetOrAddComponent<Wobble>();
            };
            _liquidMaterial.SetColor(_topColorID, content.GetTopColor());
            _liquidMaterial.SetColor(_sideColorID, content.GetSideColor());
        }
    }
}
