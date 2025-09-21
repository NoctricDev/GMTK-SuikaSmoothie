using System;
using JohaToolkit.UnityEngine.Extensions;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FruitDecal : MonoBehaviour
{
    [SerializeField] private DecalProjector decalProjector;
    [SerializeField] private Vector2 lifeTimeRange = new(5, 10);
    [SerializeField] private float fadeDuration = 1;

    private Awaitable _currentAwaitable;
    
    private float _lifeTime;
    
    private void Start()
    {
        decalProjector.fadeFactor = 1;
        _lifeTime = lifeTimeRange.RandomRange();
        _currentAwaitable  = WaitAndFade();
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
