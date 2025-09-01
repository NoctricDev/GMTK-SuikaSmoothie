using System;
using Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class MoveSceneHotKey : MonoBehaviour
    {
        [SerializeField] private InputManagerSO inputManager;
        [SerializeField] private GameObject moveSceneLeftButton;
        [SerializeField] private GameObject moveSceneRightButton;
        private void Awake()
        {
            inputManager.MoveSceneLeftEvent += OnMoveSceneLeftHotkeyPressed;
            inputManager.MoveSceneRightEvent += OnMoveSceneRightHotkeyPressed;
        }

        private void OnDestroy()
        {
            inputManager.MoveSceneLeftEvent -= OnMoveSceneLeftHotkeyPressed;
            inputManager.MoveSceneRightEvent -= OnMoveSceneRightHotkeyPressed;
        }

        private void OnMoveSceneRightHotkeyPressed(bool started)
        {
            PressButton(started, moveSceneRightButton);
        }

        private void OnMoveSceneLeftHotkeyPressed(bool started)
        {
            PressButton(started, moveSceneLeftButton);
        }

        public static void PressButton(bool started, GameObject button)
        {
            if(started)
                ExecuteEvents.Execute(button, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            else
            {
                ExecuteEvents.Execute(button, new PointerEventData(EventSystem.current),
                    ExecuteEvents.pointerClickHandler);
                ExecuteEvents.Execute(button, new PointerEventData(EventSystem.current),
                    ExecuteEvents.pointerUpHandler);
            }
        }
    }
}
