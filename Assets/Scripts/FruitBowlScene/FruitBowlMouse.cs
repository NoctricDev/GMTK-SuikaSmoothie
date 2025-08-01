using System;
using Carry;
using Input;
using JetBrains.Annotations;
using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FruitBowlScene
{
    public class FruitBowlMouse : MonoBehaviour
    {
        [Title("References")]
        [SerializeField, Required] private InputManagerSO inputManager;
        [SerializeField, Required] private FloatVariable dropHeight;

        [Title("Settings")] 
        [SerializeField] private float interactionRadius;
        [SerializeField] private Vector2 minMaxDistance;

        [CanBeNull] private ICarrieAble _currentPayload;

        public event Action<ICarrieAble> PayloadPickedUpEvent;
        public event Action<ICarrieAble> PayloadDroppedEvent;

        private UnityEngine.Camera _mainCam;
        
        private void OnEnable()
        {
            inputManager.InteractPrimaryEvent += OnInteract;
        }

        private void OnDisable()
        {
            inputManager.InteractPrimaryEvent -= OnInteract;
        }

        private void Awake()
        {
            _mainCam = UnityEngine.Camera.main;
        }

        private void OnInteract()
        {
            if (_currentPayload != null)
            {
                PayloadDroppedEvent?.Invoke(_currentPayload);
                _currentPayload.StopCarry();
                _currentPayload = null;
                return;
            }

            Collider[] overlaps = Physics.OverlapSphere(transform.position, interactionRadius);

            foreach (Collider col in overlaps)
            {
                if (!col.TryGetComponent(out ICarrieAble carrieAble)) 
                    continue;
                
                if(!carrieAble.TryStartCarry(transform, out ICarrieAble activeCarrieAble))
                    break;
                
                PayloadPickedUpEvent?.Invoke(activeCarrieAble);
                _currentPayload = activeCarrieAble;
                return;
            }

            _currentPayload = null;
        }
        
        private void Update()
        {
            Ray ray = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            float dist = GetDistanceToDropPoint(ray);
            float horizontalDist = (ray.GetPoint(dist).SetY(0) - _mainCam.transform.position.SetY(0)).magnitude;
            
            Debug.DrawRay(ray.origin, ray.direction * dist, Color.red);
            
            if(horizontalDist > minMaxDistance.y || horizontalDist < minMaxDistance.x)
            {
                return;
            }

            transform.position = ray.GetPoint(dist);
        }

        private float GetDistanceToDropPoint(Ray ray)
        {
            float angle = Vector3.Angle(Vector3.up, ray.direction);
            float heightDiff = dropHeight.Value - ray.origin.y;
            return heightDiff / Mathf.Cos(angle * Mathf.Deg2Rad);
        }
    }
}
