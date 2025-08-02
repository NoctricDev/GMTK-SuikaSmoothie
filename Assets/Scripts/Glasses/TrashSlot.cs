using Carry;

namespace Glasses
{
    public class TrashSlot : Slot
    {
        public override void StartCarry(ICarrieAble carry)
        {
            Destroy(carry.GetAttachedGameObject());
        }
    }
}
