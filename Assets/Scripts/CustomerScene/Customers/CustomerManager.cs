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
using JohaToolkit.UnityEngine.ScriptableObjects.Events;
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
        public struct CustomerDifficulty
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
        [SerializeField] private GameEvent orderCompletedEvent;

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
        
        public IEnumerable<Customer> CustomersWithOrders => customers.Select(c => c.customer).Where(c => c.HasOrder);
        
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
            _fruitPicker ??= new WeightedPicker<FruitSO>(); 
            _fruitPicker.Clear();
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
            orderCompletedEvent?.RaiseEvent(this);
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

        public void TryPlaceOrder(CustomerDifficulty customer, CustomerOrder order)
        {
            customer.customer.SetOrder(order);
        }

        public void TryPlaceOrder()
        {
            if (!TryGetRandomCustomerWithoutOrder(out CustomerDifficulty customer))
                return;
            TryPlaceOrder(customer, GenerateCustomerOrder(customer.minDifficulty, customer.maxDifficulty));
        }

        public bool TryGetRandomCustomerWithoutOrder(out CustomerDifficulty customer)
        {
            _customersList.FisherYatesShuffle();
            customer = _customersList.FirstOrDefault(c => !c.customer.HasOrder);
            return customer.customer != null;
        }

        private float GetHighestDifficulty() => availableFruits.Select(f => f.DifficultyRating).Max();
        private void AddToPicker(FruitSO fruit, float highestDifficulty) => _fruitPicker.Add(fruit, (highestDifficulty - fruit.DifficultyRating + 1) / (highestDifficulty + 1));

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

            ResetPicker();

            int desiredFruitCount = GetDesiredFruitCount();
            
            PickFruit(fruitsInSmoothie, minDifficulty, localMaxDifficulty, desiredFruitCount);
            
            return new SmoothieContent(fruitsInSmoothie);
        }

        private int GetDesiredFruitCount()
        {
            int fruitCount = 1;
            for (int i = 1; i < maxFruitsInSmoothie; i++)
            {
                if (Random.value <= chanceForNextFruit)
                    fruitCount++;
                else
                    break;
            }

            return fruitCount;
        }
        
        private float GetCurrentDifficulty(Dictionary<FruitSO, int> fruitsInSmoothie) => fruitsInSmoothie.Keys.Sum(f => f.DifficultyRating);

        private void PickFruit(Dictionary<FruitSO, int> fruitsInSmoothie, float minDifficulty, float maxDifficulty, int desiredFruitCount)
        {
            for (int i = 0; i < desiredFruitCount; i++)
            {
                SimplePick(fruitsInSmoothie);
            }
            
            while(IsSmoothieTooEasy(fruitsInSmoothie, minDifficulty) || IsSmoothieTooHard(fruitsInSmoothie, maxDifficulty))
            {
                if (_fruitPicker.Count == 0)
                {
                    Debug.LogError("No more fruits to pick from, cannot adjust smoothie difficulty further");
                    break;
                }
                if (IsSmoothieTooEasy(fruitsInSmoothie, minDifficulty))
                    MakeHarder(fruitsInSmoothie);
                else
                    MakeEasier(fruitsInSmoothie);
            }
        }
        
        private bool IsSmoothieTooEasy(Dictionary<FruitSO, int> fruitsInSmoothie, float minDifficulty) => GetCurrentDifficulty(fruitsInSmoothie) < minDifficulty;
        private bool IsSmoothieTooHard(Dictionary<FruitSO, int> fruitsInSmoothie, float maxDifficulty) => GetCurrentDifficulty(fruitsInSmoothie) > maxDifficulty;

        private void MakeEasier(Dictionary<FruitSO, int> fruitsInSmoothie)
        {
            FruitSO hardestFruit = fruitsInSmoothie.Keys.OrderByDescending(f => f.DifficultyRating).First();
            fruitsInSmoothie.Remove(hardestFruit);
            SimplePick(fruitsInSmoothie);
        }

        private void MakeHarder(Dictionary<FruitSO, int> fruitsInSmoothie)
        {
            FruitSO easiestFruit = fruitsInSmoothie.Keys.OrderBy(f => f.DifficultyRating).First();
            fruitsInSmoothie.Remove(easiestFruit);
            SimplePick(fruitsInSmoothie);
        }

        private void SimplePick(Dictionary<FruitSO, int> fruitsInSmoothie)
        {
            FruitSO pick = _fruitPicker.Pick();
            _fruitPicker.Remove(pick);
            fruitsInSmoothie.Add(pick, 1);
        }
    }
}