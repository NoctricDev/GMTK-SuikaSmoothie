using System;
using Carry;
using Events;
using Glasses;
using Helper;
using JohaToolkit.UnityEngine.DataStructures;
using JohaToolkit.UnityEngine.Extensions;
using Scenes;
using Sirenix.OdinInspector;
using UnityEngine;

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

        private void OnEnable()
        {
            SubscribeToEvents(payloadPickedUpEvent, OnPayloadPickedUp);
            SubscribeToEvents(payloadDroppedEvent, OnPayloadDropped);
        }

        private void OnDisable()
        {
            UnSubscribeFromEvents(payloadPickedUpEvent, OnPayloadPickedUp);
            UnSubscribeFromEvents(payloadDroppedEvent, OnPayloadDropped);
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

        private bool HasHit()
        {
            bool hitCarryAble = Physics.Raycast(GetMouseToWorldRay(), out RaycastHit hit, float.MaxValue, layerMask, QueryTriggerInteraction.Ignore) &&
                   !ScreenToWorldHelper.IsMouseOverUI() && (
                    hit.transform.TryGetComponent<ICarrieAble>(out ICarrieAble carry)
                    && CanCarry(carry)
                    );
            bool hitSlot = Physics.Raycast(GetMouseToWorldRay(), out hit, float.MaxValue, layerMask, QueryTriggerInteraction.Collide)
                && !ScreenToWorldHelper.IsMouseOverUI() 
                && (hit.transform.TryGetComponent<Slot>(out Slot slot) && slot.HasPayload && !IsCustomerScene());
            
            Debug.Log(hit.transform.gameObject.name);
            
            return hitCarryAble || hitSlot;
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
            UnityEngine.Cursor.SetCursor(CursorStateToTexture(state), hotSpot, CursorMode.Auto);
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
