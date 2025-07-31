using Fruits;
using Input;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FruitBowlScene
{
    public class FruitDropManager : MonoBehaviour
    {
        [Title("References")]
        [SerializeField, Required] private InputManagerSO inputManager;

        [SerializeField] private FruitSO testFruit;

        [SerializeField] private FruitSpawner spawnerPoint;
        
        [Title("Settings")]
        [SerializeField] private FloatVariable dropHeight;

        private UnityEngine.Camera _mainCam;

        private void OnEnable()
        {
            inputManager.InteractPrimaryEvent += OnInteractPrimary;
            _mainCam = UnityEngine.Camera.main;
        }

        private void OnDisable()
        {
            inputManager.InteractPrimaryEvent -= OnInteractPrimary;
        }

        private void OnInteractPrimary()
        {
            // TODO: Drop Checks:
            Ray ray = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            spawnerPoint.SpawnFruit(testFruit, ray.GetPoint(GetDistanceToDropPoint(ray)));
            
        }

        private void Update()
        {
            Ray ray = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.DrawRay(ray.origin, ray.direction * GetDistanceToDropPoint(ray), Color.red);
            spawnerPoint.transform.position = ray.GetPoint(GetDistanceToDropPoint(ray));
            
        }

        private float GetDistanceToDropPoint(Ray ray)
        {
            float angle = Vector3.Angle(Vector3.up, ray.direction);
            float heightDiff = dropHeight.Value - ray.origin.y;
            return heightDiff / Mathf.Cos(angle * Mathf.Deg2Rad);
        }
    }
}
