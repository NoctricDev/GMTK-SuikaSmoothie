using System;
using CustomerScene.Customers;
using DG.Tweening;
using JohaToolkit.UnityEngine.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomerScene
{
    public class CustomerVisuals : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private Sprite[] customerSprites;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Customer customer;
        
        [Title("Settings")]
        [SerializeField] private Vector3 HidePosition;
        [SerializeField] private Vector3 ShowPosition;
        [SerializeField] private float moveDuration = 0.5f;

        private void Start()
        {
            customer.CustomerArrivedEvent += OnCustomerArrived;
            customer.CustomerLeaveEvent += OnCustomerLeft;
            gameObject.SetActive(false);
        }

        private void OnCustomerLeft()
        {
            transform.DOKill();
            transform.DOLocalMove(HidePosition, moveDuration).OnComplete(() => gameObject.SetActive(false));
        }

        private void OnCustomerArrived()
        {
            transform.DOKill();
            transform.localPosition = HidePosition;
            spriteRenderer.sprite = customerSprites.Random();
            gameObject.SetActive(true);
            transform.DOLocalMove(ShowPosition, moveDuration);
        }
    }
}
