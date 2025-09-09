using System;
using System.Collections.Generic;
using System.Linq;
using CSharpTools.Randomization;
using Events;
using Fruits;
using Glasses;
using JohaToolkit.UnityEngine.Audio;
using JohaToolkit.UnityEngine.DataStructures;
using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using Scenes;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CustomerScene.Customers
{
    public class CustomerManager : MonoBehaviourSingleton<CustomerManager>, IGameplaySceneObject
    {
        [Serializable]
        private struct CustomerDifficulty
        {
            public Customer customer;
            public float minDifficulty;
            public float maxDifficulty;
        }
        [Title("References")]
        [SerializeField] private FruitSO[] availableFruits;
        [SerializeField] private CustomerDifficulty[] customers;
        private List<CustomerDifficulty> _customersList;
        [SerializeField] private IntVariable playerMoney;
        [SerializeField] private SoundDataAsset orderSuccessSound;
        [SerializeField] private StartGameTypeVariable startGameTypeVariable;
        [SerializeField] private CustomerPool_SO[] customerPools;

        [Title("Settings")] 
        [SerializeField, InfoBox("Smoothie difficulty * this + timeToPrepareBase = TimeToPrepare")] private float timeToPrepareMultiplier = 2f;

        [SerializeField] private float timeToPrepareBase = 10f;
        [SerializeField] private int maxFruitsInSmoothie = 3;
        [SerializeField, InfoBox("This settings checks every Random(X,Y) seconds, if a order can be placed")] private Vector2 customerOrderSpawnRate = new(1f, 3f);
        [SerializeField, Range(0, 1)] private float chanceForNextFruit;

        [Title("Order Evaluation")]
        [SerializeField] private float oneFruitMultiplier = 1f;
        [SerializeField] private float twoFruitMultiplier = 1.2f;
        [SerializeField] private float threeFruitMultiplier = 2f;
        
        private WeightedPicker<FruitSO> _fruitPicker;
        private float _timer;
        private float _nextOrderCheck;
        private bool _hasInitialized;

        private bool _hasStarted = false;
        
        protected override void Awake()
        {
            base.Awake();
            
            OrderEvaluator.OneFruitMultiplier = oneFruitMultiplier;
            OrderEvaluator.TwoFruitsMultiplier = twoFruitMultiplier;
            OrderEvaluator.ThreeFruitsMultiplier = threeFruitMultiplier;

            _nextOrderCheck = customerOrderSpawnRate.RandomRange();
            _customersList = customers.ToList();
            
            foreach (CustomerDifficulty customer in _customersList)
            {
                customer.customer.OrderCompletedEvent += OnOrderCompleted;
            }

            startGameTypeVariable.OnValueChanged += (gameMode) =>
            {
                if(gameMode is not (StartGameType.Dummy or StartGameType.Tutorial))
                    _hasStarted = true;
                playerMoney.Value = 0;
            };
        }

        private void ResetPicker()
        {
            _fruitPicker = new WeightedPicker<FruitSO>();
            float highestDifficulty = GetHighestDifficulty();
            foreach (FruitSO fruit in availableFruits)
            {
                AddToPicker(fruit, highestDifficulty);
            }
        }

        public void LoadEnd()
        {
            if (_hasInitialized || !_hasStarted)
                return;
            _hasInitialized = true;
            TryPlaceOrder();
        }

        private void OnOrderCompleted(OrderEvaluation orderEvaluation)
        {
            if (orderEvaluation.IsAccepted)
            {
                SoundManager.Instance.Play(orderSuccessSound);
                playerMoney.Value += orderEvaluation.PricePaid;
            }
        }

        private void Update()
        {
            if (!_hasStarted)
                return;
            
            _timer += Time.deltaTime;
            if (_timer >= _nextOrderCheck)
            {
                _timer = 0f;
                _nextOrderCheck = customerOrderSpawnRate.RandomRange();
                TryPlaceOrder();
            }
        }

        public void TryPlaceOrder()
        {
            _customersList.FisherYatesShuffle();
            foreach (CustomerDifficulty customer in _customersList)
            {
                if (customer.customer.HasOrder)
                    continue;
                
                customer.customer.SetOrder(GenerateCustomerOrder(customer.minDifficulty, customer.maxDifficulty));
                break;
            }
        }

        private float GetHighestDifficulty() => availableFruits.Select(f => f.DifficultyRating).Max();
        private void AddToPicker(FruitSO fruit, float highestDifficulty) => _fruitPicker.Add(fruit, highestDifficulty - fruit.DifficultyRating / highestDifficulty);

        public CustomerOrder GenerateCustomerOrder(float minDifficulty, float localMaxDifficulty)
        {
            SmoothieContent smoothieContent = GenerateRandomSmoothieContent(minDifficulty, localMaxDifficulty);
            
            return new CustomerOrder.Builder(smoothieContent)
                .WithTimeToPrepare(smoothieContent.FruitsInSmoothie.Keys.Sum(f => f.DifficultyRating) * timeToPrepareMultiplier + timeToPrepareBase)
                .WithCustomerInfo(customerPools)
                .Build();
        }

        private SmoothieContent GenerateRandomSmoothieContent(float minDifficulty, float localMaxDifficulty)
        {
            Dictionary<FruitSO, int> fruitsInSmoothie = new();
            SmoothieContent content = new(fruitsInSmoothie);

            ResetPicker();
            
            for (int i = 0; i < maxFruitsInSmoothie; i++)
            {
                float currentDifficulty = fruitsInSmoothie.Keys.Select(f => f.DifficultyRating).Sum();
                if (currentDifficulty >= localMaxDifficulty)
                    break;

                FruitSO pick;
                do
                {
                    pick = _fruitPicker.Pick();
                    _fruitPicker.Remove(pick);
                } while (pick.DifficultyRating + currentDifficulty > localMaxDifficulty || _fruitPicker.Count == 0);

                fruitsInSmoothie.Add(pick, 1);
                
                if(pick.DifficultyRating + currentDifficulty < minDifficulty)
                    continue;
                
                if (Random.Range(0f, 1f) > chanceForNextFruit)
                    break;
            }

            while (fruitsInSmoothie.Keys.Select(f => f.DifficultyRating).Sum() < minDifficulty && _fruitPicker.Count > 0)
            {
                FruitSO lowestFruit = fruitsInSmoothie.Keys.OrderBy(f => f.DifficultyRating).First();
                fruitsInSmoothie.Remove(lowestFruit);
                FruitSO pick = _fruitPicker.Pick();
                _fruitPicker.Remove(pick);
                fruitsInSmoothie.Add(pick, 1);
            }

            content.SetContent(fruitsInSmoothie);
            return content;
        }
    }
}