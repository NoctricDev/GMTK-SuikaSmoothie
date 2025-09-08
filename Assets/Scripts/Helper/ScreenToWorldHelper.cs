using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Helper
{
    public static class ScreenToWorldHelper
    {
        public static Ray GetMouseToWorldRay(UnityEngine.Camera _camera)
        {
            Ray ray = GetScreenToWorldRay(_camera, LimitToScreenBounds(Mouse.current.position.ReadValue()));
            return ray;
        }
        
        public static Vector2 LimitToScreenBounds(Vector2 position)
        {
            position.x = Mathf.Clamp(position.x, 0, Screen.width);
            position.y = Mathf.Clamp(position.y, 0, Screen.height);
            return position;
        }

        public static Ray GetScreenToWorldRay(UnityEngine.Camera _camera, Vector2 position) => _camera.ScreenPointToRay(position);

        public static float GetRayDistanceToWorldSpaceHeight(Ray ray, float height)
        {
            float angle = Vector3.Angle(Vector3.up, ray.direction);
            float heightDiff = height - ray.origin.y;
            return heightDiff / Mathf.Cos(angle * Mathf.Deg2Rad);
        }

        public static float GetRayDistanceToWorldSpaceForward(Ray ray, Vector3 forward)
        {
            float angle = Vector3.Angle(forward.normalized, ray.direction);
            float heightDiff = forward.magnitude;
            return heightDiff / Mathf.Cos(angle * Mathf.Deg2Rad);
        }

        public static bool IsMouseOverUI() => IsMouseOverUI(out _);

        private static PointerEventData _eventData;
        
        public static bool IsMouseOverUI(out PointerEventData eventData)
        {
            eventData = _eventData;
            if (EventSystem.current == null)
                return false;

            _eventData ??= new(EventSystem.current);
            _eventData.position = Mouse.current.position.ReadValue();
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(_eventData, results);
            foreach (RaycastResult raycastResult in results)
            {
                if (raycastResult.gameObject.transform is RectTransform)
                    return true;
            }

            return false;
        }
    }
}
