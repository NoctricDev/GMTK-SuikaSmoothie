using System;
using System.Collections.Generic;
using JohaToolkit.UnityEngine;
using JoHaToolkit.UnityEngine.CheatConsole;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    [CreateAssetMenu(fileName = "InputManagerSO", menuName = "Scriptable Objects/InputManagerSO")]
    public class InputManagerSO : RuntimeScriptableObject, InputSystem_Actions.IBowlSceneActions
    {
        public static InputManagerSO Instance { get; private set; }
        public enum ActionMaps
        {
            BowlScene
        }
        
        private InputSystem_Actions _inputActions;

        private List<ActionMaps> _enabledActionMaps;

        #region Events

        public event Action<ActionMaps, bool> ActionMapActiveEvent;
        
        public event Action<float> BowlSceneCameraMoveEvent;
        
        public event Action InteractPrimaryEvent;

        public event Action InteractSecondaryEvent;
        
        public event Action<bool> SpawnFruitHotkeyEvent;
        
        #endregion
        
        private void OnEnable()
        {
            Instance = this;
            _enabledActionMaps = new List<ActionMaps>();
            _inputActions = new InputSystem_Actions();
            _inputActions.Enable();
            _inputActions.BowlScene.Disable();
            _inputActions.BowlScene.SetCallbacks(this);
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        protected override void OnReset()
        {
            OnDisable();
            OnEnable();
        }

        #region Cheats

        [CheatCommand]
        public static void SetActionMapActive(ActionMaps actionMap, bool active)
        {
            if(active)
                Instance?.EnableActionMap(actionMap);
            else
                Instance.DisableActionMap(actionMap);
        }

        #endregion

        public void EnableActionMap(ActionMaps actionMap)
        {
            if(_enabledActionMaps.Contains(actionMap)) 
                return;
            EnumToActionMap(actionMap).Enable();
            ActionMapActiveEvent?.Invoke(actionMap, true);
        }
        
        public void DisableActionMap(ActionMaps actionMap)
        {
            if(!_enabledActionMaps.Contains(actionMap)) 
                return;
            EnumToActionMap(actionMap).Disable();
            ActionMapActiveEvent?.Invoke(actionMap, false);
        }

        private InputActionMap EnumToActionMap(ActionMaps actionMap)
        {
            return actionMap switch
            {
                ActionMaps.BowlScene => _inputActions.BowlScene,
                _ => throw new ArgumentOutOfRangeException(nameof(actionMap), actionMap, null)
            };
        }
        
        public void OnMoveCamera(InputAction.CallbackContext context)
        {
            BowlSceneCameraMoveEvent?.Invoke(context.ReadValue<float>());
        }

        public void OnInteractPrimary(InputAction.CallbackContext context)
        {
            if(context.performed)
                InteractPrimaryEvent?.Invoke();
        }

        public void OnInteractSecondary(InputAction.CallbackContext context)
        {
            if(context.performed)
                InteractSecondaryEvent?.Invoke();
        }

        public void OnSpawnFruitHotkey(InputAction.CallbackContext context)
        {
            if(context.started)
                SpawnFruitHotkeyEvent?.Invoke(true);
            else if(context.canceled)
                SpawnFruitHotkeyEvent?.Invoke(false);
        }
    }
}
