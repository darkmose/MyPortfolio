using Core.LobbyBase;
using Core.Resourses;
using Core.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameLogic
{
    public class BeforeLevelDroneSelector : IDisposable, IStoragableDictionary
    {
        private const int DEFAULT_DRONE_USE_AMOUNT = 1;
        private const float DEFAULT_COOLDOWN_DURATION_SEC = 200f;
        private const string AVAILABLE_COUNT_REMAIN_KEY = "AvailableCountRemain";
        private const string REMAIN_TIMER_SECONDS = "RemainTimerSeconds";
        private const string EXIT_TIME_KEY = "ExitTime";
        private const string DRONES_DATA_KEY = "DronesData";
        private const string DRONE_MAX_USES_KEY = "DroneMaxUses";
        private BeforeLevelDroneSelectorView _view;
        private IPlayerDroneSelector _droneSelector;
        private PlayerDroneType _selectedDroneForLevel;
        private BaseObjectsSpecialBonusHandler _baseObjectsSpecialBonusHandler;
        private int _additionalDroneUseAmount;
        private bool _hasSelectedDrone;
        private Dictionary<PlayerDroneType, DroneStatus> _droneStatusDictionary = new Dictionary<PlayerDroneType, DroneStatus>();
        public Dictionary<PlayerDroneType, DroneStatus> DroneStatusDictionary => _droneStatusDictionary;
        public bool HasSelectedDrone => _hasSelectedDrone;

        public BeforeLevelDroneSelector(IPlayerDroneSelector droneSelector, BaseObjectsSpecialBonusHandler baseObjectsSpecialBonusHandler)
        {
            _droneSelector = droneSelector;
            var drones = Enum.GetNames(typeof(PlayerDroneType));
            for (int i = 0; i < drones.Length; i++)
            {
                var drone = (PlayerDroneType)i;
                var droneStatus = new DroneStatus(DEFAULT_COOLDOWN_DURATION_SEC);
                _droneStatusDictionary.Add(drone, droneStatus);
            }
            _baseObjectsSpecialBonusHandler = baseObjectsSpecialBonusHandler;
            _baseObjectsSpecialBonusHandler.SpecialBonusChangedEvent.AddListener(OnSpecialBonusChanged);
        }

        private void OnSpecialBonusChanged(BaseObjectSpecialBonusType type, object value)
        {
            if (type == BaseObjectSpecialBonusType.AdditionalDroneUse)
            {
                _additionalDroneUseAmount = (int)value;
                AddAdditionalDroneUses();
            }
        }

        private void AddAdditionalDroneUses()
        {
            foreach (var item in _droneStatusDictionary)
            {
                var droneStatus = item.Value;
                var droneUses = GetFullDroneUses();
                droneStatus.InitMaxCount(droneUses);
            }
        }

        public void OnDroneItemClick(PlayerDroneType droneType)
        {
            var droneStatus = _droneStatusDictionary[droneType];
            UnselectAll();

            if (droneStatus.CanUse.Value)
            {
                _selectedDroneForLevel = droneType;
                droneStatus.Select();
            }
            else
            {
                //Start ADV
                //on adv complete 
                _selectedDroneForLevel = droneType;
                droneStatus.SetCanUse();
                var droneUses = GetFullDroneUses();
                droneStatus.InitMaxCount(droneUses);
                droneStatus.SetAvailableCount(droneUses);
                droneStatus.Select();
            }

            _hasSelectedDrone = true;
        }

        public void HandleCurrentDroneUsed()
        {
            var droneStatus = _droneStatusDictionary[_selectedDroneForLevel];
            if (droneStatus.AvailableCount.Value == 0)
            {
                return;
            }
            droneStatus.HandleUse();
            _droneSelector.SelectDrone(_selectedDroneForLevel);
            if (droneStatus.AvailableCount.Value == 0)
            {
                droneStatus.StartCooldown();
                droneStatus.CooldownOverEvent.AddListener(OnCooldownOver);
                var droneUses = GetFullDroneUses();
                droneStatus.InitMaxCount(droneUses);
                droneStatus.SetAvailableCount(droneUses);
            }
        }

        private int GetFullDroneUses()
        {
            return DEFAULT_DRONE_USE_AMOUNT + _additionalDroneUseAmount;
        }

        private void OnCooldownOver(DroneStatus status)
        {
            status.CooldownOverEvent.RemoveListener(OnCooldownOver);
            if (!_hasSelectedDrone)
            {
                SelectFirstAvailable();
            }
        }

        public void LinkView(BeforeLevelDroneSelectorView view)
        {
            RefreshDronesLockedStatus();
            _view = view;
            _view.DronePanelClickEvent.AddListener(OnDroneItemClick);
            _view.LinkModel(this);
        }

        private void RefreshDronesLockedStatus()
        {
            foreach (var item in _droneStatusDictionary)
            {
                item.Value.SetLocked(_droneSelector.CheckDroneLocked(item.Key));
            }
        }

        private void UnselectAll()
        {
            foreach (var item in _droneStatusDictionary)
            {
                var drone = item.Value;
                drone.Unselect();
            }
        }

        public void SelectFirstAvailable()
        {
            UnselectAll();
            _hasSelectedDrone = false;
            foreach (var item in _droneStatusDictionary)
            {
                var drone = item.Value;
                if (drone.CanUse.Value && !drone.IsLocked.Value)
                {
                    drone.Select();
                    _selectedDroneForLevel = item.Key;
                    _hasSelectedDrone = true;
                    return;
                }
            }
        }

        public void Dispose()
        {
            foreach (var item in _droneStatusDictionary)
            {
                var status = item.Value;
                status.CooldownRemain.RemoveAllListeners();
                status.CanUse.RemoveAllListeners();
                status.AvailableCount.RemoveAllListeners();
                status.IsLocked.RemoveAllListeners();
                status.IsSelected.RemoveAllListeners();
            }
            _droneStatusDictionary.Clear();
            _view?.DronePanelClickEvent.RemoveListener(OnDroneItemClick);
            _view?.Dispose();
        }

        public void Save(Dictionary<string, object> data)
        {
            var storageData = new Dictionary<string, object>();
            var dronesData = new Dictionary<PlayerDroneType, object>();

            foreach (var item in _droneStatusDictionary)
            {
                var droneData = new Dictionary<string, object>
                {
                    { AVAILABLE_COUNT_REMAIN_KEY, item.Value.AvailableCount.Value },
                    { REMAIN_TIMER_SECONDS, item.Value.RemainTimerTime }
                };
                dronesData.Add(item.Key, droneData);
                Debug.Log($"[SAVE][{item.Key}] {item.Value.RemainTimerTime}");
            }

            storageData.Add(EXIT_TIME_KEY, DateTime.UtcNow.ToString());
            storageData.Add(DRONES_DATA_KEY, dronesData);
            storageData.Add(DRONE_MAX_USES_KEY, GetFullDroneUses());
            data.Add(nameof(BeforeLevelDroneSelector), storageData);
        }

        public void Load(Dictionary<string, object> data)
        {
            if (data.TryGetValue(nameof(BeforeLevelDroneSelector), out var rawData))
            {
                var storageData = (JObject)rawData;
                var exitTime = (string)storageData[EXIT_TIME_KEY];
                var exitTimeConverted = Convert.ToDateTime(exitTime);
                var dronesMaxUseCount = (int)storageData[DRONE_MAX_USES_KEY];
                var dronesData = storageData[DRONES_DATA_KEY].ToObject<Dictionary<PlayerDroneType, object>>();
                

                foreach (var item in dronesData)
                {
                    var jDroneData = (JObject)item.Value;
                    var availableCount = (int)jDroneData[AVAILABLE_COUNT_REMAIN_KEY];
                    var remainTime = (float)jDroneData[REMAIN_TIMER_SECONDS];
                    var timeSinceLastSession = DateTime.UtcNow.Subtract(exitTimeConverted).Seconds;
                    remainTime -= timeSinceLastSession;
                    var droneStatus = _droneStatusDictionary[item.Key];
                    droneStatus.InitMaxCount(dronesMaxUseCount);
                    droneStatus.SetAvailableCount(availableCount);
                    if (remainTime > 0f)
                    {
                        droneStatus.StartCooldown(remainTime);
                    }
                    else
                    {
                        droneStatus.SetCanUse();
                    }
                    Debug.Log($"[LOAD][{item.Key}] RemainTime: {remainTime}");
                }
            }
        }

        public void Init()
        {
        }
    }
}