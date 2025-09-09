using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/TutorialSteps/ShowPopup", fileName = "ShowPopup_TutorialStep", order = 0)]
    public class ShowPopup_TutorialStep : TutorialStep
    {
        [SerializeField] private TutorialText popupText;

        private TutorialPopup _popupInstance;
        
        public override void StartStep()
        {
            TutorialPopupManager.Instance.ShowPopup(popupText);
            TutorialManager.Instance.NextStep();
        }

        public override void OnEndStep() { }
    }
}