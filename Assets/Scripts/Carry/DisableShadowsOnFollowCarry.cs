using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Rendering;

namespace Carry
{
    public class DisableShadowsOnFollowCarry : MonoBehaviour
    {
        [Title("References")]
        [SerializeField, Required] private FollowCarry followCarry;

        [SerializeField] private MeshRenderer[] meshRenderer;

        private void OnEnable()
        {
            followCarry.CarryStartedEvent += OnCarryStartedEvent;
            followCarry.CarryStoppedEvent += OnCarryStoppedEvent;
        }

        private void OnDisable()
        {
            if (followCarry) 
                followCarry.CarryStartedEvent -= OnCarryStartedEvent;

            if (followCarry) 
                followCarry.CarryStoppedEvent -= OnCarryStoppedEvent;
        }

        private void OnCarryStartedEvent()
        {
            meshRenderer.ForEach(m => m.shadowCastingMode = ShadowCastingMode.Off);
        }

        private void OnCarryStoppedEvent()
        {
            meshRenderer.ForEach(m => m.shadowCastingMode = ShadowCastingMode.On);
        }
    }
}
