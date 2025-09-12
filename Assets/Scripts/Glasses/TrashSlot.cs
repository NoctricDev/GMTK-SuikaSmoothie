using Carry;
using JohaToolkit.UnityEngine.ScriptableObjects.Events;
using UnityEngine;

namespace Glasses
{
    public class TrashSlot : Slot
    {
        [SerializeField] private GameEvent carryAbleTrashedEvent;
        public override void StartCarry(ICarrieAble carry)
        {
            carryAbleTrashedEvent?.RaiseEvent(this);
            Destroy(carry.GetAttachedGameObject());
        }
    }
}
