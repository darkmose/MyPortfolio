using Core.LobbyBase;
using Core.PlayerModule;
using Core.Resourses;
using Core.Storage;
using Core.Tools;
using Core.UI;
using Newtonsoft.Json.Linq;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Core.GameLogic.PlayerDroneProgressionService;

namespace Core.GameLogic
{
    public interface IPlayerDroneProgressionService : IStoragableDictionary
    {
        IPropertyReadOnly<string> DroneName { get; }
        IPropertyReadOnly<int> DroneLevel { get; }
        IPropertyReadOnly<float> DronePower { get; }

        IPropertyReadOnly<float> SpeedValue { get; }
        IPropertyReadOnly<float> ReloadValue { get; }
        IPropertyReadOnly<float> ArmorValue { get; }

        IPropertyReadOnly<int> SpeedSoftCost { get; }
        IPropertyReadOnly<int> ReloadSoftCost { get; }
        IPropertyReadOnly<int> ArmorSoftCost { get; }
        IPropertyReadOnly<int> SpeedHardCost { get; }
        IPropertyReadOnly<int> ReloadHardCost { get; }
        IPropertyReadOnly<int> ArmorHardCost { get; }

        IPropertyReadOnly<bool> EnoughSoftCurrencyForSpeed { get; }
        IPropertyReadOnly<bool> EnoughSoftCurrencyForReload { get; }
        IPropertyReadOnly<bool> EnoughSoftCurrencyForArmor { get; }
        IPropertyReadOnly<bool> EnoughHardCurrencyForSpeed { get; }
        IPropertyReadOnly<bool> EnoughHardCurrencyForReload { get; }
        IPropertyReadOnly<bool> EnoughHardCurrencyForArmor { get; }

        IPropertyReadOnly<MoneyType> SpeedSoftCurrency { get; }
        IPropertyReadOnly<MoneyType> ReloadSoftCurrency { get; }
        IPropertyReadOnly<MoneyType> ArmorSoftCurrency { get; }
        IPropertyReadOnly<MoneyType> SpeedHardCurrency { get; }
        IPropertyReadOnly<MoneyType> ReloadHardCurrency { get; }
        IPropertyReadOnly<MoneyType> ArmorHardCurrency { get; }

        void InitPlayerDroneDecorator(PlayerDroneType droneType, PlayerData playerData);
        void UpgradeStat(DroneStats droneStat, bool isHardCost = false);
    }

    public class PlayerDroneProgressionService : IPlayerDroneProgressionService
    {
        public enum DroneStats
        {
            Speed, Reload, Armor
        }

        private const string UPGRADES_COUNT_KEY = "UpgradesCount";
        private const string SPEED_LEVEL_KEY = "SpeedLevel";
        private const string RELOAD_LEVEL_KEY = "ReloadLevel";
        private const string ARMOR_LEVEL_KEY = "ArmorLevel";
        private const string DRONE_KEY = "DRONE_";
        private IntProperty _droneLevel = new IntProperty(1);
        private FloatProperty _dronePower = new FloatProperty();
        private CustomProperty<string> _droneName = new CustomProperty<string>(string.Empty);
        private FloatProperty _speed = new FloatProperty();
        private FloatProperty _reload = new FloatProperty();  
        private FloatProperty _armor = new FloatProperty();

        private IntProperty _speedSoftCost = new IntProperty(0);
        private IntProperty _reloadSoftCost = new IntProperty(0);
        private IntProperty _armorSoftCost = new IntProperty(0);
        private IntProperty _speedHardCost = new IntProperty(0);
        private IntProperty _reloadHardCost = new IntProperty(0);
        private IntProperty _armorHardCost = new IntProperty(0);

        private BoolProperty _enoughSoftCurrencyForSpeed = new BoolProperty(false); 
        private BoolProperty _enoughSoftCurrencyForReload = new BoolProperty(false); 
        private BoolProperty _enoughSoftCurrencyForArmor = new BoolProperty(false);
        private BoolProperty _enoughHardCurrencyForSpeed = new BoolProperty(false);
        private BoolProperty _enoughHardCurrencyForReload = new BoolProperty(false);
        private BoolProperty _enoughHardCurrencyForArmor = new BoolProperty(false);

        private CustomProperty<MoneyType> _speedSoftCurrency = new CustomProperty<MoneyType>(0);
        private CustomProperty<MoneyType> _reloadSoftCurrency = new CustomProperty<MoneyType>(0);
        private CustomProperty<MoneyType> _armorSoftCurrency = new CustomProperty<MoneyType>(0);
        private CustomProperty<MoneyType> _speedHardCurrency = new CustomProperty<MoneyType>(0);
        private CustomProperty<MoneyType> _reloadHardCurrency = new CustomProperty<MoneyType>(0);
        private CustomProperty<MoneyType> _armorHardCurrency = new CustomProperty<MoneyType>(0);

        public IPropertyReadOnly<string> DroneName => _droneName;
        public IPropertyReadOnly<int> DroneLevel => _droneLevel;
        public IPropertyReadOnly<float> DronePower => _dronePower;

        public IPropertyReadOnly<float> SpeedValue => _speed;
        public IPropertyReadOnly<float> ReloadValue => _reload;
        public IPropertyReadOnly<float> ArmorValue => _armor;

        public IPropertyReadOnly<int> SpeedSoftCost => _speedSoftCost;
        public IPropertyReadOnly<int> ReloadSoftCost => _reloadSoftCost;
        public IPropertyReadOnly<int> ArmorSoftCost => _armorSoftCost;
        public IPropertyReadOnly<int> SpeedHardCost => _speedHardCost;
        public IPropertyReadOnly<int> ReloadHardCost => _reloadHardCost;
        public IPropertyReadOnly<int> ArmorHardCost => _armorHardCost;        

        public IPropertyReadOnly<bool> EnoughSoftCurrencyForSpeed => _enoughSoftCurrencyForSpeed;
        public IPropertyReadOnly<bool> EnoughSoftCurrencyForReload => _enoughSoftCurrencyForReload;
        public IPropertyReadOnly<bool> EnoughSoftCurrencyForArmor => _enoughSoftCurrencyForArmor;
        public IPropertyReadOnly<bool> EnoughHardCurrencyForSpeed => _enoughHardCurrencyForSpeed;
        public IPropertyReadOnly<bool> EnoughHardCurrencyForReload => _enoughHardCurrencyForReload;
        public IPropertyReadOnly<bool> EnoughHardCurrencyForArmor => _enoughHardCurrencyForArmor;

        public IPropertyReadOnly<MoneyType> SpeedSoftCurrency => _speedSoftCurrency;
        public IPropertyReadOnly<MoneyType> ReloadSoftCurrency => _reloadSoftCurrency;
        public IPropertyReadOnly<MoneyType> ArmorSoftCurrency => _armorSoftCurrency;
        public IPropertyReadOnly<MoneyType> SpeedHardCurrency => _speedHardCurrency;
        public IPropertyReadOnly<MoneyType> ReloadHardCurrency => _reloadHardCurrency;
        public IPropertyReadOnly<MoneyType> ArmorHardCurrency => _armorHardCurrency;

        private Dictionary<PlayerDroneType, int> _upgradesCountDictionary;
        private PlayerDroneType _currentDrone;
        private Dictionary<PlayerDroneType, IPlayerDroneDecorator> _droneDecoratorsDictionary;
        private DroneProvider _droneProvider;
        private PlayerDroneUpgradeConfiguration _playerDroneUpgradeConfiguration;
        private IPlayerDroneSelector _playerDroneSelector;
        private IWallet _wallet;

        public PlayerDroneProgressionService(IWallet wallet, DroneProvider droneProvider, IPlayerDroneSelector playerDroneSelector)
        {
            _playerDroneUpgradeConfiguration = Resources.Load<PlayerDroneUpgradeConfiguration>("ScriptableObjects/" + nameof(PlayerDroneUpgradeConfiguration));
            _wallet = wallet;
            _playerDroneSelector = playerDroneSelector;
            _playerDroneSelector.SelectedDrone.RegisterValueChangeListener(OnDroneChanged);
            _droneProvider = droneProvider;
            PrepareDictionary();
        }

        private void OnDroneChanged(PlayerDroneType playerDroneType)
        {
            _currentDrone = playerDroneType;
            _droneName.SetValue(playerDroneType.ToString().SplitPascalCase(), true);
            RefreshUpgradeData();
        }

        private void PrepareDictionary()
        {
            _droneDecoratorsDictionary = new Dictionary<PlayerDroneType, IPlayerDroneDecorator>();
            _upgradesCountDictionary = new Dictionary<PlayerDroneType, int>();
            var droneTypes = Enum.GetNames(typeof(PlayerDroneType));
            for (int i = 0; i < droneTypes.Length; i++)
            {
                var droneType = (PlayerDroneType)i;
                var initConfig = _droneProvider.ProvideByType(droneType).DroneConfig;
                var initDecorator = new InitPlayerDroneDecorator(initConfig);
                var droneDecorator = new PlayerDroneUpgradeDecorator(initDecorator);
                _droneDecoratorsDictionary.Add(droneType, droneDecorator);
                _upgradesCountDictionary.Add(droneType, 0);
            }            
        }

        public void InitPlayerDroneDecorator(PlayerDroneType droneType, PlayerData playerData)
        {
            var droneDecorator = _droneDecoratorsDictionary[droneType];
            playerData.InitDroneDecorator(droneDecorator);
        }

        private void CalculateDronePower()
        {
            var decorator = _droneDecoratorsDictionary[_currentDrone] as PlayerDroneUpgradeDecorator;
            var speedLevel = decorator.GetStatLevel(DroneStats.Speed);
            var reloadLevel = decorator.GetStatLevel(DroneStats.Reload);
            var armorLevel = decorator.GetStatLevel(DroneStats.Armor);
            var summaryLevel = speedLevel + armorLevel + reloadLevel;

            var power = (float)summaryLevel / 3f;
            _dronePower.SetValue(power, false);
        }

        private void CalculateDroneLevel()
        {
            var level = _upgradesCountDictionary[_currentDrone];
            _droneLevel.SetValue(1+level, false);
        }

        private void RefreshCosts()
        {
            var upgradeDescriptor = _playerDroneUpgradeConfiguration.ProvideDroneUpgradeDescriptor(_currentDrone);

            {
                //SPEED
                if (TryRefreshStatCosts(DroneStats.Speed, upgradeDescriptor, out int softCost, out int hardCost, out bool enoughSoftCurrency, out bool enoughHardCurrency, out var softCurrency, out var hardCurrency))
                {
                    _speedSoftCost.SetValue(softCost, false);
                    _speedSoftCurrency.SetValue(softCurrency, false);
                    _enoughSoftCurrencyForSpeed.SetValue(enoughSoftCurrency, false);

                    _speedHardCost.SetValue(hardCost, false);
                    _speedHardCurrency.SetValue(hardCurrency, false);
                    _enoughHardCurrencyForSpeed.SetValue(enoughHardCurrency, false);
                }
            }
            {
                //RELOAD
                if (TryRefreshStatCosts(DroneStats.Reload, upgradeDescriptor, out int softCost, out int hardCost, out bool enoughSoftCurrency, out bool enoughHardCurrency, out var softCurrency, out var hardCurrency))
                {
                    _reloadSoftCost.SetValue(softCost, false);
                    _reloadSoftCurrency.SetValue(softCurrency, false);
                    _enoughSoftCurrencyForReload.SetValue(enoughSoftCurrency, false);

                    _reloadHardCost.SetValue(hardCost, false);
                    _reloadHardCurrency.SetValue(hardCurrency, false);
                    _enoughHardCurrencyForReload.SetValue(enoughHardCurrency, false);
                }
            }           
            {
                //ARMOR
                if (TryRefreshStatCosts(DroneStats.Armor, upgradeDescriptor, out int softCost, out int hardCost, out bool enoughSoftCurrency, out bool enoughHardCurrency, out var softCurrency, out var hardCurrency))
                {
                    _armorSoftCost.SetValue(softCost, false);
                    _armorSoftCurrency.SetValue(softCurrency, false);
                    _enoughSoftCurrencyForArmor.SetValue(enoughSoftCurrency, false);

                    _armorHardCost.SetValue(hardCost, false);
                    _armorHardCurrency.SetValue(hardCurrency, false);
                    _enoughHardCurrencyForArmor.SetValue(enoughHardCurrency, false);
                }
            }
        }

        private bool TryRefreshStatCosts(DroneStats droneStats, DroneUpgradeDescriptor droneUpgradeDescriptor, out int softCost, 
        out int hardCost, out bool enoughSoftCurrency, out bool enoughHardCurrency, out MoneyType softCurrency, out MoneyType hardCurrency)
        {
            CostProgressionDescriptor softCostProgression = null;
            CostProgressionDescriptor hardCostProgression = null;
            softCost = 0;
            hardCost = 0;
            enoughSoftCurrency = false;
            enoughHardCurrency = false;
            softCurrency = MoneyType.Coins;
            hardCurrency = MoneyType.Coins;
            var decorator = _droneDecoratorsDictionary[_currentDrone] as PlayerDroneUpgradeDecorator;
            var statLevel = 0;

            switch (droneStats)
            {
                case DroneStats.Speed:
                    softCostProgression = droneUpgradeDescriptor.SpeedSoftCostProgression;
                    hardCostProgression = droneUpgradeDescriptor.SpeedHardCostProgression;
                    statLevel = decorator.GetStatLevel(DroneStats.Speed);
                    break;
                case DroneStats.Reload:
                    softCostProgression = droneUpgradeDescriptor.ReloadSoftCostProgression;
                    hardCostProgression = droneUpgradeDescriptor.ReloadHardCostProgression;
                    statLevel = decorator.GetStatLevel(DroneStats.Reload);
                    break;
                case DroneStats.Armor:
                    softCostProgression = droneUpgradeDescriptor.ArmorSoftCostProgression;
                    hardCostProgression = droneUpgradeDescriptor.ArmorHardCostProgression;
                    statLevel = decorator.GetStatLevel(DroneStats.Armor);
                    break;
                default:
                    return false;
            }
            softCurrency = softCostProgression.MoneyType;
            softCost = (int)(float)softCostProgression.UpgradableProgressionDescriptor.HandleUpgradableCalculation(statLevel + 1);
            enoughSoftCurrency = _wallet.HasEnoughMoney(softCurrency, softCost);
            
            hardCurrency = hardCostProgression.MoneyType;
            hardCost = (int)(float)hardCostProgression.UpgradableProgressionDescriptor.HandleUpgradableCalculation(statLevel + 1);
            enoughHardCurrency = _wallet.HasEnoughMoney(hardCurrency, hardCost);
            return true;
        }

        private void RefreshUpgradeData()
        {
            var currentDroneConfig = _droneDecoratorsDictionary[_currentDrone].GetDroneConfig();
            _speed.SetValue(currentDroneConfig.Speed, false);
            _reload.SetValue(currentDroneConfig.Reload, false);
            _armor.SetValue(currentDroneConfig.Armor, false);
            CalculateDroneLevel();
            CalculateDronePower();
            RefreshCosts();
        }

        public void UpgradeStat(DroneStats droneStat, bool isHardCost = false)
        {
            var droneDecorator = _droneDecoratorsDictionary[_currentDrone];
            PlayerDroneUpgradeDecorator playerDroneDecorator = null;
            MoneyType currency = 0;
            var decorator = _droneDecoratorsDictionary[_currentDrone] as PlayerDroneUpgradeDecorator;
            var statLevel = 0;
            int cost = 0;

            BaseUpgradableProgressionDescriptor upgradeProgression = null;

            switch (droneStat)
            {
                case DroneStats.Speed:
                    upgradeProgression = _playerDroneUpgradeConfiguration.ProvideDroneUpgradeDescriptor(_currentDrone).SpeedProgression;
                    currency = isHardCost ? _speedHardCurrency.Value : _speedSoftCurrency.Value;
                    cost = isHardCost ? _speedHardCost.Value : _speedSoftCost.Value;
                    statLevel = decorator.GetStatLevel(DroneStats.Speed);
                    break;
                case DroneStats.Reload:
                    upgradeProgression = _playerDroneUpgradeConfiguration.ProvideDroneUpgradeDescriptor(_currentDrone).ReloadSpeedProgression;
                    currency = isHardCost ? _reloadHardCurrency.Value : _reloadSoftCurrency.Value;
                    cost = isHardCost ? _reloadHardCost.Value : _reloadSoftCost.Value;
                    statLevel = decorator.GetStatLevel(DroneStats.Reload);
                    break;
                case DroneStats.Armor:
                    upgradeProgression = _playerDroneUpgradeConfiguration.ProvideDroneUpgradeDescriptor(_currentDrone).ArmorProgression;
                    currency = isHardCost ? _armorHardCurrency.Value : _armorSoftCurrency.Value;
                    cost = isHardCost ? _armorHardCost.Value : _armorSoftCost.Value;
                    statLevel = decorator.GetStatLevel(DroneStats.Armor);
                    break;
            }

            if (_wallet.TryToSpend(currency, cost))
            {
                playerDroneDecorator = _droneDecoratorsDictionary[_currentDrone] as PlayerDroneUpgradeDecorator;
                var nextValue = (float)upgradeProgression.HandleUpgradableCalculation(statLevel + 1);
                switch (droneStat)
                {
                    case DroneStats.Speed:
                        playerDroneDecorator.AddSpeed(nextValue);
                        break;
                    case DroneStats.Reload:
                        playerDroneDecorator.AddReloadSpeed(nextValue);
                        break;
                    case DroneStats.Armor:
                        playerDroneDecorator.AddArmor(nextValue);
                        break;
                }
                _upgradesCountDictionary[_currentDrone] += 1;
                RefreshUpgradeData();
            }
        }

        public void Save(Dictionary<string, object> data)
        {
            var storageData = new Dictionary<string, object>();


            var drones = Enum.GetNames(typeof(PlayerDroneType));

            for (int i = 0; i < drones.Length; i++)
            {
                var droneType = (PlayerDroneType)i;
                var droneData = new Dictionary<string, object>();
                var upgradesCount = _upgradesCountDictionary[droneType];
                var droneDecorator = _droneDecoratorsDictionary[droneType] as PlayerDroneUpgradeDecorator;

                droneData.Add(UPGRADES_COUNT_KEY, upgradesCount);
                
                var speedLevel = droneDecorator.GetStatLevel(DroneStats.Speed);
                var reloadLevel = droneDecorator.GetStatLevel(DroneStats.Reload);
                var armorLevel = droneDecorator.GetStatLevel(DroneStats.Armor);

                droneData.Add(SPEED_LEVEL_KEY, speedLevel);
                droneData.Add(RELOAD_LEVEL_KEY, reloadLevel);
                droneData.Add(ARMOR_LEVEL_KEY, armorLevel);

                storageData.Add(DRONE_KEY + i.ToString(), droneData);
            }

            data.Add(nameof(PlayerDroneProgressionService), storageData);
        }

        public void Load(Dictionary<string, object> data)
        {
            if (data.TryGetValue(nameof(PlayerDroneProgressionService), out var rawData))
            {
                var storageData = rawData as JObject;

                for (int i = 0; i < storageData.Count; i++)
                {
                    var droneData = (JObject)storageData[DRONE_KEY + i.ToString()];
                    var upgradesCount = (int)droneData[UPGRADES_COUNT_KEY];
                    var speedLevel = (int)droneData[SPEED_LEVEL_KEY];
                    var reloadLevel = (int)droneData[RELOAD_LEVEL_KEY];
                    var armorLevel = (int)droneData[ARMOR_LEVEL_KEY];

                    var decorator = _droneDecoratorsDictionary[(PlayerDroneType)i] as PlayerDroneUpgradeDecorator;
                    decorator.SetStatLevel(DroneStats.Speed, speedLevel);
                    decorator.SetStatLevel(DroneStats.Reload, reloadLevel);
                    decorator.SetStatLevel(DroneStats.Armor, armorLevel);


                    var speedProgression = _playerDroneUpgradeConfiguration.ProvideDroneUpgradeDescriptor((PlayerDroneType)i).SpeedProgression;
                    var reloadProgression = _playerDroneUpgradeConfiguration.ProvideDroneUpgradeDescriptor((PlayerDroneType)i).ReloadSpeedProgression;
                    var armorProgression = _playerDroneUpgradeConfiguration.ProvideDroneUpgradeDescriptor((PlayerDroneType)i).ArmorProgression;

                    var speedValue = (float)speedProgression.HandleUpgradableCalculation(speedLevel);
                    var reloadValue = (float)reloadProgression.HandleUpgradableCalculation(reloadLevel);
                    var armorValue = (float)armorProgression.HandleUpgradableCalculation(armorLevel);

                    var config = new PlayerDroneConfig()
                    {
                        Armor = armorValue,
                        Speed = speedValue,
                        Reload = reloadValue
                    };
                    decorator.SetConfig(config);

                    _upgradesCountDictionary[(PlayerDroneType)i] = upgradesCount;
                    RefreshUpgradeData();
                }
            }
        }

        public void Init()
        {
            RefreshUpgradeData();
        }
    }
}