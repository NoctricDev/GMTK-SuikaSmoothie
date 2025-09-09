using UnityEngine;

namespace Tutorial
{
    public abstract class TutorialStep : ScriptableObject
    {
        public abstract void StartStep();
        public abstract void OnEndStep();
    }
}