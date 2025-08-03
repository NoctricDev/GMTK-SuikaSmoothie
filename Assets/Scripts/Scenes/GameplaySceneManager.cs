#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JohaToolkit.UnityEngine.DataStructures;
using JohaToolkit.UnityEngine.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scenes
{
    public class GameplaySceneManager : MonoBehaviourSingleton<GameplaySceneManager>
    {
        [Title("Gameplay Scenes (Registered automatically")]
        [SerializeField] private List<GameObject> gameplayScenes = new();

        public List<IGameplayScene> GameplayScenes { get; private set; } = new();

        
        [Title("Gameplay Scene Manager Settings")]
        [SerializeField, Required] private GameplayScenes startScene;
        [SerializeField] private float sceneTransitionDuration = 1f;


        public event Action<bool> CurrentSceneChangedEvent = null!;
        
        public IGameplayScene? CurrentScene { get; private set; }
        public GameplayScenes CurrentSceneName => CurrentScene?.Scene ?? GameplayScenes[0].Scene;

        public bool IsLoadingScene { get; private set; }

        private CancellationTokenSource _cts;
        
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
            Application.targetFrameRate = 144;
            LoadGameplayScene(startScene);
        }

        public void LoadGameplayScene(GameplayScenes scene, bool force = false)
        {
            if (force)
            {
                _cts?.Cancel();
                IsLoadingScene = false;
                CurrentScene?.Unload();
            }
            
            if (IsLoadingScene)
            {
                Debug.Log("Already loading a scene!");
                return;
            }

            _cts = new CancellationTokenSource();
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

            try
            {
                await Task.Delay(sceneTransitionDuration.Seconds(), _cts.Token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            if (_cts.IsCancellationRequested)
                return;
            CurrentScene.LoadEnd();
            CurrentSceneChangedEvent?.Invoke(true);
            
            IsLoadingScene = false;
        }

        private IGameplayScene? GetScene(GameplayScenes scene) => GameplayScenes.Find(s => s.Scene == scene);
    }
}