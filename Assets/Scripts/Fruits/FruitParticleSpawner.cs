using System;
using FruitBowlScene;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Windows.WebCam;

namespace Fruits
{
    public class FruitParticleSpawner : MonoBehaviour
    {
        [SerializeField, AssetsOnly] private ParticleSystem particlePrefab;
        private IObjectPool<ParticleSystem> _particlePool;

        private void Start()
        {
            FruitMergeManager.Instance.FruitMergedEvent += OnFruitMerged;
            InitPool();
        }

        private void OnDestroy()
        {
            FruitMergeManager.Instance.FruitMergedEvent -= OnFruitMerged;
        }

        private void OnFruitMerged(object sender, FruitMergeManager.FruitMergedEventArgs e)
        {
            ParticleSystem particle = _particlePool.Get();
            particle.transform.position = e.NewSpawnPosition;
            particle.Play();
        }

        private void InitPool()
        {
            _particlePool = new ObjectPool<ParticleSystem>(OnParticleCreate, OnParticleGet, OnParticleRelease, OnParticleDestroy);
        }

        private ParticleSystem OnParticleCreate()
        {
            ParticleSystem instantiatedParticle = Instantiate(particlePrefab, transform);
            instantiatedParticle.gameObject.SetActive(false);
            instantiatedParticle.GetComponent<FruitParticle>().Init(_particlePool);
            return instantiatedParticle;
        }

        private void OnParticleGet(ParticleSystem obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnParticleRelease(ParticleSystem obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnParticleDestroy(ParticleSystem obj)
        {
            Destroy(obj.gameObject);
        }
    }
}
