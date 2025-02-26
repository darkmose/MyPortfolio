using Core.PlayerModule;
using Core.Resourses;
using Core.UI;
using Core.Utilities;
using DestroyIt;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameLogic
{
    public class PlayerWeaponUnlockProgressionService 
    {
        private Dictionary<PlayerWeaponType, PlayerWeaponStatusDescriptor> _playerWeaponStatusDescriptors;
        private Dictionary<PlayerExtraWeaponType, PlayerExtraWeaponStatusDescriptor> _playerExtraWeaponStatusDescriptors;
        private PlayerWeaponSelectorConfiguration _playerWeaponSelectorConfiguration;
        private PlayerWeaponConfigurationProvider _configurationProvider;
        public int LevelToUnlockSecondWeapon => _playerWeaponSelectorConfiguration.LevelToUnlockSecondWeapon;
        public int LevelToUnlockExtraWeapon => _playerWeaponSelectorConfiguration.LevelToUnlockExtraWeapon;

        public PlayerWeaponUnlockProgressionService(PlayerWeaponSelectorConfiguration playerWeaponSelectorConfiguration)
        {
            _playerWeaponStatusDescriptors = new Dictionary<PlayerWeaponType, PlayerWeaponStatusDescriptor>();
            _playerExtraWeaponStatusDescriptors = new Dictionary<PlayerExtraWeaponType, PlayerExtraWeaponStatusDescriptor>();
            _configurationProvider = Resources.Load<PlayerWeaponConfigurationProvider>("ScriptableObjects/WeaponUpgradeConfigProvider");
            _playerWeaponSelectorConfiguration = playerWeaponSelectorConfiguration;
            InitData();
        }

        private void InitData()
        {
            var allWeapons = Enum.GetNames(typeof(PlayerWeaponType));
            var allExtraWeapons = Enum.GetValues(typeof(PlayerExtraWeaponType));

            for (int i = 0; i < allWeapons.Length - 1; i++)
            {
                var weaponType = (PlayerWeaponType)i; 
                var weaponRequiredLevel = _configurationProvider.GetWeaponRequiredLevel(weaponType);
                var statusDescriptor = new PlayerWeaponStatusDescriptor();
                statusDescriptor.UnlockLevel = weaponRequiredLevel;
                statusDescriptor.IsLocked = true;
                statusDescriptor.WeaponType = weaponType;
                _playerWeaponStatusDescriptors.Add(weaponType, statusDescriptor);
            }

            for (int i = 0; i < allExtraWeapons.Length - 1; i++)
            {
                var extraWeaponType = (PlayerExtraWeaponType)i;
                var weaponRequiredLevel = _configurationProvider.GetExtraWeaponRequiredLevel(extraWeaponType);
                var statusDescriptor = new PlayerExtraWeaponStatusDescriptor();
                statusDescriptor.UnlockLevel = weaponRequiredLevel;
                statusDescriptor.IsLocked = true;
                statusDescriptor.ExtraWeaponType = extraWeaponType;
                _playerExtraWeaponStatusDescriptors.Add(extraWeaponType, statusDescriptor);
            }
        }

        public bool CheckSecondWeaponIsAvailable(int droneLevel)
        {
            return droneLevel >= LevelToUnlockSecondWeapon;
        }

        public bool CheckExtraWeaponIsAvailable(int droneLevel)
        {
            return droneLevel >= LevelToUnlockExtraWeapon;
        }

        public PlayerExtraWeaponStatusDescriptor GetPlayerExtraWeaponStatus(PlayerExtraWeaponType playerExtraWeaponType)
        {
            if (_playerExtraWeaponStatusDescriptors.TryGetValue(playerExtraWeaponType, out var playerExtraWeaponStatus))
            {
                return playerExtraWeaponStatus;
            }
            else
            {
                throw new System.Exception();
            }
        }

        public PlayerWeaponStatusDescriptor GetPlayerWeaponStatus(PlayerWeaponType playerWeaponType)
        {
            if (_playerWeaponStatusDescriptors.TryGetValue(playerWeaponType, out var playerWeaponStatus))
            {
                return playerWeaponStatus;
            }
            else
            {
                throw new System.Exception();
            }
        }

        public void UpdateData(int droneLevel)
        {
            foreach (var item in _playerWeaponStatusDescriptors)
            {
                var weaponType = item.Key;
                var statusDescriptor = item.Value;
                var weaponRequiredLevel = _configurationProvider.GetWeaponRequiredLevel(weaponType);
                var isWeaponLocked = droneLevel < statusDescriptor.UnlockLevel;
                if (statusDescriptor.IsLocked != isWeaponLocked)
                {
                    statusDescriptor.IsLocked = isWeaponLocked;
                    statusDescriptor.LockStatusChangedEvent.Notify(statusDescriptor, isWeaponLocked);
                }
            }
            foreach (var item in _playerExtraWeaponStatusDescriptors) 
            {
                var extraWeaponType = item.Key;
                var statusDescriptor = item.Value;
                var isWeaponLocked = droneLevel < statusDescriptor.UnlockLevel;
                if (statusDescriptor.IsLocked != isWeaponLocked)
                {
                    statusDescriptor.IsLocked = isWeaponLocked;
                    statusDescriptor.LockStatusChangedEvent.Notify(statusDescriptor, isWeaponLocked);
                }
            }
        }
    }

    public class PlayerWeaponStatusDescriptor
    {
        public PlayerWeaponType WeaponType;
        public bool IsLocked;
        public SimpleEvent<PlayerWeaponStatusDescriptor, bool> LockStatusChangedEvent = new SimpleEvent<PlayerWeaponStatusDescriptor, bool>();
        public int UnlockLevel;
    }

    public class PlayerExtraWeaponStatusDescriptor
    {
        public PlayerExtraWeaponType ExtraWeaponType;
        public bool IsLocked;
        public SimpleEvent<PlayerExtraWeaponStatusDescriptor, bool> LockStatusChangedEvent = new SimpleEvent<PlayerExtraWeaponStatusDescriptor, bool>();
        public int UnlockLevel;
    }
}