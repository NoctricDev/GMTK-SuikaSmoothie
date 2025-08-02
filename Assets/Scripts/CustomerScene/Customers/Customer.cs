using System;
using System.Collections.Generic;
using Carry;
using Fruits;
using Glasses;
using JetBrains.Annotations;
using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.Timer;
using UnityEngine;
using UnityEngine.Rendering;

namespace CustomerScene.Customers
{
    public class Customer : MonoBehaviour
    {
        [SerializeField] private Slot slot;

        private bool _hasOrder;
        private CustomerOrder _currentOrder;
        
        public Action<CustomerOrder> OrderPlacedEvent;
        public Action<CustomerOrder> OrderCancelledEvent;
        public Action<OrderEvaluation> OrderCompletedEvent;
        public Action<CustomerOrder> OrderFailedEvent;

        private CountdownTimer _orderTimer;

        [SerializeField] private SerializedDictionary<FruitSO, int> testOrder;

        private void Start()
        {
            slot.SlotContentChangedEvent += OnSlotContentChanged;
        }

        public void Update()
        {
            if(_hasOrder && _orderTimer != null)
                _orderTimer.Tick(Time.deltaTime);
        }

        private void OnSlotContentChanged([CanBeNull] ICarrieAble slotObject)
        {
            if (slot.IsLocked || slotObject == null || !_hasOrder || slotObject is not Glass glass)
                return;

            slot.IsLocked = true;
            ProcessOrder(glass);
        }

        private void ProcessOrder(Glass glass)
        {
            if (glass.Content is not SmoothieContent content)
            {
                OrderFailedEvent?.Invoke(_currentOrder);
                return;
            }

            OrderEvaluation orderEvaluation = OrderEvaluator.EvaluateOrder(_currentOrder, content);
            if (orderEvaluation.IsAccepted)
            {
                OrderCompletedEvent?.Invoke(orderEvaluation);
            }
            else
            {
                OrderFailedEvent?.Invoke(_currentOrder);
            }
        }

        public void SetOrder(CustomerOrder order)
        {
            if (_hasOrder)
                return;
            
            _currentOrder = order;
            _hasOrder = true;
            if(order.TimeToPrepare > 0)
            {
                _orderTimer = new CountdownTimer(order.TimeToPrepare.Seconds());
                _orderTimer.TimerFinished += OnTimerFinished;
            }
            OrderPlacedEvent?.Invoke(_currentOrder);
        }

        private void OnTimerFinished()
        {
            CancelOrder();
        }

        public void CancelOrder()
        {
            if (!_hasOrder)
                return;
            _hasOrder = false;
            OrderCancelledEvent?.Invoke(_currentOrder);
            _orderTimer = null;
        }
    }
}
