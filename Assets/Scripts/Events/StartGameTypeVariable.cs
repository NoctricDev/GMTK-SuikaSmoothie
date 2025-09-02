using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "NewVariableStartGameType",
        menuName = "JoHaToolkit/Variables/SingleArg/GameEventStartGameType")]
    public class StartGameTypeVariable : SOVariableBase<StartGameType>
    { }

    public enum StartGameType
    {
        Dummy,
        Tutorial,
        Freeplay,
        Challenge
    }
}