using UnityEngine;
using UnityEngine.Pool;

namespace Fruits
{
    public class FruitParticle : MonoBehaviour
    {
        private IObjectPool<ParticleSystem> _particlePool;
        private ParticleSystem _particle;

        public void Init(IObjectPool<ParticleSystem> particlePool)
        {
            _particlePool = particlePool;
            _particle = GetComponent<ParticleSystem>();
        }
        
        private void OnParticleSystemStopped()
        {
            _particlePool.Release(_particle);
        }
    }
}