using JohaToolkit.UnityEngine.Extensions;
using JohaToolkit.UnityEngine.ScriptableObjects.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Helper
{
    public class DrawDropHeightPlane : DrawGizmos
    {
        [SerializeField, OnValueChanged(nameof(NewHeightValue))] private FloatVariable dropHeight;

        private void Start()
        {
            OnDropHeightChanged(dropHeight.Value);
            dropHeight.OnValueChanged += OnDropHeightChanged;
        }

        private void NewHeightValue(FloatVariable newHeight)
        {
            if (newHeight == null)
                return;
            OnDropHeightChanged(newHeight.Value);
        }

        private void OnDropHeightChanged(float newHeight)
        {
            transform.position = transform.position.SetY(newHeight);
        }
    }
}
