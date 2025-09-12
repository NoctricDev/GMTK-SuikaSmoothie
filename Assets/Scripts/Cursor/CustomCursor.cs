using System;
using System.Collections.Generic;
using System.Linq;
using Carry;
using Events;
using Glasses;
using Helper;
using JohaToolkit.UnityEngine.DataStructures;
using JohaToolkit.UnityEngine.Extensions;
using MixerScene.Mixer;
using Scenes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Cursor
{
    public class CustomCursor : MonoBehaviourSingleton<CustomCursor>
    {
        public enum CursorState
        {
            Open,
            Closed,
            Point
        }
        
        [SerializeField] private Texture2D cursorClosed;
        [SerializeField] private Texture2D cursorOpen;
        [SerializeField] private Texture2D cursorPoint;
        [SerializeField] private Vector2 hotSpot;
        
        [Title("Events")]
        [SerializeField] private GameEventICarrieAble[] payloadPickedUpEvent;
        [SerializeField] private GameEventICarrieAble[] payloadDroppedEvent;

        [SerializeField] private LayerMask layerMask;
        
        private UnityEngine.Camera _mainCam;

        private CursorState _currentCursorState;

        private bool _isCarrying;

        public CursorState CurrentCursorState
        {
            get => _currentCursorState;
            set
            {
                if (_currentCursorState != value)
                {
                    SetCursor(value);
                }
                _currentCursorState = value;
            }
        }
        protected override void Awake()
        {
            base.Awake();
            _mainCam = UnityEngine.Camera.main;
        }

        private void Start()
        {
            SetCursor(CursorState.Point);
        }

        protected override void OnEnable()
        {
            SubscribeToEvents(payloadPickedUpEvent, OnPayloadPickedUp);
            SubscribeToEvents(payloadDroppedEvent, OnPayloadDropped);
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            UnSubscribeFromEvents(payloadPickedUpEvent, OnPayloadPickedUp);
            UnSubscribeFromEvents(payloadDroppedEvent, OnPayloadDropped);
            base.OnDisable();
        }

        private void SubscribeToEvents(GameEventICarrieAble[] events, Action<object, ICarrieAble> action)
        {
            foreach (GameEventICarrieAble gameEvent in events)
            {
                gameEvent.Subscribe(action);
            }
        }
        
        private void UnSubscribeFromEvents(GameEventICarrieAble[] events, Action<object, ICarrieAble> action)
        {
            foreach (GameEventICarrieAble gameEvent in events)
            {
                gameEvent.Unsubscribe(action);
            }
        }
        
        private void OnPayloadPickedUp(object sender, ICarrieAble _)
        {
            _isCarrying = true;
        }
        private void OnPayloadDropped(object sender, ICarrieAble _)
        {
            _isCarrying = false;
        }

        private void Update()
        {
            bool hasHit = HasHit();
            bool canClick = HasPointerHandler();
            if (canClick)
            {
                CurrentCursorState = CursorState.Open;
                return;
            }
            if(_isCarrying)
            {
                CurrentCursorState = CursorState.Closed;
                return;
            }
            
            if (hasHit)
            {
                CurrentCursorState = CursorState.Open;
                return;
            }
            CurrentCursorState = CursorState.Point;
        }

        private bool HasPointerHandler()
        {
            InputSystemUIInputModule input = EventSystem.current.currentInputModule as InputSystemUIInputModule;
            if(input == null)
                throw new Exception("InputSystemUIInputModule not found");
            RaycastResult result = input.GetLastRaycastResult(Mouse.current.deviceId);
            return ExecuteEvents.GetEventHandler<IPointerClickHandler>(result.gameObject) 
                                     || ExecuteEvents.GetEventHandler<IPointerDownHandler>(result.gameObject)
                                     || ExecuteEvents.GetEventHandler<IPointerUpHandler>(result.gameObject);
        }

        private bool HasHit()
        {
            bool isOverUI = ScreenToWorldHelper.IsMouseOverUI(out PointerEventData eventData);
            
            bool hitWithoutTrigger = Physics.Raycast(GetMouseToWorldRay(), out RaycastHit hit, float.MaxValue, layerMask, QueryTriggerInteraction.Ignore) &&
                   !isOverUI && (
                    hit.transform.TryGetComponent<ICarrieAble>(out ICarrieAble carry)
                    && CanCarry(carry)
                    );
            bool hitWithTrigger = Physics.Raycast(GetMouseToWorldRay(), out hit, float.MaxValue, layerMask, QueryTriggerInteraction.Collide)
                && !isOverUI
                && (
                    hit.transform.TryGetComponent(out Slot slot) && slot.HasPayload && !IsCustomerScene()
                    || hit.transform.TryGetComponent(out GameObjectButton _)
                    );
            
            return hitWithoutTrigger;
        }
        
        private bool CanCarry(ICarrieAble carryAble)
        {
            if (carryAble is Glass && IsCustomerScene())
                return false;
            return true;
        }

        private bool IsCustomerScene() => GameplaySceneManager.Instance?.CurrentScene?.Scene != GameplayScenes.Customer;

        private Ray GetMouseToWorldRay() => ScreenToWorldHelper.GetMouseToWorldRay(_mainCam);

        public void SetCursor(CursorState state)
        {
#if !UNITY_WEBGL
            UnityEngine.Cursor.SetCursor(CursorStateToTexture(state), hotSpot, CursorMode.Auto);
            return;
#endif
            UnityEngine.Cursor.SetCursor(CursorStateToTexture(state), hotSpot, CursorMode.ForceSoftware);
        }

        private Texture2D CursorStateToTexture(CursorState state)
        {
            return state switch
            {
                CursorState.Open => cursorOpen,
                CursorState.Closed => cursorClosed,
                CursorState.Point => cursorPoint,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}
