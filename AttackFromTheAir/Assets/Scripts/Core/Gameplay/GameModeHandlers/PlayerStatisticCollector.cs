using Core.Buildings;
using Core.Events;
using Core.Resourses;
using Core.Storage;
using Core.Units;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Core.GameLogic
{
    public interface IPlayerStatisticCollector : IStoragableDictionary
    {
        Dictionary<UnitCategory, int> GetDestroyedUnitsStatistics();
        Dictionary<BuildingType, int> GetDestroyedBuildingStatistics();
    }

    public class PlayerStatisticCollector : IPlayerStatisticCollector
    {
        private const string DESTROYED_BUILDINGS_KEY = "DestroyedBuildings";
        private const string DESTROYED_UNITS_KEY = "DestroyedUnits";
        private Dictionary<PlayerDroneType, Dictionary<UnitCategory, int>> _destroyedUnits = new Dictionary<PlayerDroneType, Dictionary<UnitCategory, int>>();
        private Dictionary<PlayerDroneType ,Dictionary<BuildingType, int>> _destroyedBuildings = new Dictionary<PlayerDroneType, Dictionary<BuildingType, int>>();
        private IPlayerDroneSelector _playerDroneSelector;

        public PlayerStatisticCollector(IPlayerDroneSelector playerDroneSelector)
        {
            EventAggregator.Subscribe<GameEvent>(OnGameEvent);
            var playerDrones = Enum.GetNames(typeof(PlayerDroneType));
            for (int d = 0; d < playerDrones.Length; d++)
            {
                var drone = (PlayerDroneType)d;
                _destroyedUnits.Add(drone, new Dictionary<UnitCategory, int>());
                _destroyedBuildings.Add(drone, new Dictionary<BuildingType, int>());

                var unitCategories = Enum.GetNames(typeof(UnitCategory));
                for (int i = 0; i < unitCategories.Length; i++)
                {
                    var category = (UnitCategory)i;
                    _destroyedUnits[drone].Add(category, 0);
                }
                var buildingTypes = Enum.GetNames(typeof(BuildingType));
                for (int i = 0; i < buildingTypes.Length; i++)
                {
                    var buildingType = (BuildingType)i;
                    _destroyedBuildings[drone].Add(buildingType, 0);
                }
            }

            _playerDroneSelector = playerDroneSelector;
        }

        public void Init()
        {
        }

        public void Load(Dictionary<string, object> data)
        {
            if (data.TryGetValue(nameof(PlayerStatisticCollector), out var rawData))
            {
                var storageData = (JObject)rawData;

                if (storageData[DESTROYED_UNITS_KEY] != null)
                {
                    _destroyedUnits = storageData[DESTROYED_UNITS_KEY]
                        .ToObject<Dictionary<PlayerDroneType, Dictionary<UnitCategory, int>>>() ?? new Dictionary<PlayerDroneType, Dictionary<UnitCategory, int>>();
                }
                if (storageData[DESTROYED_BUILDINGS_KEY] != null)
                {
                    _destroyedBuildings = storageData[DESTROYED_BUILDINGS_KEY]
                        .ToObject<Dictionary<PlayerDroneType, Dictionary<BuildingType, int>>>() ?? new Dictionary<PlayerDroneType, Dictionary<BuildingType, int>>();
                }
            }
        }

        public void Save(Dictionary<string, object> data)
        {
            var storageData = new Dictionary<string, object>()
            {
                [DESTROYED_UNITS_KEY] = _destroyedUnits,
                [DESTROYED_BUILDINGS_KEY] = _destroyedBuildings
            };
            data[nameof(PlayerStatisticCollector)] = storageData;
        }

        private void OnGameEvent(object sender, GameEvent data)
        {
            switch (data.GameEventType)
            {
                case Level.GameEventType.EnemyInfantryKilled:
                    _destroyedUnits[_playerDroneSelector.SelectedDrone.Value][UnitCategory.Infantry]++;
                    break;
                case Level.GameEventType.EnemyMediumEquipmentDestroyed:
                    _destroyedUnits[_playerDroneSelector.SelectedDrone.Value][UnitCategory.MediumEquipment]++;
                    break;
                case Level.GameEventType.EnemyHeavyEquipmentDestroyed:
                    _destroyedUnits[_playerDroneSelector.SelectedDrone.Value][UnitCategory.HeavyEquipment]++;
                    break;
                case Level.GameEventType.EnemyBuildingDestroyed:
                    _destroyedBuildings[_playerDroneSelector.SelectedDrone.Value][BuildingType.Simple]++;
                    break;
            }
        }

        public Dictionary<UnitCategory, int> GetDestroyedUnitsStatistics()
        {
            return _destroyedUnits[_playerDroneSelector.SelectedDrone.Value];
        }

        public Dictionary<BuildingType, int> GetDestroyedBuildingStatistics()
        {
            return _destroyedBuildings[_playerDroneSelector.SelectedDrone.Value];
        }
    }
}