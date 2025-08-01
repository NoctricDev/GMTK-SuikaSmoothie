using Carry;
using Fruits;
using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FruitBowlScene
{
    public class FruitSpawner : MonoBehaviour
    {
        [Title("References")] 
        [SerializeField] private FruitSO[] fruitSpawnPool;
        [SerializeField] private FruitBowlMouse fruitBowlMouse;
        
        [SerializeField, OnValueChanged(nameof(DropHeightVarChanged))] private FloatVariable dropHeight;
        
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
        }

        private void OnDropHeightChanged(float dropHeightValue) => transform.position = transform.position.SetY(dropHeightValue);

        public void SpawnFruit(FruitSO fruitSO)
        {
            Fruit spawnedFruit = FruitFactory.SpawnFruit(fruitSO, fruitBowlMouse.transform.position, Quaternion.identity, null);
            fruitBowlMouse.StartCarry(spawnedFruit.GetComponent<ICarrieAble>());
        }

        public FruitSO GetRandomFruitFromSpawnPool() => fruitSpawnPool.Random();
    }
}