using Glasses;

namespace CustomerScene.Customers
{
    public class CustomerOrder
    {
        public SmoothieContent Content { get; private set; }
        public float TimeToPrepare { get; private set; }
        public float Price { get; private set; }
        private CustomerOrder(SmoothieContent content, float timeToPrepare, float price)
        {
            Content = content;
            TimeToPrepare = timeToPrepare;
            Price = price;
        }

        public class Builder
        {
            private SmoothieContent _content;
            private float _timeToPrepare = 0;
            private float _price = 0;
            public Builder(SmoothieContent content) => _content = content;

            public Builder WithTimeToPrepare(float time)
            {
                _timeToPrepare = time;
                return this;
            }

            public Builder WithPrice(float price)
            {
                _price = price;
                return this;
            }

            public CustomerOrder Build() => new(_content, _timeToPrepare, _price);
        }
    }
}