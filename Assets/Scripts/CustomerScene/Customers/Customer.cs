using System;
using Carry;
using Glasses;
using JetBrains.Annotations;
using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.Timer;
using UnityEngine;

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

        private CountdownTimer _orderTimer;

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
            if (slotObject == null || !_hasOrder || slotObject is not Glass glass)
                return;
            
            // Evaluate Order
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
