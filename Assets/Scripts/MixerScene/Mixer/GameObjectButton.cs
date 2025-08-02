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

        public UnityAction PointerClick;
        public UnityAction PointerUp;
        public UnityAction PointerDown;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsActive)
                return;
            PointerClick?.Invoke();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!IsActive)
                return;
            PointerUp?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsActive)
                return;
            PointerDown?.Invoke();
        }
    }
}
