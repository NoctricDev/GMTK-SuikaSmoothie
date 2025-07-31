using UnityEngine;

namespace Helper
{
    public class DrawGizmos : MonoBehaviour
    {
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private bool drawGizmosSelected = true;
        [SerializeField] private float radius = 0.5f;
        [SerializeField] private Color gizmoColor;
        private void OnDrawGizmos()
        {
            if (!drawGizmos)
                return;
            DrawGiz();
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawGizmosSelected || drawGizmos)
                return;
            DrawGiz();
        }

        private void DrawGiz()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, radius);
        }
    }
}
