using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Helper
{
    public class DrawGizmos : MonoBehaviour
    {
        protected enum Shapes
        {
            Sphere,
            Cube,
        }

        [Title("Settings")] 
        [SerializeField] protected bool drawGizmos = true;
        [SerializeField] protected bool drawGizmosSelected = true;
        [SerializeField] protected Shapes shape;
        [SerializeField] protected bool showWired;

        [BoxGroup("Sphere Settings"), ShowIf(nameof(shape), Shapes.Sphere)]
        [SerializeField] protected float radius = 0.5f;
        
        [BoxGroup("Cube Settings"), ShowIf(nameof(shape), Shapes.Cube)]
        [SerializeField] protected Vector3 cubeSize = Vector3.one;

        [SerializeField] protected Vector3 offset = Vector3.zero;
        
        [SerializeField] protected Color gizmoColor = Color.black;
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
            switch (shape)
            {
                case Shapes.Sphere when !showWired:
                    Gizmos.DrawSphere(transform.position + offset, radius);
                    break;
                case Shapes.Sphere when showWired:
                    Gizmos.DrawWireSphere(transform.position + offset, radius);
                    break;
                case Shapes.Cube when !showWired:
                    Gizmos.DrawCube(transform.position + offset, cubeSize);
                    break;
                case Shapes.Cube when showWired:
                    Gizmos.DrawWireCube(transform.position + offset, cubeSize);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
