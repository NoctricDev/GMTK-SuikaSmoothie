using UnityEngine;

namespace Carry
{
    public interface ICarrieAble
    {
        public bool TryStartCarry(Transform carryTransform);

        public void StopCarry();

        public Vector3 GetPosition();

        public GameObject GetAttachedGameObject();

    }
}