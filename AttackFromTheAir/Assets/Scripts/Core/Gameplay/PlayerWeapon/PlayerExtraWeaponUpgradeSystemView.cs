using Core.Resourses;
using Core.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameLogic
{
    public class PlayerExtraWeaponUpgradeSystemView : MonoBehaviour, IDisposable
    {
        [SerializeField] private ExtraWeaponPanelView _upgradePanelPrefab;
        [SerializeField] private RectTransform _weaponsRoot;
        [SerializeField] private PlayerWeaponSpriteProvider _weaponSpriteProvider;
        [SerializeField] private WeaponStatsSpriteProvider _weaponStatsSpriteProvider;
        private List<ExtraWeaponPanelView> _upgradePanelViews = new List<ExtraWeaponPanelView>();

        private void Clear()
        {
            _weaponsRoot.ClearAllChild();
        }

        private void OnEnable()
        {
            _weaponsRoot.anchoredPosition = Vector2.zero;
        }

        public void InitSystemView(List<PlayerExtraWeaponUpgradePanelDescriptor> upgradePanels)
        {
            Clear();

            foreach (var panel in upgradePanels)
            {
                var panelViewInstance = Instantiate(_upgradePanelPrefab, _weaponsRoot, false);
                Sprite weaponSprite = null;
                weaponSprite = _weaponSpriteProvider.ProvideByType(panel.WeaponType);
                panelViewInstance.InitStats(panel);
                _upgradePanelViews.Add(panelViewInstance);
            }
        }

        public void Dispose()
        {
            foreach (var panelView in _upgradePanelViews)
            {
                panelView.Dispose();
            }
            Clear();
        }
    }
}