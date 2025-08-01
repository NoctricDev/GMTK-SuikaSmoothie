using System;
using FruitBowlScene;
using Fruits;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SpawnFruitButton : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private FruitSpawner fruitSpawner;
        [SerializeField] private Button spawnButton;
        [SerializeField] private TextMeshProUGUI buttonText;

        private FruitSO _nextFruit;
        
        private void Awake()
        {
            spawnButton.onClick.AddListener(() =>
            {
                fruitSpawner.SpawnFruit(_nextFruit);
                PrepareNextFruit();
            });
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
