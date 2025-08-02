using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace MixerScene.Mixer
{
    public class MixerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Pointer Click!");
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Pointer UP!");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Pointer DOWN!");
        }
    }
}
