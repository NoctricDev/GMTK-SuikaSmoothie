using Input;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Scenes
{
    public class BowlScene : SerializedMonoBehaviour, IGameplayScene
    {
        [SerializeField, Required] private InputManagerSO inputManager;
        [SerializeField] private GameObject bowlSceneCamera;

        [SerializeField] private IGameplaySceneObject[] sceneObjects;
        public GameplayScenes Scene => GameplayScenes.Bowl;

        private void Awake()
        {
            Unload();
        }

        public void LoadStart(float durationTime)
        {
            bowlSceneCamera.SetActive(true);
            Debug.Log("Loading Started");
            sceneObjects.ForEach(s => s.LoadStart(durationTime));
        }

        public void LoadEnd()
        {
            Debug.Log("Loading Completed");
            inputManager.EnableActionMap(InputManagerSO.ActionMaps.BowlScene);
            sceneObjects.ForEach(s => s.LoadEnd());
        }

        public void Unload()
        {
            sceneObjects.ForEach(s => s.Unload());
            inputManager.DisableActionMap(InputManagerSO.ActionMaps.BowlScene);
            bowlSceneCamera.SetActive(false);
        }
    }
}
