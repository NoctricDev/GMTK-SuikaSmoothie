using System.Linq;
using Scenes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MixerScene.Mixer
{
    public class GameObjectButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField] private GameplayScenes[] buttonEnabledInScenes;
        
        public bool IsActive => buttonEnabledInScenes.Any(s => buttonEnabledInScenes.Contains(s));

        public UnityEvent pointerClick;
        public UnityEvent pointerUp;
        public UnityEvent pointerDown;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsActive)
                return;
            pointerClick?.Invoke();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!IsActive)
                return;
            pointerUp?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsActive)
                return;
            pointerDown?.Invoke();
        }
    }
}
