using System.Collections.Generic;
using System.Linq;
using CSharpTools.Randomization;
using Fruits;
using Glasses;
using JohaToolkit.UnityEngine.DataStructures;
using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomerScene.Customers
{
    public class CustomerManager : MonoBehaviourSingleton<CustomerManager>
    {
        [Title("References")]
        [SerializeField] private FruitSO[] availableFruits;
        [SerializeField] private Customer[] customers;
        private List<Customer> _customersList;
        [SerializeField] private IntVariable playerMoney;

        [Title("Settings")] 
        [SerializeField, InfoBox("Smoothie difficulty * this = TimeToPrepare")] private float timeToPrepareMultiplier = 2f;
        [SerializeField] private int maxFruitsInSmoothie = 3;
        [SerializeField] private int maxDifficulty;
        [SerializeField, InfoBox("This settings checks every Random(X,Y) seconds, if a order can be placed")] private Vector2 customerOrderSpawnRate = new(1f, 3f);
        [SerializeField, Range(0, 1)] private float chanceForNextFruit;

        private WeightedPicker<FruitSO> _fruitPicker;
        private float _timer;
        private float _nextOrderCheck;

        protected override void Awake()
        {
            base.Awake();
            _fruitPicker = new WeightedPicker<FruitSO>();
            float highestDifficulty = GetHighestDifficulty();
            foreach (FruitSO fruit in availableFruits)
            {
                AddToPicker(fruit, highestDifficulty);
            }

            _nextOrderCheck = customerOrderSpawnRate.RandomRange();
            _customersList = customers.ToList();
            
            foreach (Customer customer in _customersList)
            {
                customer.OrderCompletedEvent += OnOrderCompleted;
            }
        }

        private void OnOrderCompleted(OrderEvaluation orderEvaluation)
        {
            if (orderEvaluation.IsAccepted)
            {
                playerMoney.Value += orderEvaluation.PricePaid;
            }
        }

        private void Update()
        {
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
            foreach (Customer customer in _customersList)
            {
                if (customer.HasOrder)
                    continue;
                
                customer.SetOrder(GenerateCustomerOrder());
                break;
            }
        }

        private float GetHighestDifficulty() => availableFruits.Select(f => f.DifficultyRating).Max();
        private void AddToPicker(FruitSO fruit, float highestDifficulty) => _fruitPicker.Add(fruit, fruit.DifficultyRating / highestDifficulty);

        public CustomerOrder GenerateCustomerOrder()
        {
            SmoothieContent smoothieContent = GenerateRandomSmoothieContent();
            
            return new CustomerOrder.Builder(smoothieContent)
                .WithTimeToPrepare(smoothieContent.FruitsInSmoothie.Keys.Sum(f => f.DifficultyRating) * timeToPrepareMultiplier + 10)
                .Build();
        }

        private SmoothieContent GenerateRandomSmoothieContent()
        {
            Dictionary<FruitSO, int> fruitsInSmoothie = new();
            SmoothieContent content = new(fruitsInSmoothie);

            List<FruitSO> pickedFruits = new();
            
            for (int i = 0; i < maxFruitsInSmoothie; i++)
            {
                float currentDifficulty = fruitsInSmoothie.Keys.Select(f => f.DifficultyRating).Sum();
                if (currentDifficulty >= maxDifficulty)
                    break;

                FruitSO pick;
                do
                {
                    pick = _fruitPicker.Pick();
                    _fruitPicker.Remove(pick);
                    pickedFruits.Add(pick);
                } while (pick.DifficultyRating + currentDifficulty > maxDifficulty || _fruitPicker.Count == 0);

                fruitsInSmoothie.Add(pick, 1);
                if (Random.Range(0f, 1f) > chanceForNextFruit)
                    break;
            }
            
            float highestDifficulty = GetHighestDifficulty();
            foreach (FruitSO pickedFruit in pickedFruits)
            {
                AddToPicker(pickedFruit, highestDifficulty);
            }
            
            content.SetContent(fruitsInSmoothie);
            return content;
        }
    }
}