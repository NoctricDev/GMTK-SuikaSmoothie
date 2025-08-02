using System.Collections.Generic;
using Fruits;
using Glasses;
using UnityEngine;

namespace CustomerScene.Customers
{
    public static class OrderEvaluator
    {
        public static OrderEvaluation EvaluateOrder(CustomerOrder order, SmoothieContent content)
        {
            OrderEvaluation evaluation = new();

            int pricePaid = 0;
            
            foreach ((FruitSO fruit, int count) in content.FruitsInSmoothie)
            {
                if (IsFruitCorrect(order.Content.FruitsInSmoothie, fruit, out int expectedCount))
                {
                    pricePaid += fruit.FruitValue;
                }
                else
                {
                    pricePaid -= fruit.FruitValue / 2;
                }
            }

            pricePaid = Mathf.Max(0, pricePaid);
            
            evaluation.IsAccepted = pricePaid > 0;
            evaluation.PricePaid = pricePaid;
            
            return evaluation;
        }

        private static bool IsFruitCorrect(Dictionary<FruitSO, int> expectedOrder, FruitSO fruit, out int expectedCount)
        {
            return expectedOrder.TryGetValue(fruit, out expectedCount);
        }
    }
}