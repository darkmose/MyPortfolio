using Core.MVP;
using Core.PlayerModule;
using Core.Weapon;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Core.GameLogic
{
    public class PlayerWeaponUpgradeSystem : IDisposable
    {
        private PlayerWeaponConfigurationProvider _configurationProvider;
        private List<PlayerWeaponUpgradePanelDescriptor> _descriptors;
        private PlayerWeaponUpgradeSystemView _upgradeSystemView;
        private IWeaponDecoratorsHolder _weaponDecoratorsDataHolder;
        private Dictionary<PlayerWeaponType, UpgradePlayerWeaponDecorator> _weaponDecorators;
        private Dictionary<PlayerExtraWeaponType, UpgradePlayerWeaponDecorator> _extraWeaponDecorators;
        private IWallet _wallet;

        public PlayerWeaponUpgradeSystem(IWeaponDecoratorsHolder weaponDecoratorsDataHolder, IWallet wallet)
        {
            _configurationProvider = Resources.Load<PlayerWeaponConfigurationProvider>("ScriptableObjects/WeaponUpgradeConfigProvider");
            _descriptors = new List<PlayerWeaponUpgradePanelDescriptor>();
            _weaponDecoratorsDataHolder = weaponDecoratorsDataHolder;
            _weaponDecorators = new Dictionary<PlayerWeaponType, UpgradePlayerWeaponDecorator>();
            _extraWeaponDecorators = new Dictionary<PlayerExtraWeaponType, UpgradePlayerWeaponDecorator>();
            _wallet = wallet;
        }

        public void InitWeaponUpgradeSystemView(PlayerWeaponUpgradeSystemView upgradeSystemView)
        {
            _descriptors.Clear();
            _descriptors = new List<PlayerWeaponUpgradePanelDescriptor>();
            _upgradeSystemView = upgradeSystemView;
            var allWeapons = Enum.GetNames(typeof(PlayerWeaponType));
            for (int i = 0; i < allWeapons.Length; i++)
            {
                var weapon = (PlayerWeaponType)i;
                var initConfig = _configurationProvider.ProvideWeaponInitConfig(weapon);
                var panelDescriptor = new PlayerWeaponUpgradePanelDescriptor(weapon, initConfig);
                panelDescriptor.OnHardCurrencyButtonEvent.AddListener(OnHardCurrencyButtonClick);
                panelDescriptor.OnSoftCurrencyButtonEvent.AddListener(OnSoftCurrencyButtonClick);
                _descriptors.Add(panelDescriptor);
                var initDecorator = new InitPlayerWeaponDecorator(initConfig);
                var upgradeDecorator = new UpgradePlayerWeaponDecorator(initDecorator);
                var currentUpgradeLevel = _weaponDecoratorsDataHolder.GetPlayerWeaponLevel(weapon);
                if (currentUpgradeLevel > 1)
                {
                    var upgradeDescriptor = _configurationProvider.ProvideWeaponUpgradeDescriptor(weapon, currentUpgradeLevel);
                    upgradeDecorator.SetStats(upgradeDescriptor.AdditionalStats);
                }

                _weaponDecorators.Add(weapon, upgradeDecorator);
            }

            var allExtraWeapons = Enum.GetNames(typeof(PlayerExtraWeaponType));
            for (int i = 0; i < allExtraWeapons.Length; i++)
            {
                var extraWeapon = (PlayerExtraWeaponType)i;
                var initConfig = _configurationProvider.ProvideExtraWeaponInitConfig(extraWeapon);
                var panelDescriptor = new PlayerWeaponUpgradePanelDescriptor(extraWeapon, initConfig);
                panelDescriptor.OnHardCurrencyButtonEvent.AddListener(OnHardCurrencyButtonClick);
                panelDescriptor.OnSoftCurrencyButtonEvent.AddListener(OnSoftCurrencyButtonClick);
                _descriptors.Add(panelDescriptor);
                var initDecorator = new InitPlayerWeaponDecorator(initConfig);
                var upgradeDecorator = new UpgradePlayerWeaponDecorator(initDecorator);
                var currentUpgradeLevel = _weaponDecoratorsDataHolder.GetPlayerExtraWeaponLevel(extraWeapon);
                if (currentUpgradeLevel > 1)
                {
                    var upgradeDescriptor = _configurationProvider.ProvideExtraWeaponUpgradeDescriptor(extraWeapon, currentUpgradeLevel);
                    upgradeDecorator.SetStats(upgradeDescriptor.AdditionalStats);
                }

                _extraWeaponDecorators.Add(extraWeapon, upgradeDecorator);
            }

            upgradeSystemView.InitSystemView(_descriptors);
            RefreshPanels();
        }

        private void RefreshPanel(PlayerWeaponUpgradePanelDescriptor panel)
        {
            if (panel.IsExtraWeapon)
            {
                var upgradeLevel = _weaponDecoratorsDataHolder.GetPlayerExtraWeaponLevel(panel.ExtraWeaponType);
                var nextLevel = upgradeLevel + 1;
                var hasNextUpgrade = _configurationProvider.HasExtraWeaponUpgradeDescriptor(panel.ExtraWeaponType, nextLevel);

                if (hasNextUpgrade)
                {
                    var upgradeDescriptor = _configurationProvider.ProvideExtraWeaponUpgradeDescriptor(panel.ExtraWeaponType, nextLevel);
                    var softCost = upgradeDescriptor.SoftCost;
                    var hardCost = upgradeDescriptor.HardCost;
                    panel.SetSoftCurrency(softCost.MoneyType);
                    panel.SetSoftCost(softCost.Cost);
                    panel.SetHardCurrency(hardCost.MoneyType);
                    panel.SetHardCost(hardCost.Cost);

                    panel.SetSoftCurrencyAvailable(_wallet.HasEnoughMoney(softCost.MoneyType, softCost.Cost));
                    panel.SetHardCurrencyAvailable(_wallet.HasEnoughMoney(hardCost.MoneyType, hardCost.Cost));
                }
                else
                {
                    panel.SetSoftCost(-1);
                    panel.SetHardCost(-1);
                    panel.SetSoftCurrencyAvailable(false);
                    panel.SetHardCurrencyAvailable(false);
                }
                panel.SetWeaponRank(upgradeLevel);
                var requiredLevel = _configurationProvider.GetExtraWeaponRequiredLevel(panel.ExtraWeaponType);
                panel.SetWeaponRequiredLevel(requiredLevel);

                if (_extraWeaponDecorators.TryGetValue(panel.ExtraWeaponType, out var decorator))
                {
                    var config = decorator.GetWeaponConfig();
                    panel.SetDamage(config.Damage);
                    panel.SetProjectilesCount(config.ProjectileCount);
                    panel.SetProjectilesSpeed(config.Speed);
                    panel.SetReloadSpeed(config.ReloadSpeed);
                }
            }
            else
            {
                var upgradeLevel = _weaponDecoratorsDataHolder.GetPlayerWeaponLevel(panel.WeaponType);
                var nextLevel = upgradeLevel + 1;
                var hasNextUpgrade = _configurationProvider.HasWeaponUpgradeDescriptor(panel.WeaponType, nextLevel);

                if (hasNextUpgrade)
                {
                    var upgradeDescriptor = _configurationProvider.ProvideWeaponUpgradeDescriptor(panel.WeaponType, nextLevel);
                    var softCost = upgradeDescriptor.SoftCost;
                    var hardCost = upgradeDescriptor.HardCost;
                    panel.SetSoftCurrency(softCost.MoneyType);
                    panel.SetSoftCost(softCost.Cost);
                    panel.SetHardCurrency(hardCost.MoneyType);
                    panel.SetHardCost(hardCost.Cost);

                    panel.SetSoftCurrencyAvailable(_wallet.HasEnoughMoney(softCost.MoneyType, softCost.Cost));
                    panel.SetHardCurrencyAvailable(_wallet.HasEnoughMoney(hardCost.MoneyType, hardCost.Cost));
                }
                else
                {
                    panel.SetSoftCost(-1);
                    panel.SetHardCost(-1);
                    panel.SetSoftCurrencyAvailable(false);
                    panel.SetHardCurrencyAvailable(false);
                }
                panel.SetWeaponRank(upgradeLevel);
                var requiredLevel = _configurationProvider.GetWeaponRequiredLevel(panel.WeaponType);
                panel.SetWeaponRequiredLevel(requiredLevel);
                if (_weaponDecorators.TryGetValue(panel.WeaponType, out var decorator))
                {
                    var config = decorator.GetWeaponConfig();
                    panel.SetDamage(config.Damage);
                    panel.SetProjectilesCount(config.ProjectileCount);
                    panel.SetProjectilesSpeed(config.Speed);
                    panel.SetReloadSpeed(config.ReloadSpeed);
                }
            }
        }

        private void RefreshPanels()
        {
            foreach (var panel in _descriptors)
            {
                RefreshPanel(panel);
            }
        }

        public void InitWeaponDecorator(PlayerWeapon playerWeapon)
        {
            if (_weaponDecorators.TryGetValue(playerWeapon.PlayerWeaponType, out var decorator))
            {
                playerWeapon.InitWeaponConfigDecorator(decorator);
            }
        }

        public void InitExtraWeaponDecorator(PlayerExtraWeapon playerExtraWeapon)
        {
            if (_extraWeaponDecorators.TryGetValue(playerExtraWeapon.PlayerWeaponType, out var decorator))
            {
                playerExtraWeapon.InitWeaponConfigDecorator(decorator);
            }
        }

        private void OnSoftCurrencyButtonClick(PlayerWeaponUpgradePanelDescriptor descriptor)
        {
            if (_wallet.TryToSpend(descriptor.SoftCurrency.Value, descriptor.SoftCost.Value))
            {
                Upgrade(descriptor);
            }
        }

        private void Upgrade(PlayerWeaponUpgradePanelDescriptor descriptor)
        {
            if (descriptor.IsExtraWeapon)
            {
                var currentUpgradeLevel = _weaponDecoratorsDataHolder.GetPlayerExtraWeaponLevel(descriptor.ExtraWeaponType);
                var nextLevel = currentUpgradeLevel + 1;
                var upgradeDescriptor = _configurationProvider.ProvideExtraWeaponUpgradeDescriptor(descriptor.ExtraWeaponType, nextLevel);
                _weaponDecoratorsDataHolder.SetPlayerExtraWeaponLevel(descriptor.ExtraWeaponType, nextLevel);
                if (_extraWeaponDecorators.TryGetValue(descriptor.ExtraWeaponType, out var decorator))
                {
                    var newAddStats = upgradeDescriptor.AdditionalStats;
                    decorator.SetStats(newAddStats);
                }
            }
            else
            {
                var currentUpgradeLevel = _weaponDecoratorsDataHolder.GetPlayerWeaponLevel(descriptor.WeaponType);
                var nextLevel = currentUpgradeLevel + 1;
                var upgradeDescriptor = _configurationProvider.ProvideWeaponUpgradeDescriptor(descriptor.WeaponType, nextLevel);
                _weaponDecoratorsDataHolder.SetPlayerWeaponLevel(descriptor.WeaponType, nextLevel);
                if (_weaponDecorators.TryGetValue(descriptor.WeaponType, out var decorator))
                {
                    var newAddStats = upgradeDescriptor.AdditionalStats;
                    decorator.SetStats(newAddStats);
                }
            }

            RefreshPanel(descriptor);
            //RefreshPanels();
        }

        private void OnHardCurrencyButtonClick(PlayerWeaponUpgradePanelDescriptor descriptor)
        {
            if (_wallet.TryToSpend(descriptor.HardCurrency.Value, descriptor.HardCost.Value))
            {
                Upgrade(descriptor);
            }
        }

        public void Dispose()
        {
            foreach (var panelDescriptor in _descriptors)
            {
                panelDescriptor.OnHardCurrencyButtonEvent.RemoveAllListeners();
                panelDescriptor.OnSoftCurrencyButtonEvent.RemoveAllListeners();
                panelDescriptor.DamageStat.RemoveAllListeners();
                panelDescriptor.ProjectilesCountStat.RemoveAllListeners();
                panelDescriptor.ProjectileSpeedStat.RemoveAllListeners();
                panelDescriptor.ReloadSpeedStat.RemoveAllListeners();
                panelDescriptor.HardCost.RemoveAllListeners();
                panelDescriptor.SoftCost.RemoveAllListeners();
                panelDescriptor.SoftCurrency.RemoveAllListeners();
                panelDescriptor.HardCurrency.RemoveAllListeners();
                panelDescriptor.SoftCurrencyButtonAvailable.RemoveAllListeners();
                panelDescriptor.HardCurrencyButtonAvailable.RemoveAllListeners();
                panelDescriptor.WeaponRank.RemoveAllListeners();
                panelDescriptor.WeaponRequiredLevel.RemoveAllListeners();
            }

            _upgradeSystemView.Dispose();
        }
    }
}