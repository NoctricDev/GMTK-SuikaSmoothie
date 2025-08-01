using Sirenix.OdinInspector;
using UnityEngine;

namespace Mixer
{
    public class MixerMouseTarget : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private MixerMouse mixerMouse;
        
        private Transform _mixerMouseTransform;
    
        private void Awake()
        {
            _mixerMouseTransform = mixerMouse.transform;
        }

        private void FixedUpdate()
        {
            if (Physics.CheckSphere(_mixerMouseTransform.position, 0.02f, LayerMask.GetMask("MouseMoveLayer")))
                return;
            transform.position = _mixerMouseTransform.position;
        }
    }
}
