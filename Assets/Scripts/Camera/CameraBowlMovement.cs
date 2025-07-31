using System;
using Input;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CameraBowlMovement : MonoBehaviour
    {
        [Title("References")]
        [SerializeField, Required] private InputManagerSO inputManager;
        [SerializeField, Required, ChildGameObjectsOnly] private CinemachineSplineDolly cinemachineSplineDolly;
        
        [Title("Settings")]
        [SerializeField] private float speed;

        [SerializeField] private bool invert;

        private float _currentPosition;
        private float _desiredDelta;

        private void OnEnable()
        {
            inputManager.BowlSceneCameraMoveEvent += OnCameraMove;
        }

        private void OnCameraMove(float dir)
        {
            _desiredDelta = dir * speed;
        }

        private void Update()
        {
            _currentPosition += (invert? -1 : 1) * _desiredDelta * Time.deltaTime;
            _currentPosition %= 1;
            cinemachineSplineDolly.CameraPosition = _currentPosition;
        }
    }
}
