using FruitBowlScene;
using UnityEngine;

namespace Carry
{
    public interface ICarrieAble
    {
        public bool TryStartCarry(Transform carryTransform, ICarrieAbleMouse mouseCarry);

        public void OnStopCarry();

        public GameObject GetAttachedGameObject();

    }
}