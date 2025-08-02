using JohaToolkit.UnityEngine.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MixerScene
{
    public class PhysicsBasedMouseTarget : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private Transform targetMouse;

        [Title("Settings")]
        private Rigidbody _rigidbody;
    
        private void Awake()
        {
            _rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void FixedUpdate()
        {
            if (!targetMouse)
                return;
            Vector3 direction = Vector3.zero;
            if (Vector3.Distance(targetMouse.position, transform.position) > 0.001f)
                direction = (targetMouse.position - transform.position);
            _rigidbody.linearVelocity = direction / Time.fixedDeltaTime;
        }

        public void SetTargetMouse(Transform target) => targetMouse = target;
    }
}
