using System;
using System.Threading;
using Carry;
using DG.Tweening;
using Events;
using Helper;
using Input;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FruitBowlScene
{
    public class FruitBowlGrab : MonoBehaviour
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
        
        private CancellationTokenSource _cancellationTokenSource;
        
        private void OnEnable()
        {
            _mainCam = UnityEngine.Camera.main;
            inputManager.InteractSecondaryEvent += OnInteractSecondary;
            payloadPickedUpGameEvent.Subscribe(OnPayLoadPickedUp);
            payloadDroppedGameEvent.Subscribe(OnPayLoadDropped);
        }

        private void OnDisable()
        {
            inputManager.InteractSecondaryEvent -= OnInteractSecondary;
            payloadPickedUpGameEvent.Unsubscribe(OnPayLoadPickedUp);
            payloadDroppedGameEvent.Unsubscribe(OnPayLoadDropped);
        }

        private void OnPayLoadPickedUp(object arg1, ICarrieAble arg2) => _isCarrying = true;
        private void OnPayLoadDropped(object arg1, ICarrieAble arg2) => _isCarrying = false;

        private void OnInteractSecondary()
        {
            if (_isCarrying)
            {
                return;
            }
            
            if (ScreenToWorldHelper.IsMouseOverUI())
                return;
            
            // Drop if has active Payload
            
            Ray ray = ScreenToWorldHelper.GetMouseToWorldRay(_mainCam);
            ray.direction = ray.direction.normalized * 100;
            if (!Physics.Raycast(ray, out RaycastHit hit, 10, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                return;
            
            if (!hit.collider.TryGetComponent(out ICarrieAble carrieAble))
                return;

            if (!carrieAble.TryStartCarry(travelStartPoint, out ICarrieAble activePayload))
                return;

            travelStartPoint.position = activePayload.GetPosition();
            
            _activePayload = carrieAble;

            _ = WaitForPayloadToArrive(_activePayload);

        }
        
        private async Awaitable WaitForPayloadToArrive(ICarrieAble payload)
        {
            Vector3 startPos = travelStartPoint.position;
            float timer = 0;
            
            while (timer < 1)
            {
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                timer += Time.deltaTime / 0.5f;
                travelStartPoint.position = Vector3.Lerp(startPos, travelEndPoint.position, timer);
                await Awaitable.EndOfFrameAsync(_cancellationTokenSource.Token);
            }
        }
    }
}
