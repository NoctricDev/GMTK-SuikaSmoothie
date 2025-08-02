using Carry;
using FruitBowlScene;
using UnityEngine;

namespace Glasses
{
    public class Slot : MonoBehaviour, ICarrieAbleMouse
    {
        public ICarrieAble CurrentCarrieAble { get; private set; }
        
        [SerializeField] private Transform slotTransform;

        public bool HasPayload => CurrentCarrieAble != null;
        public bool IsLocked { get; set; }

        public void SetSlot(ICarrieAble carrieAble)
        {
            if (IsLocked)
                return;
            StartCarry(carrieAble);
        }

        public ICarrieAble RemoveSlot()
        {
            if (IsLocked)
            {
                return null;
            }
            
            if (!HasPayload)
            {
                Debug.Log("[Slot] Cannot remove carrieAble, slot is empty");
                return null;
            }
            ICarrieAble carry = CurrentCarrieAble;
            StopCarry();
            return carry;
        }
        
        public void StartCarry(ICarrieAble carry)
        {
            if (IsLocked)
                return;
            if (HasPayload)
            {
                Debug.Log("[Slot] Cannot set carrieAble, slot is not empty");
                return;
            }
            
            CurrentCarrieAble = carry;
            carry.TryStartCarry(slotTransform, null);
        }

        public void StopCarry()
        {
            if (IsLocked)
                return;
            
            if (!HasPayload)
            {
                Debug.Log("[Slot] Cannot remove carrieAble, slot is empty");
                return;
            }
            CurrentCarrieAble.OnStopCarry();
            CurrentCarrieAble = null;
        }
    }
}
