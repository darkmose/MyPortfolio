using Core.PlayerModule;
using Core.Resourses;
using Core.Storage;
using Core.UI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Core.GameLogic
{
    public interface IPlayerDroneSelector : IStoragableDictionary
    {
        IPropertyReadOnly<PlayerDroneType> SelectedDrone { get; }
        IPropertyReadOnly<PlayerDroneType> SelectedAvailableDrone { get; }
        IPropertyReadOnly<bool> IsCurrentDroneAvailable { get; }
        IPropertyReadOnly<int> CurrentDroneRequiredPlayerLevel { get; }
        void NextDrone();
        void PreviousDrone();
        void SelectDrone(PlayerDroneType droneType);
        bool CheckDroneLocked(PlayerDroneType droneType);
    }

    public class PlayerDroneSelector : IPlayerDroneSelector
    {
        private const string SELECTED_DRONE_KEY = "SelectedDrone";
        private CustomProperty<PlayerDroneType> _selectedDrone = new CustomProperty<PlayerDroneType>(0);
        private CustomProperty<PlayerDroneType> _selectedAvailableDrone = new CustomProperty<PlayerDroneType>(0);
        private BoolProperty _isCurrentDroneAvailable = new BoolProperty(false);
        private IntProperty _currentDroneRequiredPlayerLevel = new IntProperty(0);
        private DroneProvider _droneProvider;
        private IPlayerExpirienceService _playerExpirienceService;
        public IPropertyReadOnly<PlayerDroneType> SelectedDrone => _selectedDrone;
        public IPropertyReadOnly<PlayerDroneType> SelectedAvailableDrone => _selectedAvailableDrone;
        public IPropertyReadOnly<bool> IsCurrentDroneAvailable => _isCurrentDroneAvailable;
        public IPropertyReadOnly<int> CurrentDroneRequiredPlayerLevel => _currentDroneRequiredPlayerLevel;

        public PlayerDroneSelector(DroneProvider droneProvider, IPlayerExpirienceService playerExpirienceService)
        {
            _droneProvider = droneProvider;
            _playerExpirienceService = playerExpirienceService;
        }

        public void NextDrone()
        {
            var dronesCount = Enum.GetNames(typeof(PlayerDroneType)).Length;
            var currentDrone = (int)_selectedDrone.Value;
            if (currentDrone < dronesCount - 1)
            {
                currentDrone++;
            }
            else
            {
                currentDrone = 0;
            }
            SelectDrone((PlayerDroneType)currentDrone);
        }

        public void PreviousDrone()
        {
            var dronesCount = Enum.GetNames(typeof(PlayerDroneType)).Length;
            var currentDrone = (int)_selectedDrone.Value;
            if (currentDrone > 0)
            {
                currentDrone--;
            }
            else
            {
                currentDrone = dronesCount - 1;
            }
            SelectDrone((PlayerDroneType)currentDrone);
        }

        public void SelectDrone(PlayerDroneType drone)
        {
            _selectedDrone.SetValue(drone, false);
            CheckAvailability(drone);
        }

        private void CheckAvailability(PlayerDroneType playerDrone)
        {
            var droneConfig = _droneProvider.ProvideByType(playerDrone);
            var availability = _playerExpirienceService.PlayerLevel.Value >= droneConfig.PlayerLevelRequired;
            _isCurrentDroneAvailable.SetValue(availability);
            if (!availability)
            {
                SetRequiredLevel(droneConfig.PlayerLevelRequired);
            }
        }

        private void SetRequiredLevel(int requiredLevel)
        {
            _currentDroneRequiredPlayerLevel.SetValue(requiredLevel, false);
        }

        public void Save(Dictionary<string, object> data)
        {
            var storageData = new Dictionary<string, object>();
                storageData.Add(SELECTED_DRONE_KEY, _selectedDrone.Value.ToString());
            data.Add(nameof(PlayerDroneSelector), storageData);
        }

        public void Load(Dictionary<string, object> data)
        {
            if (data.TryGetValue(nameof(PlayerDroneSelector), out var rawData))
            {
                var storageData = rawData as JObject;
                var selectedDrone = (PlayerDroneType)Enum.Parse(typeof(PlayerDroneType), (string)storageData[SELECTED_DRONE_KEY]);
                SelectDrone(selectedDrone);
            }
        }

        public void Init()
        {
            SelectDrone(PlayerDroneType.SmallDrone);
        }

        public bool CheckDroneLocked(PlayerDroneType droneType)
        {
            var config = _droneProvider.ProvideByType(droneType);
            return _playerExpirienceService.PlayerLevel.Value < config.PlayerLevelRequired;
        }
    }
}