using Core.Resourses;
using Core.Storage;
using Core.UI;
using Core.Weapon;
using LunarConsolePlugin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

namespace Core.GameLogic
{
    public interface IPlayerWeaponSelector : IStoragableDictionary
    {
        IPropertyReadOnly<PlayerWeaponType> SelectedMainWeapon { get; }
        IPropertyReadOnly<PlayerWeaponType> SelectedSecondWeapon { get; }
        IPropertyReadOnly<PlayerExtraWeaponType> SelectedExtraWeapon { get; }
        void LinkView(PlayerWeaponSelectorView view);
    }

    public class PlayerWeaponSelector : IPlayerWeaponSelector
    {
        private const string MAIN_WEAPON_KEY = "MainWeapon";
        private const string SECOND_WEAPON_KEY = "SecondWeapon";
        private const string EXTRA_WEAPON_KEY = "ExtraWeapon";
        private PlayerWeaponSelectorView _view;
        private CustomProperty<PlayerExtraWeaponType> _selectedExtraWeaponType = new CustomProperty<PlayerExtraWeaponType>(0);
        private CustomProperty<PlayerWeaponType> _selectedMainWeaponType = new CustomProperty<PlayerWeaponType>(0);
        private CustomProperty<PlayerWeaponType> _selectedSecondWeaponType = new CustomProperty<PlayerWeaponType>(0);
        private PlayerWeaponUnlockProgressionService _playerWeaponUnlockProgressionService;
        private PlayerWeaponSelectorConfiguration _playerWeaponSelectorConfiguration;
        private DroneContainerView _droneContainerView;
        private IPlayerDroneProgressionService _playerDroneProgressionService;
        private IPlayerDroneSelector _playerDroneSelector;
        private PlayerDroneType _currentDrone;
        private int _droneLevel;
        public IPropertyReadOnly<PlayerWeaponType> SelectedMainWeapon => _selectedMainWeaponType;
        public IPropertyReadOnly<PlayerWeaponType> SelectedSecondWeapon => _selectedSecondWeaponType;
        public IPropertyReadOnly<PlayerExtraWeaponType> SelectedExtraWeapon=> _selectedExtraWeaponType;

        public PlayerWeaponSelector(PlayerWeaponUnlockProgressionService playerWeaponUnlockProgressionService, IPlayerDroneSelector playerDroneSelector, 
        PlayerWeaponSelectorConfiguration playerWeaponSelectorConfiguration, DroneContainerView droneContainerView, IPlayerDroneProgressionService playerDroneProgressionService)
        {
            _playerWeaponUnlockProgressionService = playerWeaponUnlockProgressionService;
            _playerWeaponSelectorConfiguration = playerWeaponSelectorConfiguration;
            _droneContainerView = droneContainerView;
            _playerDroneSelector = playerDroneSelector;
            _playerDroneProgressionService = playerDroneProgressionService;
        }

        private void OnDroneLevelChanged(int level)
        {
            _droneLevel = level;
            _playerWeaponUnlockProgressionService.UpdateData(_droneLevel);
            RefreshWeaponPanels();
        }

        public void RefreshWeaponPanels()
        {
            if (_playerWeaponUnlockProgressionService.CheckSecondWeaponIsAvailable(_droneLevel))
            {
                _view.SetLockedSecondWeaponPanel(false);
            }
            else
            {
                _view.SetLockedSecondWeaponPanel(true, _playerWeaponUnlockProgressionService.LevelToUnlockSecondWeapon);
            }

            if (_playerWeaponUnlockProgressionService.CheckExtraWeaponIsAvailable(_droneLevel))
            {
                _view.SetLockedExtraWeaponPanel(false);
            }
            else
            {
                _view.SetLockedExtraWeaponPanel(true, _playerWeaponUnlockProgressionService.LevelToUnlockExtraWeapon);
            }
        }

        private void Refresh(PlayerDroneType droneType)
        {
            _currentDrone = droneType;
            var availableDroneWeapon = _playerWeaponSelectorConfiguration.GetAvailableWeaponsDescriptor(droneType);
            List<PlayerWeaponStatusDescriptor> playerWeapons = new List<PlayerWeaponStatusDescriptor>();
            List<PlayerExtraWeaponStatusDescriptor> playerExtraWeapons = new List<PlayerExtraWeaponStatusDescriptor>();
            foreach (var weapon in availableDroneWeapon.AvailableWeapons)
            {
                var status = _playerWeaponUnlockProgressionService.GetPlayerWeaponStatus(weapon);
                playerWeapons.Add(status);
            }
            foreach (var extraWeapon in availableDroneWeapon.AvailableExtraWeapons)
            {
                var status = _playerWeaponUnlockProgressionService.GetPlayerExtraWeaponStatus(extraWeapon);
                playerExtraWeapons.Add(status);
            }

            playerWeapons.Sort((x, y) => x.UnlockLevel.CompareTo(y.UnlockLevel));
            playerExtraWeapons.Sort((x, y) => x.UnlockLevel.CompareTo(y.UnlockLevel));

            _view.InitMainWeapons(playerWeapons);

            var startWeapon = _playerWeaponSelectorConfiguration.ProvideDroneStartWeapon(droneType);

            var mainWeapon = playerWeapons.Find(pred=>pred.WeaponType == _selectedMainWeaponType.Value);
            if (startWeapon != null)
            {
                SelectMainWeapon(_selectedMainWeaponType.Value);
            }
            else
            {
                SelectMainWeapon(startWeapon.PlayerWeaponType);
            }
            _view.InitSecondWeapons(playerWeapons);

            if (_playerWeaponUnlockProgressionService.CheckSecondWeaponIsAvailable(_droneLevel))
            {
                var secondWeapon = playerWeapons.Find(pred => pred.WeaponType == _selectedSecondWeaponType.Value && pred.UnlockLevel <= _playerDroneProgressionService.DroneLevel.Value);
                if (secondWeapon != null)
                {
                    SelectSecondWeapon(_selectedSecondWeaponType.Value);
                }
                else
                {
                    SelectSecondWeapon(startWeapon.PlayerSecondWeaponType);
                }
            }

            _view.InitExtraWeapons(playerExtraWeapons);

            if (_playerWeaponUnlockProgressionService.CheckExtraWeaponIsAvailable(_droneLevel))
            {
                SelectExtraWeapon(_selectedExtraWeaponType.Value);
            }

            _droneContainerView.InitDrone(droneType);
            _droneContainerView.InitDroneWeapons(_selectedMainWeaponType.Value, _selectedSecondWeaponType.Value);
            RefreshWeaponPanels();
        }

        public void LinkView(PlayerWeaponSelectorView view)
        {
            _view = view;
            _view.LinkModel(this);
            _playerDroneSelector.SelectedDrone.RegisterValueChangeListener(Refresh);
            _playerDroneProgressionService.DroneLevel.RegisterValueChangeListener(OnDroneLevelChanged);
            _droneLevel = _playerDroneProgressionService.DroneLevel.Value;
            _playerWeaponUnlockProgressionService.UpdateData(_droneLevel);
            Refresh(_playerDroneSelector.SelectedDrone.Value);
        }

        public void SelectMainWeapon(PlayerWeaponType mainWeapon)
        {
            if (_selectedSecondWeaponType.Value == mainWeapon)
            {
                _view.AnimateMainWeaponWarning(mainWeapon);
                _view.AnimateSecondWeaponWarning(_selectedSecondWeaponType.Value);
                return;
            }
            _selectedMainWeaponType.SetValue(mainWeapon, true);
            _droneContainerView.InitDroneWeapons(_selectedMainWeaponType.Value, _selectedSecondWeaponType.Value);
        }

        public void SelectSecondWeapon(PlayerWeaponType secondWeapon)
        {
            if (_selectedMainWeaponType.Value == secondWeapon)
            {
                _view.AnimateMainWeaponWarning(_selectedMainWeaponType.Value);
                _view.AnimateSecondWeaponWarning(secondWeapon);
                return;
            }
            _selectedSecondWeaponType.SetValue(secondWeapon, true);
            _droneContainerView.InitDroneWeapons(_selectedMainWeaponType.Value, _selectedSecondWeaponType.Value);
        }

        public void SelectExtraWeapon(PlayerExtraWeaponType extraWeapon)
        {
            _selectedExtraWeaponType.SetValue(extraWeapon, true);
        }

        public void Save(Dictionary<string, object> data)
        {
            var storageData = new Dictionary<string, object>()
            {
                [MAIN_WEAPON_KEY] = _selectedMainWeaponType.Value.ToString(),
                [SECOND_WEAPON_KEY] = _selectedSecondWeaponType.Value.ToString(),
                [EXTRA_WEAPON_KEY] = _selectedExtraWeaponType.Value.ToString()
            };
            data.Add(nameof(PlayerWeaponSelector), storageData);
        }

        public void Load(Dictionary<string, object> data)
        {
            if (data.TryGetValue(nameof(PlayerWeaponSelector), out var rawData))
            {
                var storageData = rawData as JObject;
                var mainWeaponType = (string)storageData[MAIN_WEAPON_KEY];
                var secondWeaponType = (string)storageData[SECOND_WEAPON_KEY];
                var extraWeaponType = (string)storageData[EXTRA_WEAPON_KEY];

                var mainWeapon = (PlayerWeaponType)Enum.Parse(typeof(PlayerWeaponType), mainWeaponType);
                var secondWeapon = (PlayerWeaponType)Enum.Parse(typeof(PlayerWeaponType), secondWeaponType);
                var extraWeapon = (PlayerExtraWeaponType)Enum.Parse(typeof(PlayerExtraWeaponType), extraWeaponType);

                _selectedMainWeaponType.SetValue(mainWeapon, true);
                _selectedSecondWeaponType.SetValue(secondWeapon, true);
                _selectedExtraWeaponType.SetValue(extraWeapon, true);

                var selectedDrone = _playerDroneSelector.SelectedDrone.Value;
                _droneContainerView.InitDrone(selectedDrone);
            }
        }

        public void Init()
        {
            var selectedDrone = _playerDroneSelector.SelectedDrone.Value;
            var mainWeapon = _playerWeaponSelectorConfiguration.ProvideDroneStartWeapon(selectedDrone).PlayerWeaponType;
            var secondWeapon = PlayerWeaponType.None;
            var extraWeapon = PlayerExtraWeaponType.None;

            _selectedMainWeaponType.SetValue(mainWeapon, true);
            _selectedSecondWeaponType.SetValue(secondWeapon, true);
            _selectedExtraWeaponType.SetValue(extraWeapon, true);

            _droneContainerView.InitDrone(selectedDrone);
        }
    }
}