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
        
        public void ShowPopup(TutorialText tutorialText) => ShowPopup(tutorialText.UniqueID, tutorialText, defaultPopupPrefab);

        public void ShowPopup(int id, TutorialText tutorialText, BaseTutorialPopup popupPrefab)
        {
            if (_activePopups.ContainsKey(id))
            {
                Debug.LogWarning($"Popup already active for tutorial ID {id} and Header {tutorialText?.HeaderText}");
                return;
            }
            
            BaseTutorialPopup instantiatedPopup = Instantiate(popupPrefab, transform);
            _activePopups.Add(id, instantiatedPopup);
            instantiatedPopup.Show(tutorialText);
        }
        
        public void HidePopup(TutorialText tutorialText) => HidePopup(tutorialText.UniqueID);
        public void HidePopup(int id)
        {
            if (!_activePopups.TryGetValue(id, out BaseTutorialPopup popup))
            {
                Debug.LogWarning($"No active popup found for tutorial ID {id}");
                return;
            }
            popup.Hide();
            _activePopups.Remove(id);
        }
    }
}