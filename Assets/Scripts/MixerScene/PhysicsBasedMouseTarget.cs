using System;
using JohaToolkit.UnityEngine.Extensions;
using Scenes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MixerScene
{
    public class PhysicsBasedMouseTarget : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private Transform targetMouse;

        [Title("Settings")]
        [SerializeField] private float maxVelocity = 10f;
        private Rigidbody _rigidbody;

        private Vector3 _startPosition;
        
        private void Awake()
        {
            _rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void Start()
        {
            GameplaySceneManager.Instance.CurrentSceneChangedEvent += OnCurrentSceneChanged;
            _startPosition = transform.position;
        }
        
        private void OnDisable()
        {
            GameplaySceneManager.Instance.CurrentSceneChangedEvent -= OnCurrentSceneChanged;
        }

        private void OnCurrentSceneChanged(bool finished)
        {
            if(!finished)
                return;
            if (!GameplaySceneManager.Instance.CurrentScene.Scene.Equals(GameplayScenes.Mixer))
                return;
            transform.position = _startPosition;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(_startPosition, .02f);
        }

        private void FixedUpdate()
        {
            if (!targetMouse)
                return;
            Vector3 direction = Vector3.zero;
            if (Vector3.Distance(targetMouse.position, transform.position) > 0.001f)
                direction = (targetMouse.position - transform.position);
            _rigidbody.linearVelocity = Vector3.ClampMagnitude(direction / Time.fixedDeltaTime, maxVelocity);
            //_rigidbody.linearVelocity = direction / Time.fixedDeltaTime;
        }

        public void SetTargetMouse(Transform target) => targetMouse = target;
    }
}
