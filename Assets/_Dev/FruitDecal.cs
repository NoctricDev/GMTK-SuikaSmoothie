using System;
using Fruits;
using JohaToolkit.UnityEngine.Audio;
using JohaToolkit.UnityEngine.Extensions;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FruitDecal : MonoBehaviour
{
    private static readonly int mainColor = Shader.PropertyToID(("_MainColor"));
    [SerializeField] private DecalProjector decalProjector;
    [SerializeField] private Vector2 lifeTimeRange = new(5, 10);
    [SerializeField] private float fadeDuration = 1;
    [SerializeField] private Material baseMaterial;
    [SerializeField] private SoundDataAsset squishSound;
    
    private Awaitable _currentAwaitable;
    
    private float _lifeTime;
    private FruitSO _fruitSO;
    
    private void Start()
    {
        decalProjector.fadeFactor = 1;
        _lifeTime = lifeTimeRange.RandomRange();
        _currentAwaitable  = WaitAndFade();
        SoundManager.Instance.Play(squishSound);
    }
    
    
    public void Init(FruitSO fruitSO)
    {
        _fruitSO = fruitSO;
        Material material = new(baseMaterial);
        material.SetColor(mainColor, fruitSO.SmoothieTopColor);
        decalProjector.material = material;
    }

    private void OnDestroy()
    {
        _currentAwaitable?.Cancel();
    }

    private async Awaitable WaitAndFade()
    {
        float timer = 0;
        while (timer < _lifeTime)
        {
            timer += Time.deltaTime;
            await Awaitable.NextFrameAsync();
        }

        timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            decalProjector.fadeFactor = timer.IntervalRemap(0, fadeDuration, 1, 0);
            await Awaitable.NextFrameAsync();
        }
        
        Destroy(gameObject);
    }
}
