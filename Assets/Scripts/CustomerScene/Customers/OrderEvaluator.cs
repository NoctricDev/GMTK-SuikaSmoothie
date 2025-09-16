using System.Collections.Generic;
using System.Linq;
using Fruits;
using Glasses;
using UnityEngine;

namespace CustomerScene.Customers
{
    public static class OrderEvaluator
    {
        public static float OneFruitMultiplier = 1f;
        public static float TwoFruitsMultiplier = 1.5f;
        public static float ThreeFruitsMultiplier = 2f;
        public static OrderEvaluation EvaluateOrder(CustomerOrder order, SmoothieContent content)
        {
            OrderEvaluation evaluation = new();

            int pricePaid = 0;
            bool atLeastOneFruitCorrect = false;
            
            foreach ((FruitSO fruit, int count) in content.FruitsInSmoothie)
            {
                if (IsFruitCorrect(order.Content.FruitsInSmoothie, fruit, out int expectedCount))
                {
                    atLeastOneFruitCorrect = true;
                    pricePaid += fruit.FruitValue;
                }
                else
                {
                    pricePaid -= fruit.FruitValue / 2;
                }
            }

            pricePaid = Mathf.Max(1, pricePaid);
            
            evaluation.IsAccepted = pricePaid > 0 && atLeastOneFruitCorrect;
            evaluation.PricePaid = Mathf.CeilToInt(pricePaid * GetMultiplier(order));
            return evaluation;
        }

        private static float GetMultiplier(CustomerOrder order)
        {
            return order.Content.FruitsInSmoothie.Keys.Count() switch
            {
                1 => OneFruitMultiplier,
                2 => TwoFruitsMultiplier,
                3 => ThreeFruitsMultiplier,
                _ => 1f
            };
        }

        private static bool IsFruitCorrect(IReadOnlyDictionary<FruitSO, int> expectedOrder, FruitSO fruit, out int expectedCount)
        {
            return expectedOrder.TryGetValue(fruit, out expectedCount);
        }
    }
}