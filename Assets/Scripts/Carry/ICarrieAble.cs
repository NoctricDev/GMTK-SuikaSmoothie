using UnityEngine;

namespace Carry
{
    public interface ICarrieAble
    {
        public bool TryStartCarry(Transform carryTransform, out ICarrieAble carryAble);

        public void StopCarry();

        public Vector3 GetPosition();

    }
}