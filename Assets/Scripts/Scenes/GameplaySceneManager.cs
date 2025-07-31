#nullable enable
using System;
using System.Collections.Generic;
using JohaToolkit.UnityEngine.DataStructures;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scenes
{
    public class GameplaySceneManager : MonoBehaviourSingleton<GameplaySceneManager>
    {
        [Title("Gameplay Scenes (Registered automatically")]
        [SerializeField] private List<GameObject> gameplayScenes = new();

        public List<IGameplayScene> GameplayScenes { get; private set; } = new();

        [SerializeField, Required] private GameObject startScene = null!;
        
        [Header("Gameplay Scene Manager Settings")]
        [SerializeField] private float sceneTransitionDuration = 1f;


        public event Action<bool> CurrentSceneChangedEvent = null!;
        
        public IGameplayScene? CurrentScene { get; private set; }

        public bool IsLoadingScene { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            GameplayScenes = new List<IGameplayScene>();
            foreach (GameObject gameplayScene in gameplayScenes)
            {
                if (!gameplayScene.TryGetComponent(out IGameplayScene scene))
                {
                    Debug.LogWarning("[GameplaySceneManager] GameObject " + gameplayScene.name + " does not implement IGameplayScene interface!");
                    return;
                }
                GameplayScenes.Add(scene);
            }
        }

        private void Start()
        {
            LoadGameplayScene(startScene.GetComponent<IGameplayScene>().Scene);
        }

        public void LoadGameplayScene(GameplayScenes scene)
        {
            if (IsLoadingScene)
            {
                Debug.Log("Already loading a scene!");
                return;
            }
            IGameplayScene? sceneToLoad = GetScene(scene);
            if (sceneToLoad == null)
            {
                Debug.Log("[SceneManager] Scene not found: " + scene);
                return;
            }
            
            _ = LoadGameplaySceneAsync(sceneToLoad);
        }

        private async Awaitable LoadGameplaySceneAsync(IGameplayScene scene)
        {
            IsLoadingScene = true;
            CurrentScene?.Unload();
            CurrentScene = scene;
            CurrentScene.LoadStart(sceneTransitionDuration);
            CurrentSceneChangedEvent?.Invoke(false);
            
            await Awaitable.WaitForSecondsAsync(sceneTransitionDuration);
            
            CurrentScene.LoadEnd();
            CurrentSceneChangedEvent?.Invoke(true);
            
            IsLoadingScene = false;
        }

        private IGameplayScene? GetScene(GameplayScenes scene) => GameplayScenes.Find(s => s.Scene == scene);
    }
}