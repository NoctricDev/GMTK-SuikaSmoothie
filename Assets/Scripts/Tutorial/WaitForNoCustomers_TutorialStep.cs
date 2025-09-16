using System.Linq;
using CustomerScene.Customers;
using JohaToolkit.UnityEngine.ScriptableObjects.Events;
using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/TutorialSteps/WaitForNoCustomers", fileName = "WaitForNoCustomers", order = 0)]
    public class WaitForNoCustomers_TutorialStep : TutorialStep
    {
        [SerializeField] private GameEvent ordercompletedEvent;
        public override void StartStep()
        {
            ordercompletedEvent.Subscribe(OnOrderCompleted);
        }

        public override void OnEndStep()
        {
            ordercompletedEvent.Unsubscribe(OnOrderCompleted);
        }

        private void OnOrderCompleted(object sender)
        {
            if(((CustomerManager)sender).CustomersWithOrders.Count() == 1)
                TutorialManager.Instance.NextStep();
        }
    }
}