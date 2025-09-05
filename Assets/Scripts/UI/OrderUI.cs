using System;
using System.Collections.Generic;
using System.Linq;
using CustomerScene.Customers;
using DG.Tweening;
using Fruits;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OrderUI : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private Customer connectedCustomer;
        [SerializeField] private Transform orderContainer;
    
        [SerializeField] private Image orderTimerImage;
        [SerializeField] private Image[] fruitImages;

        [Title("Settings")]
        [SerializeField] private bool slideInOut;
        [SerializeField, ShowIfGroup(nameof(slideInOut))] private float slideDuration = 0.2f;
        [SerializeField, ShowIfGroup(nameof(slideInOut))] private Vector2 slideInPosition;
        [SerializeField, ShowIfGroup(nameof(slideInOut))] private Vector2 slideOutPosition;

        private void Awake()
        {
            connectedCustomer.OrderPlacedEvent += OnNewOrderPlaced;
            connectedCustomer.OrderCancelledEvent += (_) => HideGameObject();
            connectedCustomer.OrderCompletedEvent += (_) => HideGameObject();
            connectedCustomer.OrderFailedEvent += (_) => HideGameObject();
            connectedCustomer.OrderTimeUpdatedEvent += OrderTimerUpdated;
            orderContainer.position = slideInOut? slideOutPosition : orderContainer.position;
            orderContainer.gameObject.SetActive(false);
        }

        [Button]
        private void CancelOrder()
        {
            connectedCustomer.CancelOrder();
        }
    
        private void OrderTimerUpdated(float remaining)
        {
            orderTimerImage.fillAmount = remaining;
        }

        private void HideGameObject()
        {
            if(!slideInOut)
            {
                orderContainer.gameObject.SetActive(false);
                return;
            }
            orderContainer.DOKill();
            orderContainer.DOLocalMove(slideOutPosition, slideDuration).OnComplete(() => orderContainer.gameObject.SetActive(false));
        }

        private void OnNewOrderPlaced(CustomerOrder order)
        {
            orderContainer.gameObject.SetActive(true);
            SetImages(order);
            if (!slideInOut)
                return;
            orderContainer.localPosition = slideOutPosition;
            orderContainer.DOKill();
            orderContainer.DOLocalMove(slideInPosition, slideDuration);
        }

        private void SetImages(CustomerOrder order)
        {
            List<FruitSO> fruitsInOrder = order.Content.FruitsInSmoothie.Keys.ToList();
            int requiredCount = fruitsInOrder.Count;

            if(fruitImages.Length < requiredCount)
            {
                Debug.LogError("Order UI has less images than required");
                return;
            }
        
            for (int i = 0; i < fruitImages.Length; i++)
            {
                Image image = fruitImages[i];
                if (requiredCount <= i)
                {
                    image.gameObject.SetActive(false);
                    continue;
                }
                image.sprite = fruitsInOrder[i].FruitIcon;
                image.gameObject.SetActive(true);
            }
        }
    }
}
