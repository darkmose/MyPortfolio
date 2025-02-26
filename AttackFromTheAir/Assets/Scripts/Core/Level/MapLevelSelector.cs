using Core.PlayerModule;
using Core.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Core.Level
{

    public class MapLevelSelector : IStoragableDictionary
    {
        private const string FINISHED_LEVELS_COUNT_KEY = "FinishedLevelsCount";
        private const string MAP_LEVEL_KEY = "MapLevel";
        private const string MAP_LEVELS_KEY = "MapLevels";
        private const string IS_AVAILABLE_KEY = "IsAvailable";
        private const string IS_FINISHED_KEY = "IsFinished";

        public static int LEVELS_IN_GROUP = 3;

        private ILevelProgression _levelProgression;
        private MapLevelSelectorView _view;
        private LevelSelector _levelSelector;
        private List<MapLevel> _mapLevels;
        private int _finishedLevelsCount;
        public List<MapLevel> MapLevels => _mapLevels;

        public MapLevelSelector(LevelSelector levelSelector, Resourses.ResourceHolder resourceHolder, ILevelProgression levelProgression)
        {
            _levelSelector = levelSelector;
            _levelSelector.CurrentLevel.RegisterValueChangeListener(OnCurrentLevelChanged);
            var allLevelsCount = resourceHolder.Levels.Count;
            _mapLevels = new List<MapLevel>();
            for (int i = 0; i < allLevelsCount; i++)
            {
                var mapLevel = new MapLevel();
                mapLevel.MapLevelClickEvent.AddListener(OnMapLevelClickHandler);
                mapLevel.LevelNumber = i + 1;
                _mapLevels.Add(mapLevel);
            }

            _levelProgression = levelProgression;
        }

        private void OnMapLevelClickHandler(MapLevel level)
        {
            var levelNumber = _mapLevels.IndexOf(level) + 1;
            if (level.IsAvailable.Value)
            {
                SelectLevel(levelNumber);
            }
        }

        public void LinkView(MapLevelSelectorView view)
        {
            _view = view;
            _view.LinkModel(this);
        }

        private void OnCurrentLevelChanged(int level)
        {
            if (_mapLevels.Count >= level)
            {
                SelectLevel(level);
            }
        }

        private void SelectLevel(int level)
        {
            for (int i = 0; i < _mapLevels.Count; i++)
            {
                var mapLevel = _mapLevels[i];

                if ((i+1) == level)
                {
                    mapLevel.Select();
                }
                else
                {
                    mapLevel.Unselect();
                }
            }
            _levelSelector.SetLevel(level);
        }

        public void SelectFirstAvailable()
        {
            MapLevel lastAvailableMapLevel = null;

            foreach (var mapLevel in _mapLevels)
            {
                if (mapLevel.IsAvailable.Value)
                {
                    lastAvailableMapLevel = mapLevel;
                }
                if (mapLevel.IsAvailable.Value && !mapLevel.IsFinished.Value)
                {
                    var levelNumber = mapLevel.LevelNumber;
                    SelectLevel(levelNumber);
                    _view.FocusOnLevel(levelNumber);
                    return;
                }
            }
            if (lastAvailableMapLevel != null)
            {
                SelectLevel(lastAvailableMapLevel.LevelNumber);
                _view.FocusOnLevel(lastAvailableMapLevel.LevelNumber);
            }
        }

        public void SetLevelFinished(int level)
        {
            var mapLevel = _mapLevels[level - 1];
            if (!mapLevel.IsFinished.Value)
            {
                mapLevel.SetFinished(true);
                _finishedLevelsCount++;
                Refresh();
            }
        }

        public List<MapLevel> GetCurrentLevelsGroup()
        {
            var currentLevel = _levelSelector.CurrentLevel.Value;
            var list = new List<MapLevel>();
            int currentLevelsGroup = 0;
            if (currentLevel <= LEVELS_IN_GROUP)
            {
                currentLevelsGroup = 1;
            }
            else
            {
                if (currentLevel % LEVELS_IN_GROUP == 0)
                {
                    currentLevel--;
                }
                currentLevelsGroup = (currentLevel / LEVELS_IN_GROUP) + 1;
            }

            var maxLevel = currentLevelsGroup * LEVELS_IN_GROUP;
            var minLevel = maxLevel - (LEVELS_IN_GROUP - 1);

            for (int i = minLevel - 1; i < maxLevel; i++)
            {
                if (i < _mapLevels.Count)
                {
                    var mapLevel = _mapLevels[i];
                    list.Add(mapLevel);
                }
            }

            return list;
        }

        private void Refresh()
        {
            RefreshMaxLevel();
            RefreshAvailability();
        }

        private void RefreshMaxLevel()
        {
            var maxLevel = ((_finishedLevelsCount / LEVELS_IN_GROUP) * LEVELS_IN_GROUP) + LEVELS_IN_GROUP;
            if (maxLevel <= _mapLevels.Count)
            {
                _levelProgression.SetLevelNumber(maxLevel);
            }
            else
            {
                _levelProgression.SetLevelNumber(_mapLevels.Count);
            }
        }

        private void RefreshAvailability()
        {
            for (int i = 0; i < _mapLevels.Count; i++)
            {
                var levelNumber = i + 1;
                bool isAvailable = false;
                if (levelNumber <= LEVELS_IN_GROUP)
                {
                    isAvailable = true;
                }
                else
                {
                    var result = ((levelNumber / LEVELS_IN_GROUP) * LEVELS_IN_GROUP);
                    if (levelNumber % LEVELS_IN_GROUP == 0)
                    {
                        result -= LEVELS_IN_GROUP;
                    }
                    isAvailable = result <= _finishedLevelsCount;
                }

                _mapLevels[i].SetAvailable(isAvailable);
            }
        }

        public void Save(Dictionary<string, object> data)
        {
            var storageData = new Dictionary<string, object>();
            var mapLevelsData = new Dictionary<string, object>();

            for (int i = 0; i < _mapLevels.Count; i++)
            {
                var key = MAP_LEVEL_KEY + i.ToString();

                var isAvailable = _mapLevels[i].IsAvailable.Value;
                var isFinished = _mapLevels[i].IsFinished.Value;

                var mapLevelData = new Dictionary<string, object>()
                {
                    [IS_AVAILABLE_KEY] = isAvailable,
                    [IS_FINISHED_KEY] = isFinished
                };

                mapLevelsData.Add(key, mapLevelData);
            }

            storageData.Add(FINISHED_LEVELS_COUNT_KEY, _finishedLevelsCount);
            storageData.Add(MAP_LEVELS_KEY, mapLevelsData);
            data.Add(nameof(MapLevelSelector), storageData);
        }

        public void Load(Dictionary<string, object> data)
        {
            if (data.TryGetValue(nameof(MapLevelSelector), out var rawData))
            {
                var storageData = (JObject)rawData;
                _finishedLevelsCount = (int)storageData[FINISHED_LEVELS_COUNT_KEY];
                var mapLevelsData = (JObject)storageData[MAP_LEVELS_KEY];
                for (int i = 0; i < mapLevelsData.Count; i++)
                {
                    var mapLevelData = (JObject)mapLevelsData[MAP_LEVEL_KEY + i.ToString()];
                    var isAvailable = (bool)mapLevelData[IS_AVAILABLE_KEY];
                    var isFinished = (bool)mapLevelData[IS_FINISHED_KEY];

                    _mapLevels[i].SetAvailable(isAvailable);
                    _mapLevels[i].SetFinished(isFinished);
                }
                Refresh();
            }
        }

        public void Init()
        {
            Refresh();
        }
    }
}