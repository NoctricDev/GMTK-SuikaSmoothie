using System;
using System.Collections.Generic;
using Input;
using JohaToolkit.UnityEngine.DataStructures;
using UnityEngine;

namespace UI
{
    public interface IInitOptions
    {
        public void Init();
    }
    public class OptionsScreenUI : MonoBehaviourSingleton<OptionsScreenUI>
    {
        private List<InputManagerSO.ActionMaps> _enabledActionMapsCache;
        private bool _isShown = false;
        public event Action<bool> VisibilityChangedEvent;

        protected override void Awake()
        {
            base.Awake();

            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out IInitOptions initOptions))
                {
                    initOptions.Init();
                }
            }
            
            gameObject.SetActive(false);
        }

        public void Show()
        {
            if (_isShown)
                return;
            CacheAndDisableEnabledActionMaps();
            _isShown = true;
            gameObject.SetActive(true);
            VisibilityChangedEvent?.Invoke(true);
        }


        public void Hide()
        {
            if (!_isShown)
                return;
            EnableCachedActionMaps();
            _isShown = false;
            gameObject.SetActive(false);
            VisibilityChangedEvent?.Invoke(false);
        }
        
        private void CacheAndDisableEnabledActionMaps()
        {
            _enabledActionMapsCache ??= new List<InputManagerSO.ActionMaps>();
            _enabledActionMapsCache.Clear();
            for (int i = _enabledActionMapsCache.Count - 1; i >= 0; i--)
            {
                InputManagerSO.ActionMaps actionMap = _enabledActionMapsCache[i];
                InputManagerSO.Instance.DisableActionMap(actionMap);
                _enabledActionMapsCache.Add(actionMap);
            }
        }

        private void EnableCachedActionMaps()
        {
            if(_enabledActionMapsCache is null or {Count: 0})
                return;
            foreach (InputManagerSO.ActionMaps actionMap in _enabledActionMapsCache)
            {
                InputManagerSO.Instance.EnableActionMap(actionMap);
            }
        }
    }
}
