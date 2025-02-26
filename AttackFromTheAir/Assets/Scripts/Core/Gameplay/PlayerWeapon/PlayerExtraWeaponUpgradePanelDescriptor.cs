using Core.UI;
using System;
using System.Collections.Generic;

namespace Core.GameLogic
{
    public class PlayerExtraWeaponUpgradePanelDescriptor : IDisposable
    {
        private PlayerExtraWeaponType _weaponType;
        private IntProperty _weaponRank = new IntProperty(1);
        private IntProperty _weaponRequiredLevel = new IntProperty(1);
        private PlayerExtraWeaponStatUpgradeDescriptor _damageStat;
        private PlayerExtraWeaponStatUpgradeDescriptor _ammoStat;
        private PlayerExtraWeaponStatUpgradeDescriptor _reloadSpeedStat;         
        private PlayerExtraWeaponStatUpgradeDescriptor _projectileSpeedStat;         
        public PlayerExtraWeaponType WeaponType => _weaponType;
        public IPropertyReadOnly<int> WeaponRank => _weaponRank;
        public IPropertyReadOnly<int> WeaponRequiredLevel => _weaponRequiredLevel;
        public PlayerExtraWeaponStatUpgradeDescriptor DamageStat => _damageStat;
        public PlayerExtraWeaponStatUpgradeDescriptor AmmoStat => _ammoStat;
        public PlayerExtraWeaponStatUpgradeDescriptor ReloadSpeedStat => _reloadSpeedStat;
        public PlayerExtraWeaponStatUpgradeDescriptor ProjectileSpeedStat => _projectileSpeedStat;
        public List<PlayerExtraWeaponStatUpgradeDescriptor> AllStats => new List<PlayerExtraWeaponStatUpgradeDescriptor>() { _damageStat, _ammoStat, _reloadSpeedStat, _projectileSpeedStat };

        public PlayerExtraWeaponUpgradePanelDescriptor(PlayerExtraWeaponType playerWeaponType, PlayerWeaponConfig initWeaponConfig = default)
        {
            _weaponType = playerWeaponType;
            _damageStat = new PlayerExtraWeaponStatUpgradeDescriptor(Resourses.PlayerWeaponStats.Damage, playerWeaponType);
            _ammoStat = new PlayerExtraWeaponStatUpgradeDescriptor(Resourses.PlayerWeaponStats.ProjectilesCount, playerWeaponType);
            _reloadSpeedStat = new PlayerExtraWeaponStatUpgradeDescriptor(Resourses.PlayerWeaponStats.ReloadSpeed, playerWeaponType);
            _projectileSpeedStat = new PlayerExtraWeaponStatUpgradeDescriptor(Resourses.PlayerWeaponStats.ProjectileSpeed, playerWeaponType);
            SetInitialStats(initWeaponConfig);
        }

        private void SetInitialStats(PlayerWeaponConfig weaponConfig)
        {
            _damageStat.SetValue(weaponConfig.Damage);
            _ammoStat.SetValue(weaponConfig.ProjectileCount);
            _reloadSpeedStat.SetValue(weaponConfig.ReloadSpeed);
            _projectileSpeedStat.SetValue(weaponConfig.Speed);
        }

        public void SetWeaponRank(int weaponRank) 
        {
            _weaponRank.SetValue(weaponRank, false);
        }

        public void SetWeaponRequiredLevel(int requiredLevel)
        {
            _weaponRequiredLevel.SetValue(requiredLevel, false);
        }

        public void Dispose()
        {
            _damageStat.Dispose();
            _ammoStat.Dispose();    
            _reloadSpeedStat.Dispose();
            _projectileSpeedStat.Dispose();

            _weaponRank.RemoveAllListeners();
            _weaponRequiredLevel.RemoveAllListeners();
        }
    }
}