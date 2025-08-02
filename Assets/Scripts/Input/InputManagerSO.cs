using System;
using System.Collections.Generic;
using JohaToolkit.UnityEngine;
using JoHaToolkit.UnityEngine.CheatConsole;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    [CreateAssetMenu(fileName = "InputManagerSO", menuName = "Scriptable Objects/InputManagerSO")]
    public class InputManagerSO : RuntimeScriptableObject, InputSystem_Actions.IBowlSceneActions, InputSystem_Actions.IMixerSceneActions, InputSystem_Actions.ICustomerSceneActions
    {
        public static InputManagerSO Instance { get; private set; }
        public enum ActionMaps
        {
            BowlScene,
            MixerScene,
            CustomerScene
        }
        
        private InputSystem_Actions _inputActions;
        public InputSystem_Actions InputActions => _inputActions;
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
            SetupInputActions();
        }

        private void SetupInputActions()
        {
            _inputActions.BowlScene.SetCallbacks(this);
            _inputActions.MixerScene.SetCallbacks(this);
            _inputActions.CustomerScene.SetCallbacks(this);
            
            _inputActions.BowlScene.Disable();
            _inputActions.MixerScene.Disable();
            _inputActions.CustomerScene.Disable();
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
        
        [CheatCommand]
        public static void PrintActiveActionMaps()
        {
            Debug.Log("Mixer: " + Instance.InputActions.MixerScene.enabled);
            Debug.Log("Bowl: " + Instance.InputActions.BowlScene.enabled);
        }

        #endregion

        public void EnableActionMap(ActionMaps actionMap)
        {
            if(_enabledActionMaps.Contains(actionMap)) 
                return;
            EnumToActionMap(actionMap).Enable();
            _enabledActionMaps.Add(actionMap);
            ActionMapActiveEvent?.Invoke(actionMap, true);
        }
        
        public void DisableActionMap(ActionMaps actionMap)
        {
            if(!_enabledActionMaps.Contains(actionMap)) 
                return;
            _enabledActionMaps.Remove(actionMap);
            EnumToActionMap(actionMap).Disable();
            ActionMapActiveEvent?.Invoke(actionMap, false);
        }

        private InputActionMap EnumToActionMap(ActionMaps actionMap)
        {
            return actionMap switch
            {
                ActionMaps.BowlScene => _inputActions.BowlScene,
                ActionMaps.MixerScene => _inputActions.MixerScene,
                ActionMaps.CustomerScene => _inputActions.CustomerScene,
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
