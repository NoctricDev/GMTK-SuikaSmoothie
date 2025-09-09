using System;
using UnityEngine;

namespace Tutorial
{
    
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/TutorialSteps/HidePopup", fileName = "HidePopup_TutorialStep", order = 0)]
    public class HidePopup_TutorialStep : TutorialStep
    {
        [SerializeField] private TutorialText popupText;
        public override void StartStep()
        {
            TutorialPopupManager.Instance.HidePopup(popupText);
            TutorialManager.Instance.NextStep();
        }

        public override void OnEndStep() { }
    }
}