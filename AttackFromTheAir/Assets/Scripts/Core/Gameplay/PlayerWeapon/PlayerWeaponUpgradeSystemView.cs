using Core.Resourses;
using Core.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameLogic
{
    public class PlayerWeaponUpgradeSystemView : MonoBehaviour, IDisposable
    {
        [SerializeField] private WeaponUpgradePanelView _upgradePanelPrefab;
        [SerializeField] private RectTransform _weaponsRoot;
        [SerializeField] private PlayerWeaponSpriteProvider _weaponSpriteProvider;
        [SerializeField] private WeaponStatsSpriteProvider _weaponStatsSpriteProvider;
        private List<WeaponUpgradePanelView> _upgradePanelViews = new List<WeaponUpgradePanelView>();

        private void Clear()
        {
            _weaponsRoot.ClearAllChild();
        }

        public void InitSystemView(List<PlayerWeaponUpgradePanelDescriptor> upgradePanels)
        {
            Clear();

            foreach (var panel in upgradePanels)
            {
                var panelViewInstance = Instantiate(_upgradePanelPrefab, _weaponsRoot, false);
                Sprite weaponSprite = null;
                if (panel.IsExtraWeapon)
                {
                    weaponSprite = _weaponSpriteProvider.ProvideByType(panel.ExtraWeaponType);
                }
                else
                {
                    weaponSprite = _weaponSpriteProvider.ProvideByType(panel.WeaponType);
                }

                panelViewInstance.InitWeaponSprite(weaponSprite);
                panelViewInstance.SetDamageStatIcon(_weaponStatsSpriteProvider.ProvideSprite(PlayerWeaponStats.Damage));
                panelViewInstance.SetReloadSpeedStatIcon(_weaponStatsSpriteProvider.ProvideSprite(PlayerWeaponStats.ReloadSpeed));
                panelViewInstance.SetProjectileCountStatIcon(_weaponStatsSpriteProvider.ProvideSprite(PlayerWeaponStats.ProjectilesCount));
                panelViewInstance.SetProjectileSpeedStatIcon(_weaponStatsSpriteProvider.ProvideSprite(PlayerWeaponStats.ProjectileSpeed));


                panel.DamageStat.RegisterValueChangeListener(panelViewInstance.SetDamageStat);
                panel.ProjectilesCountStat.RegisterValueChangeListener(panelViewInstance.SetProjectileCountStat);
                panel.ProjectileSpeedStat.RegisterValueChangeListener(panelViewInstance.SetProjectileSpeedStat);
                panel.ReloadSpeedStat.RegisterValueChangeListener(panelViewInstance.SetReloadSpeedStat);

                panel.SoftCurrency.RegisterValueChangeListener(panelViewInstance.SetSoftCurrency);
                panel.HardCurrency.RegisterValueChangeListener(panelViewInstance.SetHardCurrency);

                panel.SoftCost.RegisterValueChangeListener(panelViewInstance.SetSoftCurrencyCost);
                panel.HardCost.RegisterValueChangeListener(panelViewInstance.SetHardCurrencyCost);
                panel.SoftCurrencyButtonAvailable.RegisterValueChangeListener(panelViewInstance.SetSoftCurrencyAvailable);
                panel.HardCurrencyButtonAvailable.RegisterValueChangeListener(panelViewInstance.SetHardCurrencyAvailable);
                panel.WeaponRank.RegisterValueChangeListener(panelViewInstance.SetWeaponRank);
                panel.WeaponRequiredLevel.RegisterValueChangeListener(panelViewInstance.SetWeaponRequiredLevel);

                panelViewInstance.InitCallback(panel.OnSoftCurrencyBuyButtonClick, panel.OnHardCurrencyBuyButtonClick);

                panelViewInstance.SetDamageStat(panel.DamageStat.Value);
                panelViewInstance.SetProjectileCountStat(panel.ProjectilesCountStat.Value);
                panelViewInstance.SetProjectileSpeedStat(panel.ProjectileSpeedStat.Value);
                panelViewInstance.SetReloadSpeedStat(panel.ReloadSpeedStat.Value);

                panelViewInstance.SetSoftCurrency(panel.SoftCurrency.Value);
                panelViewInstance.SetHardCurrency(panel.HardCurrency.Value);

                panelViewInstance.SetSoftCurrencyCost(panel.SoftCost.Value);
                panelViewInstance.SetHardCurrencyCost(panel.HardCost.Value);
                panelViewInstance.SetSoftCurrencyAvailable(panel.SoftCurrencyButtonAvailable.Value);
                panelViewInstance.SetHardCurrencyAvailable(panel.HardCurrencyButtonAvailable.Value);
                panelViewInstance.SetWeaponRank(panel.WeaponRank.Value);
                panelViewInstance.SetWeaponRequiredLevel(panel.WeaponRequiredLevel.Value);
            }
        }

        public void Dispose()
        {
            Clear();
        }
    }
}