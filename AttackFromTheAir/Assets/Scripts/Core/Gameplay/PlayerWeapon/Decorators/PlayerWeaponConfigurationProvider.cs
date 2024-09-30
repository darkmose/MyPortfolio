using Core.PlayerModule;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameLogic
{
    [CreateAssetMenu(fileName ="WeaponUpgradeConfigProvider", menuName ="ScriptableObjects/WeaponUpgradeConfigProvider")]
    public class PlayerWeaponConfigurationProvider : ScriptableObject
    {
        [SerializeField] private List<PlayerWeaponDataDescriptor> _playerWeaponDataDescriptors;
        [SerializeField] private List<PlayerExtraWeaponDataDescriptor> _playerExtraWeaponDataDescriptors;
        private Dictionary<PlayerWeaponType, PlayerWeaponConfig> _playerWeaponsInitConfigs;
        private Dictionary<PlayerWeaponType, List<PlayerWeaponUpgradeDescriptor>> _playerWeaponUpgradeConfigs;
        private Dictionary<PlayerExtraWeaponType, PlayerWeaponConfig> _playerExtraWeaponsInitConfigs;
        private Dictionary<PlayerExtraWeaponType, List<PlayerWeaponUpgradeDescriptor>> _playerExtraWeaponUpgradeConfigs;

        private void PrepareWeaponDictionary()
        {
            if (_playerWeaponsInitConfigs == null) 
            {
                _playerWeaponsInitConfigs = new Dictionary<PlayerWeaponType, PlayerWeaponConfig>();

                foreach (var data in _playerWeaponDataDescriptors)
                {
                    _playerWeaponsInitConfigs.Add(data.PlayerWeaponType ,data.InitWeaponConfig);
                }
            }
            if (_playerWeaponUpgradeConfigs == null)
            {
                _playerWeaponUpgradeConfigs = new Dictionary<PlayerWeaponType, List<PlayerWeaponUpgradeDescriptor>>();

                foreach (var data in _playerWeaponDataDescriptors)
                {
                    _playerWeaponUpgradeConfigs.Add(data.PlayerWeaponType, data.PlayerWeaponUpgradeDescriptors);
                }
            }
        }

        private void PrepareExtraWeaponDictionary()
        {
            if (_playerExtraWeaponsInitConfigs == null)
            {
                _playerExtraWeaponsInitConfigs = new Dictionary<PlayerExtraWeaponType, PlayerWeaponConfig>();

                foreach (var data in _playerExtraWeaponDataDescriptors)
                {
                    _playerExtraWeaponsInitConfigs.Add(data.ExtraWeaponType, data.InitWeaponConfig);
                }
            }
            if (_playerExtraWeaponUpgradeConfigs == null)
            {
                _playerExtraWeaponUpgradeConfigs = new Dictionary<PlayerExtraWeaponType, List<PlayerWeaponUpgradeDescriptor>>();

                foreach (var data in _playerExtraWeaponDataDescriptors)
                {
                    _playerExtraWeaponUpgradeConfigs.Add(data.ExtraWeaponType, data.PlayerWeaponUpgradeDescriptors);
                }
            }
        }

        public PlayerWeaponConfig ProvideWeaponInitConfig(PlayerWeaponType playerWeaponType)
        {
            PrepareWeaponDictionary();
            if (_playerWeaponsInitConfigs.TryGetValue(playerWeaponType, out var config))
            {
                return config;
            }
            else
            {
                throw new System.Exception($"Provider does not contain init config of player weapon {playerWeaponType}");
            }
        }

        public PlayerWeaponConfig ProvideExtraWeaponInitConfig(PlayerExtraWeaponType playerExtraWeaponType)
        {
            PrepareExtraWeaponDictionary();
            if (_playerExtraWeaponsInitConfigs.TryGetValue(playerExtraWeaponType, out var config))
            {
                return config;
            }
            else
            {
                throw new System.Exception($"Provider does not contain init config of player extra weapon {playerExtraWeaponType}");
            }
        }

        public PlayerWeaponUpgradeDescriptor ProvideWeaponUpgradeDescriptor(PlayerWeaponType playerWeaponType, int upgradeLevel)
        {
            PrepareWeaponDictionary();
            if (_playerWeaponUpgradeConfigs.TryGetValue(playerWeaponType, out var descriptors))
            {
                var descriptor = descriptors.Find(pred => pred.UpgradeLevel == upgradeLevel);
                if (descriptor != null)
                {
                    return descriptor;
                }
                else
                {
                    throw new System.Exception($"Provider does not contain upgrade configuration of level {upgradeLevel} for player weapon {playerWeaponType}");
                }
            }
            else
            {
                throw new System.Exception($"Provider does not contain upgrade configuration for player weapon {playerWeaponType}");
            }
        }

        public PlayerWeaponUpgradeDescriptor ProvideExtraWeaponUpgradeDescriptor(PlayerExtraWeaponType playerExtraWeaponType, int upgradeLevel)
        {
            PrepareExtraWeaponDictionary();
            if (_playerExtraWeaponUpgradeConfigs.TryGetValue(playerExtraWeaponType, out var descriptors))
            {
                var descriptor = descriptors.Find(pred => pred.UpgradeLevel == upgradeLevel);
                if (descriptor != null)
                {
                    return descriptor;
                }
                else
                {
                    throw new System.Exception($"Provider does not contain upgrade configuration of level {upgradeLevel} for player weapon {playerExtraWeaponType}");
                }
            }
            else
            {
                throw new System.Exception($"Provider does not contain upgrade configuration for player weapon {playerExtraWeaponType}");
            }
        }

        public bool HasWeaponUpgradeDescriptor(PlayerWeaponType playerWeaponType, int upgradeLevel)
        {
            PrepareWeaponDictionary();
            
            if (_playerWeaponUpgradeConfigs.TryGetValue(playerWeaponType, out var upgradeDescriptors))
            {
                var hit = upgradeDescriptors.Find(pred => pred.UpgradeLevel == upgradeLevel);
                if (hit!=null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool HasExtraWeaponUpgradeDescriptor(PlayerExtraWeaponType playerExtraWeaponType, int upgradeLevel)
        {
            PrepareExtraWeaponDictionary();

            if (_playerExtraWeaponUpgradeConfigs.TryGetValue(playerExtraWeaponType, out var upgradeDescriptors))
            {
                var hit = upgradeDescriptors.Find(pred => pred.UpgradeLevel == upgradeLevel);
                if (hit != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public int GetWeaponRequiredLevel(PlayerWeaponType playerWeaponType)
        {
            PrepareWeaponDictionary();
            var descriptor = _playerWeaponDataDescriptors.Find(pred => pred.PlayerWeaponType == playerWeaponType);
            if (descriptor != null)
            {
                return descriptor.RequiredLevel;
            }
            else
            {
                return 1;
            }
        }
        
        public int GetExtraWeaponRequiredLevel(PlayerExtraWeaponType playerWeaponType)
        {
            PrepareExtraWeaponDictionary();
            var descriptor = _playerExtraWeaponDataDescriptors.Find(pred => pred.ExtraWeaponType == playerWeaponType);
            if (descriptor != null)
            {
                return descriptor.RequiredLevel;
            }
            else
            {
                return 1;
            }
        }


    }

    [System.Serializable]
    public class PlayerWeaponDataDescriptor
    {
        public PlayerWeaponType PlayerWeaponType;
        public PlayerWeaponConfig InitWeaponConfig;
        public int RequiredLevel = 1;
        public List<PlayerWeaponUpgradeDescriptor> PlayerWeaponUpgradeDescriptors;
    }

    [System.Serializable]
    public class PlayerExtraWeaponDataDescriptor
    {
        public PlayerExtraWeaponType ExtraWeaponType;
        public PlayerWeaponConfig InitWeaponConfig;
        public int RequiredLevel = 1;
        public List<PlayerWeaponUpgradeDescriptor> PlayerWeaponUpgradeDescriptors;
    }

    [System.Serializable]
    public class PlayerWeaponUpgradeDescriptor
    {
        public int UpgradeLevel = 1;
        public PlayerWeaponConfig AdditionalStats;
        public CostDescriptor SoftCost;
        public CostDescriptor HardCost;
    }

    [System.Serializable]
    public class CostDescriptor
    {
        public MoneyType MoneyType;
        public int Cost;
    }
}