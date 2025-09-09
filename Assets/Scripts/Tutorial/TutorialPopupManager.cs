using System.Collections.Generic;
using JohaToolkit.UnityEngine.DataStructures;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tutorial
{
    public class TutorialPopupManager : MonoBehaviourSingleton<TutorialPopupManager>
    {
        [SerializeField, AssetsOnly] private BaseTutorialPopup defaultPopupPrefab;

        private Dictionary<int, BaseTutorialPopup> _activePopups = new();
        
        public void ShowPopup(TutorialText tutorialText) => ShowPopup(tutorialText, defaultPopupPrefab);

        public void ShowPopup(TutorialText tutorialText, BaseTutorialPopup popupPrefab)
        {
            if (_activePopups.ContainsKey(tutorialText.UniqueID))
            {
                Debug.LogWarning($"Popup already active for tutorial ID {tutorialText.UniqueID} and Header {tutorialText.HeaderText}");
                return;
            }
            
            BaseTutorialPopup instantiatedPopup = Instantiate(popupPrefab, transform);
            _activePopups.Add(tutorialText.UniqueID, instantiatedPopup);
            instantiatedPopup.Show(tutorialText);
        }
        
        public void HidePopup(TutorialText tutorialText)
        {
            if (!_activePopups.TryGetValue(tutorialText.UniqueID, out BaseTutorialPopup popup))
            {
                Debug.LogWarning($"No active popup found for tutorial ID {tutorialText.UniqueID} and Header {tutorialText.HeaderText}");
                return;
            }
            popup.Hide();
            _activePopups.Remove(tutorialText.UniqueID);
        }
    }
}