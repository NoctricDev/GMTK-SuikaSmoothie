using Carry;
using DG.Tweening;
using Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Camera
{
    public class AdjustCameratarget : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private Transform thisTransform;
        [SerializeField] private Transform posA;
        [SerializeField] private Transform posB;

        [SerializeField] private GameEventICarrieAble gameEventA;
        [SerializeField] private GameEventICarrieAble gameEventB;

        [Title("Settings")]
        [SerializeField] private bool AIsStartPosition = true;
        [SerializeField] private float duration = 0.5f;
        
        private void Start()
        {
            gameEventA.Subscribe(GameEventATriggered);
            gameEventB.Subscribe(GameEventBTriggered);
            thisTransform.position = AIsStartPosition ? posA.position : posB.position;
        }

        private void GameEventATriggered(object sender, ICarrieAble _)
        {
            thisTransform.DOKill();
            thisTransform.DOMove(posA.position, duration);
        }

        private void GameEventBTriggered(object sender, ICarrieAble _)
        {
            thisTransform.DOKill();
            thisTransform.DOMove(posB.position, duration);
        }
    }
}
