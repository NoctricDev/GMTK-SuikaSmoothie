using System.Collections.Generic;
using System.Linq;
using CSharpTools.Randomization;
using Fruits;
using Glasses;
using JohaToolkit.UnityEngine.DataStructures;
using UnityEngine;

namespace CustomerScene.Customers
{
    public class CustomerManager : MonoBehaviourSingleton<CustomerManager>
    {
        [SerializeField] private FruitSO[] availableFruits;

        [SerializeField] private Customer[] customers;

        private WeightedPicker<FruitSO> _fruitPicker;

        protected override void Awake()
        {
            base.Awake();
            _fruitPicker = new WeightedPicker<FruitSO>();
            float maxDifficulty = availableFruits.Select(f => f.DifficultyRating).Max();
            foreach (FruitSO fruit in availableFruits)
            {
                
            }
        }

        public CustomerOrder GenerateCustomerOrder()
        {
            FruitSO randomFruit = availableFruits[Random.Range(0, availableFruits.Length)];
            SmoothieContent smoothieContent = new SmoothieContent(new Dictionary<FruitSO, int> { { randomFruit, 1 } });
            float timeToPrepare = randomFruit.DifficultyRating * 2f; // Example calculation
            float price = randomFruit.DifficultyRating * 5f; // Example calculation

            return new CustomerOrder.Builder(smoothieContent)
                .WithTimeToPrepare(timeToPrepare)
                .WithPrice(price)
                .Build();
        }
    }
}