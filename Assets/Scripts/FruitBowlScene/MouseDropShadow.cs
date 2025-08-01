using Carry;
using Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FruitBowlScene
{
    public class MouseDropShadow : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private GameEventICarrieAble payloadPickedUpGameEvent;
        [SerializeField] private GameEventICarrieAble payloadDroppedGameEvent;

        private void Awake()
        {
            payloadPickedUpGameEvent.Subscribe(OnPayLoadPickedUp);
            payloadDroppedGameEvent.Subscribe(OnPayLoadDropped);
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnPayLoadDropped(object sender, ICarrieAble _) => gameObject.SetActive(false);

        private void OnPayLoadPickedUp(object sender, ICarrieAble _) => gameObject.SetActive(true);
    }
}