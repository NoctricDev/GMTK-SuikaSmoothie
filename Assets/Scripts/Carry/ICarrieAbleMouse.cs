using Carry;

namespace FruitBowlScene
{
    public interface ICarrieAbleMouse
    {
        public bool HasPayload { get; }
        public void StartCarry(ICarrieAble carry);
        public void StopCarry();
    }
}