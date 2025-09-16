using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/TutorialSteps/HideCustomPopup", fileName = "HideCustomPopup_TutorialStep", order = 0)]
    public class HideCustomPopup_TutorialStep : TutorialStep
    {
        [SerializeField] private int id;
        public override void StartStep()
        {
            TutorialPopupManager.Instance.HidePopup(id);
            TutorialManager.Instance.NextStep();
        }

        public override void OnEndStep() { }
    }
}