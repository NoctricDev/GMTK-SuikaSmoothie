using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using UnityEngine;

namespace Helper
{
    public class MixerInteractionPlane : MonoBehaviour
    {
        [SerializeField] private FloatVariable distanceFromCam;

        [SerializeField] private Transform _camera;
        [SerializeField] private float cameraNearPlane;
        
        private void Start()
        {
            OnDropHeightChanged(distanceFromCam.Value);
            distanceFromCam.OnValueChanged += OnDropHeightChanged;
        }

        private void OnDropHeightChanged(float newHeight)
        {
            if (!_camera)
                return;
            transform.position = _camera.transform.position + _camera.transform.forward.SetY(0) * (newHeight + cameraNearPlane);
            transform.rotation = Quaternion.LookRotation(_camera.transform.forward.SetY(0), Vector3.up);
        }
    }
}
