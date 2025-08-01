using System;
using FruitBowlScene;
using Fruits;
using Input;
using Sirenix.OdinInspector;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI buttonText;

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
            if(started)
                ExecuteEvents.Execute(spawnButton.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            else
            {
                ExecuteEvents.Execute(spawnButton.gameObject, new PointerEventData(EventSystem.current),
                    ExecuteEvents.pointerClickHandler);
                ExecuteEvents.Execute(spawnButton.gameObject, new PointerEventData(EventSystem.current),
                    ExecuteEvents.pointerUpHandler);
            }
        }

        private void Start()
        {
            PrepareNextFruit();
        }

        private void PrepareNextFruit()
        {
            _nextFruit = fruitSpawner.GetRandomFruitFromSpawnPool();
            buttonText.text = _nextFruit.FruitType.ToString();
        }
    }
}
