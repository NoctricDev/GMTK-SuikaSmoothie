using System.Linq;
using CustomerScene.Customers;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private Customer connectedCustomer;
    [SerializeField] private Transform orderContainer;
    [SerializeField] private Transform orderImageContainer;
    [SerializeField] private Image imagePrefab;
    [SerializeField] private Image orderTimerImage;

    private void Start()
    {
        connectedCustomer.OrderPlacedEvent += OnNewOrderPlaced;
        connectedCustomer.OrderCancelledEvent += (_) => HideGameObject();
        connectedCustomer.OrderCompletedEvent += (_) => HideGameObject();
        connectedCustomer.OrderFailedEvent += (_) => HideGameObject();
        connectedCustomer.OrderTimeUpdatedEvent += OrderTimerUpdated;
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
        orderContainer.gameObject.SetActive(false);
    }

    private void OnNewOrderPlaced(CustomerOrder order)
    {
        orderContainer.gameObject.SetActive(true);

        SetImages(order);
    }

    private void SetImages(CustomerOrder order)
    {
        int childCount = orderImageContainer.childCount;
        int requiredCount = order.Content.FruitsInSmoothie.Keys.Count;
        while (requiredCount < childCount)
        {
            Destroy(orderImageContainer.GetChild(childCount-1).gameObject);
            childCount--;
        }
        
        for(int i = 0; i < requiredCount; i++)
        {
            Image image = childCount > i ? orderImageContainer.GetChild(i).GetComponent<Image>() : Instantiate(imagePrefab, orderImageContainer);
            image.sprite = order.Content.FruitsInSmoothie.Keys.ToList()[i].FruitIcon;
        }
    }
}
