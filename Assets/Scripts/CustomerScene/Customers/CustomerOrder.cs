using Glasses;

namespace CustomerScene.Customers
{
    public class CustomerOrder
    {
        public SmoothieContent Content { get; private set; }
        public float TimeToPrepare { get; private set; }
        private CustomerOrder(SmoothieContent content, float timeToPrepare)
        {
            Content = content;
            TimeToPrepare = timeToPrepare;
        }

        public class Builder
        {
            private SmoothieContent _content;
            private float _timeToPrepare = 0;
            public Builder(SmoothieContent content) => _content = content;

            public Builder WithTimeToPrepare(float time)
            {
                _timeToPrepare = time;
                return this;
            }

            public CustomerOrder Build() => new(_content, _timeToPrepare);
        }
    }
}