using System.Collections.Generic;
using System.Linq;
using CSharpTools.Randomization;
using Fruits;
using Glasses;
using JohaToolkit.UnityEngine.DataStructures;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace CustomerScene.Customers
{
    public class CustomerManager : MonoBehaviourSingleton<CustomerManager>
    {
        [Title("References")]
        [SerializeField] private FruitSO[] availableFruits;
        [SerializeField] private Customer[] customers;

        [Title("Settings")] 
        [SerializeField, InfoBox("Smoothie difficulty * this = TimeToPrepare")] private float timeToPrepareMultiplier = 2f;
        [SerializeField] private int maxFruitsInSmoothie = 3;
        [SerializeField] private int maxDifficulty;

        private WeightedPicker<FruitSO> _fruitPicker;

        protected override void Awake()
        {
            base.Awake();
            _fruitPicker = new WeightedPicker<FruitSO>();
            float highestDifficulty = GetHighestDifficulty();
            foreach (FruitSO fruit in availableFruits)
            {
                AddToPicker(fruit, highestDifficulty);
            }
        }

        private float GetHighestDifficulty() => availableFruits.Select(f => f.DifficultyRating).Max();
        private void AddToPicker(FruitSO fruit, float highestDifficulty) => _fruitPicker.Add(fruit, fruit.DifficultyRating / highestDifficulty);

        public CustomerOrder GenerateCustomerOrder()
        {
            SmoothieContent smoothieContent = GenerateRandomSmoothieContent();
            
            return new CustomerOrder.Builder(smoothieContent)
                .WithTimeToPrepare(smoothieContent.FruitsInSmoothie.Keys.Sum(f => f.DifficultyRating) * timeToPrepareMultiplier)
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
                if (currentDifficulty <= maxDifficulty)
                    break;

                FruitSO pick;
                do
                {
                    pick = _fruitPicker.Pick();
                    _fruitPicker.Remove(pick);
                    pickedFruits.Add(pick);
                } while (pick.DifficultyRating + currentDifficulty > maxDifficulty || _fruitPicker.Count == 0);

                fruitsInSmoothie.Add(pick, 1);
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