using System.Linq;
using Scenes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MixerScene.Mixer
{
    public class GameObjectButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameplayScenes[] buttonEnabledInScenes;
        
        public bool IsActive => buttonEnabledInScenes.Any(s => buttonEnabledInScenes.Contains(s));

        public UnityEvent pointerClick;
        public UnityEvent pointerUp;
        public UnityEvent pointerDown;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Clicked {gameObject.name}");
            if (!IsActive)
                return;
            pointerClick?.Invoke();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log($"Pointer Up {gameObject.name}");
            if (!IsActive)
                return;
            pointerUp?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log($"Pointer Down {gameObject.name}");
            if (!IsActive)
                return;
            pointerDown?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log($"Pointer Enter {gameObject.name}");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log($"Pointer Exit {gameObject.name}");
        }
        
    }
}
