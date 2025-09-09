using UnityEngine;

namespace Tutorial
{
    public abstract class BaseTutorialPopup : MonoBehaviour
    {
        public abstract void Show(TutorialText tutorialText);
        public abstract void Hide();
    }
}