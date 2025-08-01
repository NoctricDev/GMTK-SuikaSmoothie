using Carry;
using Fruits;
using JetBrains.Annotations;
using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FruitBowlScene
{
    public class FruitSpawner : MonoBehaviour, ICarrieAble
    {
        [Title("References")] 
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private FruitSO[] fruitSpawnPool;

        [SerializeField, OnValueChanged(nameof(DropHeightVarChanged))] private FloatVariable dropHeight;

        private Fruit _spawnedFruit;
        
        private void DropHeightVarChanged(FloatVariable newHeight)
        {
            if (newHeight == null)
                return;
            OnDropHeightChanged(newHeight.Value);
        }
        
        private void Start()
        {
            OnDropHeightChanged(dropHeight.Value);
            dropHeight.OnValueChanged += OnDropHeightChanged;
            SpawnFruit();
        }

        private void OnDropHeightChanged(float dropHeightValue) => transform.position = transform.position.SetY(dropHeightValue);

        private void OnTriggerExit(Collider other)
        {
            if(!other.transform.TryGetComponent(out Fruit fruit) || fruit != _spawnedFruit)
                return;
            
            SpawnFruit();
        }

        private void SpawnFruit()
        {
            _spawnedFruit = FruitFactory.SpawnFruit(fruitSpawnPool.Random(), spawnPoint.position, Quaternion.identity, null);
        }

        public bool TryStartCarry(Transform carryTransform, out ICarrieAble carrieAble)
        {
            carrieAble = null;
            if (_spawnedFruit == null)
                return false;

            bool result = _spawnedFruit.GetComponent<ICarrieAble>().TryStartCarry(carryTransform, out ICarrieAble activeCarrieAble);
            carrieAble = result ? activeCarrieAble : null;
            return result;
        }

        public void StopCarry()
        {
            Debug.LogError("This should not happen! FruitSpawner should not be stopped directly.");
        }
    }
}