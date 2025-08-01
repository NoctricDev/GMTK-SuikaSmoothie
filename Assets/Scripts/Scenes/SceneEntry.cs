using Input;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Scenes
{
    public class SceneEntry : SerializedMonoBehaviour, IGameplayScene
    {
        [SerializeField, Required] private InputManagerSO inputManager;
        [SerializeField] private GameObject sceneCamera;

        [SerializeField] private IGameplaySceneObject[] sceneObjects;

        [SerializeField] private GameplayScenes gameplayScene;
        [SerializeField] private InputManagerSO.ActionMaps[] actionMaps;
        
        public GameplayScenes Scene => gameplayScene;

        private void Awake()
        {
            Unload();
        }

        public void LoadStart(float durationTime)
        {
            sceneCamera.SetActive(true);
            Debug.Log("Loading Started");
            sceneObjects.ForEach(s => s.LoadStart(durationTime));
        }

        public void LoadEnd()
        {
            Debug.Log("Loading Completed");
            actionMaps.ForEach(inputManager.EnableActionMap);
            sceneObjects.ForEach(s => s.LoadEnd());
        }

        public void Unload()
        {
            sceneObjects.ForEach(s => s.Unload());
            actionMaps.ForEach(inputManager.DisableActionMap);
            sceneCamera.SetActive(false);
        }
    }
}
