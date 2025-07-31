using Input;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scenes
{
    public class BowlScene : MonoBehaviour, IGameplayScene
    {
        [SerializeField, Required] private InputManagerSO inputManager;
        [SerializeField] private GameObject bowlSceneCamera;
        public GameplayScenes Scene => GameplayScenes.Bowl;

        private void Awake()
        {
            Unload();
        }

        public void LoadStart(float durationTime)
        {
            bowlSceneCamera.SetActive(true);
            Debug.Log("Loading Started");
        }

        public void LoadEnd()
        {
            Debug.Log("Loading Completed");
            inputManager.EnableActionMap(InputManagerSO.ActionMaps.BowlScene);
        }

        public void Unload()
        {
            inputManager.DisableActionMap(InputManagerSO.ActionMaps.BowlScene);
            bowlSceneCamera.SetActive(false);
        }
    }
}
