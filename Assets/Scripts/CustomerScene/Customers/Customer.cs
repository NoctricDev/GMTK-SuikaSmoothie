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

        public bool HasOrder { get; private set; }
        private CustomerOrder _currentOrder;
        
        public Action<CustomerOrder> OrderPlacedEvent;
        public Action<CustomerOrder> OrderCancelledEvent;
        public Action<OrderEvaluation> OrderCompletedEvent;
        public Action<CustomerOrder> OrderFailedEvent;
        public Action<float> OrderTimeUpdatedEvent;

        private CountdownTimer _orderTimer;

        public event Action CustomerArrivedEvent;
        public event Action CustomerLeaveEvent;
        
        private void Start()
        {
            slot.SlotContentChangedEvent += OnSlotContentChanged;
        }

        public void Update()
        {
            if (!HasOrder || _orderTimer == null) 
                return;
            _orderTimer.Tick(Time.deltaTime);
            
            OrderTimeUpdatedEvent?.Invoke(_orderTimer?.Progress ?? 0);
        }

        private void OnSlotContentChanged([CanBeNull] ICarrieAble slotObject)
        {
            if (slot.IsLocked || slotObject == null || !HasOrder || slotObject is not Glass glass)
                return;

            slot.IsLocked = true;
            ProcessOrder(glass);

        }

        private void ProcessOrder(Glass glass)
        {
            if (glass.Content is not SmoothieContent content)
            {
                ResetCustomer();
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
            
            ResetCustomer();
        }

        public void SetOrder(CustomerOrder order)
        {
            if (HasOrder)
                return;
            CustomerArrivedEvent?.Invoke();
            _currentOrder = order;
            HasOrder = true;
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
            if (!HasOrder)
                return;
            ResetCustomer();
            OrderCancelledEvent?.Invoke(_currentOrder);
        }

        private void ResetCustomer()
        {
            if(HasOrder)
                CustomerLeaveEvent?.Invoke();
            HasOrder = false;
            _orderTimer = null;
            slot.IsLocked = false;
            if(slot.HasPayload)
                Destroy(slot.RemoveSlot().GetAttachedGameObject());
        }
    }
}
