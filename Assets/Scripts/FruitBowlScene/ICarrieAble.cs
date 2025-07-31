using UnityEngine;

namespace FruitBowlScene
{
    public interface ICarrieAble
    {
        public bool TryStartCarry(Transform carryTransform);

        public void StopCarry();

    }
}