using Glasses;
using JohaToolkit.UnityEngine.Extensions;
using UnityEngine;

namespace CustomerScene.Customers
{
    public class CustomerOrder
    {
        public CustomerInfo CustomerInfo { get; private set; }
        public SmoothieContent Content { get; private set; }
        public float TimeToPrepare { get; private set; }
        public bool CanCancelOrderAllowed;
        private CustomerOrder(SmoothieContent content, float timeToPrepare, CustomerInfo customerInfo, bool canCancelOrderAllowed = true)
        {
            CustomerInfo = customerInfo;
            Content = content;
            TimeToPrepare = timeToPrepare;
            CanCancelOrderAllowed = canCancelOrderAllowed;
        }

        public class Builder
        {
            private SmoothieContent _content;
            private float _timeToPrepare = 0;
            private bool _canCancelOrderAllowed;
            private CustomerInfo _customerInfo;
            public Builder(SmoothieContent content)
            {
                _content = content;
                _canCancelOrderAllowed = true;
            }

            public Builder WithTimeToPrepare(float time)
            {
                _timeToPrepare = time;
                return this;
            }

            public Builder WithCustomerInfo(CustomerPool_SO[] customerPools)
            {
                _customerInfo = SetRandomCustomerInfo(customerPools);
                return this;
            }
            
            public Builder WithCanCancelOrderAllowed(bool canCancelOrderAllowed)
            {
                _canCancelOrderAllowed = canCancelOrderAllowed;
                return this;
            }
            
            public CustomerOrder Build() => new(_content, _timeToPrepare, _customerInfo, _canCancelOrderAllowed);

            private CustomerInfo SetRandomCustomerInfo(CustomerPool_SO[] customerPools)
            {
                CustomerPool_SO pool = customerPools.Random();
                string randomName = pool.customerNamePool.Random().Names.Random();
                CustomerSprites sprite = pool.customerSpritePool.Random();
                return new CustomerInfo
                {
                    Name = randomName,
                    CustomerSprite = sprite.CustomerSprite,
                    CustomerIcon = sprite.CustomerIcon
                };
            }
        }
    }

    public class CustomerInfo
    {
        public string Name;
        public Sprite CustomerSprite;
        public Sprite CustomerIcon;

    }
}