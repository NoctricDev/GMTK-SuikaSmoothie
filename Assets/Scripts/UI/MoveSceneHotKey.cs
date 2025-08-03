using Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class MoveSceneHotKey : MonoBehaviour
    {
        [SerializeField] private InputManagerSO inputManager;
        [SerializeField] private Button moveSceneLeftButton;
        [SerializeField] private Button moveSceneRightButton;
        private void Awake()
        {
            inputManager.MoveSceneLeftEvent += OnMoveSceneLeftHotkeyPressed;
            inputManager.MoveSceneRightEvent += OnMoveSceneRightHotkeyPressed;
        }

        private void OnMoveSceneRightHotkeyPressed(bool started)
        {
            PressButton(started, moveSceneRightButton);
        }

        private void OnMoveSceneLeftHotkeyPressed(bool started)
        {
            PressButton(started, moveSceneLeftButton);
        }

        public static void PressButton(bool started, Button button)
        {
            if(started)
                ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            else
            {
                ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current),
                    ExecuteEvents.pointerClickHandler);
                ExecuteEvents.Execute(button.gameObject, new PointerEventData(EventSystem.current),
                    ExecuteEvents.pointerUpHandler);
            }
        }
    }
}
