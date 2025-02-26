using Core.Buildings;
using Core.LobbyBase;
using Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class BaseBuildingTabGenerator : MonoBehaviour, IDisposable
    {
        [SerializeField] private BaseBuildingTab _tabPrefab;
        [SerializeField] private Transform _tabsRoot;
        [SerializeField] private ToggleGroup _toggleGroup;
        private Dictionary<BuildingType, BaseBuildingTab> _tabs = new Dictionary<BuildingType, BaseBuildingTab>();

        public void AddTab(BuildingType baseObjectType, Action<BuildingType, bool> tabStateChangedAction = null)
        {
            var tab = Instantiate<BaseBuildingTab>(_tabPrefab, _tabsRoot);
            tab.InitBuildingType(baseObjectType);
            tab.InitToggleGroup(_toggleGroup);
            tab.TabStateChangeEvent.AddListener(tabStateChangedAction);
            if (!_tabs.ContainsKey(baseObjectType))
            {
                _tabs.Add(baseObjectType ,tab);
            }
            else
            {
                throw new Exception($"[LobbyBasePage][BaseBuildingTabGenerator] Tab {baseObjectType} already exists.");
            }
        }

        public void ResetRootPosition()
        {
            var pos = _tabsRoot.localPosition;
            pos.x = 0f;
            _tabsRoot.localPosition = pos;
        }

        public void ToggleOnFirstTab()
        {
            var firstTab = _tabs.First().Value;
            if (!firstTab.IsOn)
            {
                firstTab.ToggleOn();
            }
        }

        public void ClearTabs()
        {
            foreach (var tab in _tabs.Values)
            {
                tab.TabStateChangeEvent.RemoveAllListeners();
                GameObject.Destroy(tab.gameObject);
            }

            _tabs.Clear();
        }

        public void Dispose()
        {
            ClearTabs();
        }
    }
}