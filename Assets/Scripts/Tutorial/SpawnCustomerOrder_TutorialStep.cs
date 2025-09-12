using System;
using System.Collections.Generic;
using CustomerScene;
using CustomerScene.Customers;
using Fruits;
using Glasses;
using UnityEngine;
using UnityEngine.Rendering;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/TutorialSteps/SpawnCustomerOrder", fileName = "SpawnCustomerOrder_TutorialStep", order = 0)]
    public class SpawnCustomerOrder_TutorialStep : TutorialStep
    {
        [SerializeField] private SerializedDictionary<FruitSO, int> smoothieContent;
        [SerializeField] private CustomerPool_SO[] customerPool;
        public override void StartStep()
        {
            if(!CustomerManager.Instance.TryGetRandomCustomerWithoutOrder(out CustomerManager.CustomerDifficulty customer))
                throw new InvalidOperationException("No customers without orders available to spawn an order for. This is invalid for this tutorial step");

            CustomerOrder order = new CustomerOrder.Builder(new SmoothieContent(smoothieContent))
                .WithCustomerInfo(customerPool)
                .WithCanCancelOrderAllowed(false)
                .Build();
            
            CustomerManager.Instance.TryPlaceOrder(customer, order);
            TutorialManager.Instance.NextStep();
        }

        public override void OnEndStep()
        {
            
        }
    }
}