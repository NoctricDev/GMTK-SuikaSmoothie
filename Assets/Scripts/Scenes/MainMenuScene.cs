using Input;
using UnityEngine;

namespace Scenes
{
    public class MainMenuScene : MonoBehaviour, IGameplaySceneObject
    {
        public void LoadStart(float durationTime)
        {
            InputManagerSO.Instance.DisableActionMap(InputManagerSO.ActionMaps.General);
            Time.timeScale = 0;
        }

        public void Unload()
        {
            Time.timeScale = 1;
            InputManagerSO.Instance.EnableActionMap(InputManagerSO.ActionMaps.General);
        }
    }
}
