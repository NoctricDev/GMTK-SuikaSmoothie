using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Fruits;
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

        Material _liquidMaterial;
        private int _fillID = Shader.PropertyToID("_Fill");
        private int _topColorID = Shader.PropertyToID("_TopColor");
        private int _sideColorID = Shader.PropertyToID("_SideColor");
        
        private void Awake()
        {
            _liquidMaterial = new Material(liquidShader);
            liquidRenderer.SetMaterials(new List<Material> { _liquidMaterial });
            SetFillContent(0, 0, null);
            glass.GlassContentSetEvent += (content) => SetFillContent(0.2f, 1, content as SmoothieContent);
            glass.GlassContentClearEvent += (_) => SetFillContent(0.2f, 0, null);
        }

        private void SetFillContent(float time, float fillAmount, SmoothieContent content)
        {
            _liquidMaterial.DOFloat(fillAmount, _fillID, time);
            if (content == null) 
                return;
            FruitSO firstFruit = content.FruitsInSmoothie?.Keys?.First() ?? null;
            if (firstFruit == null)
                return;
            
            _liquidMaterial.SetColor(_topColorID, firstFruit.SmoothieTopColor);
            _liquidMaterial.SetColor(_topColorID, firstFruit.SmoothieSideColor);
        }
    }
}
