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

            bool isMissingFruit = false;
            foreach ((FruitSO expectedFruit, int expectedCount) in order.Content.FruitsInSmoothie)
            {
                if (IsFruitCorrect(content.FruitsInSmoothie, expectedFruit, expectedCount, out int countDifference)) 
                    continue;
                isMissingFruit = true;
                break;
            }
            
            if (isMissingFruit)
            {
                evaluation.IsAccepted = false;
                evaluation.PricePaid = 0;
            }
            else
            {
                evaluation.IsAccepted = true;
                evaluation.PricePaid = order.Price * (content.TotalFruitsCount - ((float)GetWrongFruitsCount(content.FruitsInSmoothie, order) / content.TotalFruitsCount));
            }
            return evaluation;
        }

        private static bool IsFruitCorrect(Dictionary<FruitSO, int> content, FruitSO expectedFruit, int expectedCount, out int difference)
        {
            if (content.TryGetValue(expectedFruit, out int countInContent))
            {
                difference = Mathf.Abs(countInContent - expectedCount);
                return countInContent >= expectedCount;
            }
            difference = Mathf.Abs(0 - expectedCount);
            return false;
        }
        
        private static int GetWrongFruitsCount(Dictionary<FruitSO, int> content, CustomerOrder order)
        {
            int incorrectCount = 0;
            foreach ((FruitSO fruit, int count) in content)
            {
                if (order.Content.FruitsInSmoothie.ContainsKey(fruit))
                    continue;
                incorrectCount += count;
            }
            return incorrectCount;
        }
    }
}