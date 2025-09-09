using System;
using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/Tutorial Steps", fileName = "TutorialSteps", order = 0)]
    public class TutorialSteps : ScriptableObject
    {
        [SerializeField] private TutorialStep[] tutorialSteps;
        public TutorialStep[] Steps => tutorialSteps;
    }
}