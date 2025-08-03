using FruitBowlScene;
using Fruits;
using Input;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class SpawnFruitButton : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private InputManagerSO inputManager;
        [SerializeField] private FruitSpawner fruitSpawner;
        [SerializeField] private Button spawnButton;
        [SerializeField] private Image nextFruitImage;
        [SerializeField] private Image nextFruitShadowImage;

        private FruitSO _nextFruit;
        
        private void Awake()
        {
            inputManager.SpawnFruitHotkeyEvent += OnSpawnFruitHotkeyPressed;
            spawnButton.onClick.AddListener(() =>
            {
                if (!fruitSpawner.SpawnFruit(_nextFruit))
                    return;
                PrepareNextFruit();
            });
        }

        private void OnSpawnFruitHotkeyPressed(bool started)
        {
            MoveSceneHotKey.PressButton(started, spawnButton);
        }

        private void Start()
        {
            PrepareNextFruit();
        }

        private void PrepareNextFruit()
        {
            _nextFruit = fruitSpawner.GetRandomFruitFromSpawnPool();
            
            nextFruitImage.sprite = _nextFruit.FruitIcon;
            nextFruitShadowImage.sprite = _nextFruit.FruitIcon;
        }
    }
}
