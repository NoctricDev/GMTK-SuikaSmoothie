using Carry;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FruitBowlScene
{
    public class MouseDropShadow : MonoBehaviour
    {
        [Title("References")]
        [SerializeField, Required] private FruitBowlMouse fruitBowlMouse;

        private void Awake()
        {
            fruitBowlMouse.PayloadPickedUpEvent += OnPayLoadPickedUp;
            fruitBowlMouse.PayloadDroppedEvent += OnPayLoadDropped;
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnPayLoadDropped(ICarrieAble _) => gameObject.SetActive(false);

        private void OnPayLoadPickedUp(ICarrieAble _) => gameObject.SetActive(true);
    }
}