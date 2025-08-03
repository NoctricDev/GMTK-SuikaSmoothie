using System;
using Carry;
using Fruits;
using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using JohaToolkit.UnityEngine.Timer;
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

        [Title("Settings")] 
        [SerializeField] private float coolDownTime = .2f;

        private bool _canSpawn = true;
        private float _coolDownTimer;
        
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

        private void Update()
        {
            if (_canSpawn)
                return;
            _coolDownTimer += Time.deltaTime;
            if (_coolDownTimer >= coolDownTime)
                _canSpawn = true;
        }

        private void OnDropHeightChanged(float dropHeightValue) => transform.position = transform.position.SetY(dropHeightValue);

        public bool SpawnFruit(FruitSO fruitSO)
        {
            if (fruitBowlMouse.HasPayload || !_canSpawn)
                return false;
            Fruit spawnedFruit = FruitFactory.SpawnFruit(fruitSO, fruitBowlMouse.transform.position, Quaternion.identity, null, true);
            fruitBowlMouse.StartCarry(spawnedFruit.GetComponent<ICarrieAble>());
            _canSpawn = false;
            _coolDownTimer = 0;
            return true;
        }

        public FruitSO GetRandomFruitFromSpawnPool() => fruitSpawnPool.Random();
    }
}