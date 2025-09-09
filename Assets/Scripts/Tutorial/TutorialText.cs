using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tutorial/TutorialText", fileName = "TutorialText", order = 0)]
    public class TutorialText : ScriptableObject
    {
        [SerializeField] private int uniqueID;
        [SerializeField] private string headerText;
        [SerializeField, TextArea] private string contentText;
        
        public int UniqueID => uniqueID;
        public string HeaderText => headerText;
        public string ContentText => contentText;
    }
}