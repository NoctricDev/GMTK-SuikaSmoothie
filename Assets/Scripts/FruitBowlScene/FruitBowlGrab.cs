using System;
using System.Threading;
using Carry;
using DG.Tweening;
using Events;
using Helper;
using Input;
using Scenes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FruitBowlScene
{
    public class FruitBowlGrab : MonoBehaviour, IGameplaySceneObject, ICarrieAbleMouse
    {
        [Title("References")] 
        [SerializeField, Required] private InputManagerSO inputManager;
        [SerializeField] private GameEventICarrieAble payloadPickedUpGameEvent;
        [SerializeField] private GameEventICarrieAble payloadDroppedGameEvent;
        [SerializeField] private Transform travelStartPoint;
        [SerializeField] private Transform travelEndPoint;

        private bool _isCarrying;
        private UnityEngine.Camera _mainCam;
        
        private ICarrieAble _activePayload;
        private GameObject _activePayloadGameObject;
        
        private CancellationTokenSource _cancellationTokenSource;
        
        public bool HasPayload => _activePayloadGameObject;
        
        public void LoadEnd()
        {
            _mainCam = UnityEngine.Camera.main;
            inputManager.InteractSecondaryEvent += OnInteractSecondary;
            payloadPickedUpGameEvent.Subscribe(OnPayLoadPickedUp);
            payloadDroppedGameEvent.Subscribe(OnPayLoadDropped);
        }

        public void Unload()
        {
            StopCarry();
            inputManager.InteractSecondaryEvent -= OnInteractSecondary;
            payloadPickedUpGameEvent.Unsubscribe(OnPayLoadPickedUp);
            payloadDroppedGameEvent.Unsubscribe(OnPayLoadDropped);
        }

        private void OnPayLoadPickedUp(object arg1, ICarrieAble arg2) => _isCarrying = true;
        private void OnPayLoadDropped(object arg1, ICarrieAble arg2) => _isCarrying = false;

        private void OnInteractSecondary()
        {
            Debug.Log("Testing");
            if (_isCarrying)
            {
                return;
            }
            
            if (ScreenToWorldHelper.IsMouseOverUI())
                return;
            
            Ray ray = ScreenToWorldHelper.GetMouseToWorldRay(_mainCam);
            ray.direction = ray.direction.normalized * 100;
            if (!Physics.Raycast(ray, out RaycastHit hit, 10, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                return;
            
            if (!hit.collider.TryGetComponent(out ICarrieAble carrieAble))
                return;

            
            if (_activePayload == carrieAble)
            {
                StopCarry();
                return;
            }
            
            StartCarry(carrieAble);
        }

        public void StartCarry(ICarrieAble carrieAble)
        {
            if (_activePayloadGameObject)
                return;

            if (!carrieAble.TryStartCarry(travelStartPoint, this))
                return;

            travelStartPoint.position = carrieAble.GetAttachedGameObject().transform.position;
            
            _activePayload = carrieAble;
            _activePayloadGameObject = carrieAble.GetAttachedGameObject();
            _cancellationTokenSource = new CancellationTokenSource();
            _ = WaitForPayloadToArrive();
        }

        public void StopCarry()
        {
            if (_activePayload == null || !_activePayloadGameObject)
                return;
            
            _cancellationTokenSource?.Cancel();
            _activePayload.OnStopCarry();
            _activePayload = null;
            _activePayloadGameObject = null;
        }

        private async Awaitable WaitForPayloadToArrive()
        {
            Vector3 startPos = travelStartPoint.position;
            float timer = 0;
            
            while (timer < 1)
            {
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                if (!_activePayloadGameObject)
                {
                    _activePayload = null;
                    return;
                }
                timer += Time.deltaTime / 0.5f;
                travelStartPoint.position = Vector3.Lerp(startPos, travelEndPoint.position, timer);
                await Awaitable.EndOfFrameAsync(_cancellationTokenSource.Token);
            }
        }
    }
}
