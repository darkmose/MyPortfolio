using Core.PlayerModule;
using Core.Resourses;
using Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Core.GameLogic
{
    public class PlayerExtraWeaponUpgradeSystem : IDisposable
    {
        private PlayerWeaponConfigurationProvider _configurationProvider;
        private List<PlayerExtraWeaponUpgradePanelDescriptor> _descriptors;
        private PlayerExtraWeaponUpgradeSystemView _upgradeSystemView;
        private IWeaponDecoratorsHolder _weaponDecoratorsDataHolder;
        private IExtraWeaponStatsSaver _extraWeaponStatsSaver;
        private Dictionary<PlayerExtraWeaponType, UpgradePlayerWeaponDecorator> _extraWeaponDecorators;
        private Dictionary<PlayerExtraWeaponStatUpgradeDescriptor, PlayerExtraWeaponType> _extraWeaponStatsDict;
        private IWallet _wallet;

        public PlayerExtraWeaponUpgradeSystem(IWeaponDecoratorsHolder weaponDecoratorsDataHolder, IWallet wallet, IExtraWeaponStatsSaver extraWeaponStatsSaver)
        {
            _configurationProvider = Resources.Load<PlayerWeaponConfigurationProvider>("ScriptableObjects/WeaponUpgradeConfigProvider");
            _descriptors = new List<PlayerExtraWeaponUpgradePanelDescriptor>();
            _weaponDecoratorsDataHolder = weaponDecoratorsDataHolder;
            _extraWeaponDecorators = new Dictionary<PlayerExtraWeaponType, UpgradePlayerWeaponDecorator>();
            _extraWeaponStatsDict = new Dictionary<PlayerExtraWeaponStatUpgradeDescriptor, PlayerExtraWeaponType>();
            _wallet = wallet;
            _extraWeaponStatsSaver = extraWeaponStatsSaver;
        }

        public void InitWeaponUpgradeSystemView(PlayerExtraWeaponUpgradeSystemView upgradeSystemView)
        {
            _descriptors.Clear();
            _descriptors = new List<PlayerExtraWeaponUpgradePanelDescriptor>();
            _upgradeSystemView = upgradeSystemView;

            var allExtraWeapons = Enum.GetNames(typeof(PlayerExtraWeaponType));
            for (int i = 0; i < allExtraWeapons.Length - 1; i++)
            {
                var extraWeapon = (PlayerExtraWeaponType)i;
                var initConfig = _configurationProvider.ProvideExtraWeaponInitConfig(extraWeapon);
                var panelDescriptor = new PlayerExtraWeaponUpgradePanelDescriptor(extraWeapon, initConfig);
                var allStats = panelDescriptor.AllStats;

                _descriptors.Add(panelDescriptor);
                var initDecorator = new InitPlayerWeaponDecorator(initConfig);
                var upgradeDecorator = new UpgradePlayerWeaponDecorator(initDecorator);
                var initialStats = _configurationProvider.ProvideExtraWeaponInitConfig(extraWeapon);
                var upgradeProgression = _configurationProvider.ProvideExtraWeaponUpgradeProgression(extraWeapon);

                foreach (var stat in allStats)
                {
                    _extraWeaponStatsDict.Add(stat, extraWeapon);
                    stat.OnAdvertisementButtonEvent.AddListener(OnAdvButtonClick);
                    stat.OnSoftCurrencyButtonEvent.AddListener(OnSoftCurrencyButtonClick);
                    stat.OnHardCurrencyButtonEvent.AddListener(OnHardCurrencyButtonClick);

                    var statLevel = _extraWeaponStatsSaver.GetStatLevel(extraWeapon ,stat.PlayerWeaponStats);
                    if (statLevel > 0)
                    {
                        var statValue = upgradeProgression.GetPlayerWeaponAdditionalStat(stat.PlayerWeaponStats, statLevel);
                        upgradeDecorator.SetStat(stat.PlayerWeaponStats, statValue);
                    }

                    stat.SetStatLevel(statLevel);
                }

                _extraWeaponDecorators.Add(extraWeapon, upgradeDecorator);
            }

            RefreshPanels();
            upgradeSystemView.InitSystemView(_descriptors);
        }

        private void OnAdvButtonClick(PlayerExtraWeaponStatUpgradeDescriptor descriptor)
        {
            //TODO MAKE ADV
            UpgradeStat(descriptor);
        }

        private void RefreshPanel(PlayerExtraWeaponUpgradePanelDescriptor panel)
        {
            var upgradeLevel = _weaponDecoratorsDataHolder.GetPlayerExtraWeaponLevel(panel.WeaponType);

            RefreshStatsUpgradeCost(panel);

            panel.SetWeaponRank(upgradeLevel);
            var requiredLevel = _configurationProvider.GetExtraWeaponRequiredLevel(panel.WeaponType);
            panel.SetWeaponRequiredLevel(requiredLevel);

            if (_extraWeaponDecorators.TryGetValue(panel.WeaponType, out var decorator))
            {
                var config = decorator.GetWeaponConfig();

                var upgradeProgression = _configurationProvider.ProvideExtraWeaponUpgradeProgression(panel.WeaponType);
                var damageProgression = upgradeProgression.GetStatProgression(PlayerWeaponStats.Damage);
                var ammoProgression = upgradeProgression.GetStatProgression(PlayerWeaponStats.ProjectilesCount);
                var projectileSpeedProgresson = upgradeProgression.GetStatProgression(PlayerWeaponStats.ProjectileSpeed);
                var reloadSpeedProgression = upgradeProgression.GetStatProgression(PlayerWeaponStats.ReloadSpeed);

                var damageNextLevel = panel.DamageStat.StatLevel.Value + 1;
                var ammoNextLevel = panel.AmmoStat.StatLevel.Value + 1;
                var projSpeedNextLevel = panel.ProjectileSpeedStat.StatLevel.Value + 1;
                var reloadSpeedNextLevel = panel.ReloadSpeedStat.StatLevel.Value + 1;

                var nextDamage = config.Damage + (float)damageProgression.ValueProgression.HandleUpgradableCalculation(damageNextLevel);
                var nextAmmo = config.ProjectileCount + (float)ammoProgression.ValueProgression.HandleUpgradableCalculation(ammoNextLevel);
                var nextProjectileSpeed = config.Speed + (float)projectileSpeedProgresson.ValueProgression.HandleUpgradableCalculation(projSpeedNextLevel);
                var nextReloadSpeed = config.ReloadSpeed + (float)reloadSpeedProgression.ValueProgression.HandleUpgradableCalculation(reloadSpeedNextLevel);
                
                panel.DamageStat.SetValue(config.Damage);
                panel.AmmoStat.SetValue(config.ProjectileCount);
                panel.ProjectileSpeedStat.SetValue(config.Speed);
                panel.ReloadSpeedStat.SetValue(config.ReloadSpeed);

                panel.DamageStat.SetNextLevelValue(nextDamage);
                panel.AmmoStat.SetNextLevelValue(nextAmmo);
                panel.ProjectileSpeedStat.SetNextLevelValue(nextProjectileSpeed);
                panel.ReloadSpeedStat.SetNextLevelValue(nextReloadSpeed);
            }
        }

        private void RefreshStatsUpgradeCost(PlayerExtraWeaponUpgradePanelDescriptor weaponUpgradePanelDescriptor)
        {
            var upgradeProgression = _configurationProvider.ProvideExtraWeaponUpgradeProgression(weaponUpgradePanelDescriptor.WeaponType);
            var statsProgression = upgradeProgression.PlayerWeaponStatsUpgradeProgression;
            var allStats = weaponUpgradePanelDescriptor.AllStats;

            foreach (var statProgression in statsProgression)
            {
                var stat = allStats.Find(pred=>pred.PlayerWeaponStats == statProgression.PlayerWeaponStat);
                if (stat != null)
                {
                    var nextLevel = stat.StatLevel.Value + 1;
                    var softCostProgression = statProgression.SoftCost;
                    var hardCostProgression = statProgression.HardCost;
                    var softCost = (int)(float)softCostProgression.UpgradableProgressionDescriptor.HandleUpgradableCalculation(nextLevel);
                    var hardCost = (int)(float)hardCostProgression.UpgradableProgressionDescriptor.HandleUpgradableCalculation(nextLevel);

                    stat.SetSoftCurrency(softCostProgression.MoneyType);
                    stat.SetSoftCost(softCost);
                    stat.SetHardCurrency(hardCostProgression.MoneyType);
                    stat.SetHardCost(hardCost);

                    stat.SetSoftCurrencyAvailable(_wallet.HasEnoughMoney(softCostProgression.MoneyType, softCost));
                    stat.SetHardCurrencyAvailable(_wallet.HasEnoughMoney(hardCostProgression.MoneyType, hardCost));
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

        public void InitExtraWeaponDecorator(PlayerExtraWeapon playerExtraWeapon)
        {
            if (_extraWeaponDecorators.TryGetValue(playerExtraWeapon.PlayerWeaponType, out var decorator))
            {
                playerExtraWeapon.InitWeaponConfigDecorator(decorator);
            }
        }

        private void OnSoftCurrencyButtonClick(PlayerExtraWeaponStatUpgradeDescriptor descriptor)
        {
            if (_wallet.TryToSpend(descriptor.SoftCurrency.Value, descriptor.SoftCost.Value))
            {
                UpgradeStat(descriptor);
            }
        }

        private void UpgradeStat(PlayerExtraWeaponStatUpgradeDescriptor descriptor)
        {
            if (_extraWeaponStatsDict.TryGetValue(descriptor, out var weaponType))
            {
                var currentUpgradeLevel = _weaponDecoratorsDataHolder.GetPlayerExtraWeaponLevel(weaponType);
                var nextStatLevel = descriptor.StatLevel.Value + 1;
                var upgradeProgression = _configurationProvider.ProvideExtraWeaponUpgradeProgression(weaponType);
                _weaponDecoratorsDataHolder.SetPlayerExtraWeaponLevel(weaponType, currentUpgradeLevel + 1);
                if (_extraWeaponDecorators.TryGetValue(weaponType, out var decorator))
                {
                    descriptor.SetStatLevel(nextStatLevel);
                    var newAddStats = upgradeProgression.GetPlayerWeaponAdditionalStats(nextStatLevel);
                    var newStatValue = newAddStats.GetStatValue(descriptor.PlayerWeaponStats);
                    decorator.SetStat(descriptor.PlayerWeaponStats, newStatValue);
                    _extraWeaponStatsSaver.SetStatLevel(descriptor.ExtraWeaponType, descriptor.PlayerWeaponStats, nextStatLevel);
                }

                RefreshPanels();
            }
        }

        private void OnHardCurrencyButtonClick(PlayerExtraWeaponStatUpgradeDescriptor descriptor)
        {
            if (_wallet.TryToSpend(descriptor.HardCurrency.Value, descriptor.HardCost.Value))
            {
                UpgradeStat(descriptor);
            }
        }

        public void Dispose()
        {
            foreach (var panelDescriptor in _descriptors)
            {
                panelDescriptor.Dispose();
            }

            _upgradeSystemView.Dispose();
        }
    }
}