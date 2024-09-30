using Core.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class LobbyTabToggles : MonoBehaviour
    {
        [SerializeField] private ToggleGroup _toggleGroup;
        [SerializeField] private List<LobbyToggle> _toggles;
        private Dictionary<LobbyTabs, LobbyToggle> _tabTogglesDictionary;
        private LobbyTabs _currentTab = LobbyTabs.LevelStart;
        private bool _isLevelStartToggle;
        public LobbyTabs CurrentTab => _currentTab;
        public LobbyTabs LastTab { get; private set; } = LobbyTabs.LevelStart;
        public event Action<LobbyTabs> OnLobbyTabChangedEvent;

        private void Start()
        {
            InitToggles();
            SwitchTab(LobbyTabs.LevelStart);
        }

        private void InitToggles()
        {
            _tabTogglesDictionary = new Dictionary<LobbyTabs, LobbyToggle>();
            foreach (var toggle in _toggles)
            {
                _tabTogglesDictionary.Add(toggle.LobbyTab, toggle);
                toggle.OnToggledEvent += OnToggleSwitchedHandler;
                toggle.Toggle.group = _toggleGroup;
                toggle.Init();
            }
        }

        private void OnToggleSwitchedHandler(LobbyTabs tab, bool isToggled)
        {
            if (isToggled)
            {
                if (tab == LobbyTabs.LevelStart)
                {
                    if (_isLevelStartToggle)
                    {
                        EventAggregator.Post(this, new LevelStartRequestEvent());
                        return;
                    }
                    _isLevelStartToggle = true;
                }
                else
                {
                    _isLevelStartToggle = false;
                }
                LastTab = _currentTab;
                _currentTab = tab;
                OnLobbyTabChangedEvent?.Invoke(tab);
            }
        }

        public void SwitchTab(LobbyTabs tab, bool sendCallback = true)
        {
            if (_tabTogglesDictionary.TryGetValue(tab, out var toggle))
            {
                toggle.Toggle.isOn = true;
            }
        }

        private void OnDestroy()
        {
            foreach (var toggle in _toggles)
            {
                toggle.OnToggledEvent -= OnToggleSwitchedHandler;
            }
        }

    }
}