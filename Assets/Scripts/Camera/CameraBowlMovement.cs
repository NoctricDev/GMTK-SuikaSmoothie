using System;
using Input;
using Scenes;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CameraBowlMovement : MonoBehaviour, IGameplaySceneObject
    {
        [Title("References")]
        [SerializeField, Required] private InputManagerSO inputManager;
        [SerializeField, Required, ChildGameObjectsOnly] private CinemachineSplineDolly cinemachineSplineDolly;
        
        [Title("Settings")]
        [SerializeField] private float speed;

        [SerializeField] private bool invert;

        private float _currentPosition;
        private float _desiredDelta;

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

        public void LoadStart(float durationTime) { }

        public void LoadEnd()
        {
            inputManager.BowlSceneCameraMoveEvent += OnCameraMove;
        }

        public void Unload()
        {
            inputManager.BowlSceneCameraMoveEvent -= OnCameraMove;
        }
    }
}
