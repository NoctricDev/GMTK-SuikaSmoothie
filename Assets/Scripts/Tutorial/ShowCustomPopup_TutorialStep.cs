using Sirenix.OdinInspector;
using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/TutorialSteps/ShowCustomPopup", fileName = "ShowCustomPopup_TutorialStep", order = 0)]
    public class ShowCustomPopup_TutorialStep : TutorialStep
    {
        [SerializeField] private int UniqueID;
        [AssetSelector, SerializeField] BaseTutorialPopup popupPrefab;

        private TutorialPopup _popupInstance;
        
        public override void StartStep()
        {
            TutorialPopupManager.Instance.ShowPopup(UniqueID, null, popupPrefab);
            TutorialManager.Instance.NextStep();
        }

        public override void OnEndStep() { }
    }
}