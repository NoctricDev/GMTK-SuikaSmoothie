using Carry;
using JohaToolkit.UnityEngine.ScriptableObjects.Events;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "NewICarrieAbleEvent", menuName = "JoHaToolkit/Events/SingleArg/GameEventICarrieAble")]
    public class GameEventICarrieAble : GameEvent<ICarrieAble>
    {
    }
}
