using Core.Storage;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.LobbyBase
{

    public interface IPlayerBaseProgression : IStoragableDictionary
    {
        int SelectedBaseIndex { get; }
        int CurrentBaseIndex { get; }
        void SetCurrentBaseIndex(int index);
        void SetSelectedBaseIndex(int index);   
    }

    public class PlayerBasesProgressionSaver : IPlayerBaseProgression
    {
        private const string CURRENT_BASE_INDEX_KEY = "CurrentBaseIndex";
        private const string SELECTED_BASE_INDEX_KEY = "SelectedBaseIndex";
        private const string ALL_BASES_DATA_KEY = "AllBasesData";
        private const string BASE_KEY = "Base_";
        private const string BASE_OBJECT_KEY = "BaseObject_";
        private const string IS_UNLOCKED_KEY = "IsUnlocked";
        private const string RESOURCE_AMOUNT_KEY = "ResourceAmount";
        private const string RESOURCE_GOAL_KEY = "ResourceGoal";
        private const string BUILDING_LEVEL_KEY = "BuildingLevel";
        private const string IS_LOCKED_KEY = "IsLocked";
        private PlayerBasesManager _manager;
        private PlayerBasesManagerView _managerView;
        private PlayerBasesHolder _holder;
        private BaseObjectsSpecialBonusHandler _baseObjectsSpecialBonusHandler;
        public int CurrentBaseIndex { get; private set; }
        public int SelectedBaseIndex { get; private set; }

        public PlayerBasesProgressionSaver(PlayerBasesManager playerBasesManager, PlayerBasesManagerView managerView, PlayerBasesHolder holder, BaseObjectsSpecialBonusHandler baseObjectsSpecialBonusHandler)
        {
            _manager = playerBasesManager;
            _managerView = managerView;
            _holder = holder;
            _baseObjectsSpecialBonusHandler = baseObjectsSpecialBonusHandler;
        }

        public void Init()
        {
            var currentBaseDescriptor = _holder.PlayerBaseDescriptors[CurrentBaseIndex];
            var basesSaveData = new List<BaseSaveData>();
            _manager.InitBases(_holder.PlayerBaseDescriptors, CurrentBaseIndex);
            _manager.LinkView(_managerView);
            _manager.InitLoadData(basesSaveData, SelectedBaseIndex);
            _manager.InitBasesUpgradableData();
            _baseObjectsSpecialBonusHandler.InitHandling();
        }

        public void Load(Dictionary<string, object> data)
        {
            if (data.TryGetValue(nameof(PlayerBasesProgressionSaver), out var rawData))
            {
                var storageData = (JObject)rawData;
                var currentBaseIndex = (int)storageData[CURRENT_BASE_INDEX_KEY];
                var selectedBaseIndex = (int)storageData[SELECTED_BASE_INDEX_KEY];
                var allBasesData = (JObject)storageData[ALL_BASES_DATA_KEY];

                var basesSaveData = new List<BaseSaveData>();

                for (int i = 0; i < allBasesData.Count; i++)
                {
                    var @base = (JObject)allBasesData[BASE_KEY + i];
                    var baseSaveData = new BaseSaveData();

                    for (int k = 0; k < @base.Count - 1; k++)
                    {
                        var baseObject = (JObject)@base[BASE_OBJECT_KEY + k];
                        var baseObjectDescriptor = new BaseObjectSaveData();
                        baseObjectDescriptor.IsUnlocked = (bool)baseObject[IS_UNLOCKED_KEY];
                        baseObjectDescriptor.CurrentResourceAmount = (int)baseObject[RESOURCE_AMOUNT_KEY];
                        baseObjectDescriptor.ResourceGoal = (int)baseObject[RESOURCE_GOAL_KEY];
                        baseObjectDescriptor.BuildingLevel = (int)baseObject[BUILDING_LEVEL_KEY];
                        baseSaveData.ObjectsData.Add(baseObjectDescriptor);
                    }
                    var isLocked = (bool)@base[IS_LOCKED_KEY];
                    baseSaveData.IsLocked = isLocked;
                    basesSaveData.Add(baseSaveData);
                }

                CurrentBaseIndex = currentBaseIndex;
                SelectedBaseIndex = selectedBaseIndex;
                _manager.InitBases(_holder.PlayerBaseDescriptors, currentBaseIndex);
                _manager.LinkView(_managerView);
                _manager.InitLoadData(basesSaveData, selectedBaseIndex);
                _manager.InitBasesUpgradableData();
                _baseObjectsSpecialBonusHandler.InitHandling();
            }
        }

        public void Save(Dictionary<string, object> data)
        {
            var bases = _manager.Bases;
            var allBasesSaveData = new Dictionary<string, object>();

            for (int k = 0; k < bases.Count; k++)
            {
                var @base = bases[k];
                var baseObjects = @base.BaseObjectsDict;
                var baseSaveData = new Dictionary<string, object>();
                var isLocked = @base.IsLocked;
                if (!isLocked)
                {
                    int i = 0;
                    foreach (var keyValuePair in baseObjects)
                    {
                        foreach (var baseObject in keyValuePair.Value)
                        {
                            var baseObjectData = new Dictionary<string, object>()
                            {
                                [IS_UNLOCKED_KEY] = baseObject.IsUnlocked.Value,
                                [RESOURCE_AMOUNT_KEY] = baseObject.CurrencyAmount.Value,
                                [RESOURCE_GOAL_KEY] = baseObject.CurrencyToUnlock.Value,
                                [BUILDING_LEVEL_KEY] = baseObject.UpgradableBuilding.Level.Value,
                            };
                            baseSaveData.Add(BASE_OBJECT_KEY + i, baseObjectData);
                            i++;
                        }
                    }
                }
                baseSaveData.Add(IS_LOCKED_KEY, isLocked);
                allBasesSaveData.Add(BASE_KEY + k, baseSaveData);
            }

            var storageData = new Dictionary<string, object>()
            {
                [SELECTED_BASE_INDEX_KEY] = SelectedBaseIndex,
                [CURRENT_BASE_INDEX_KEY] = CurrentBaseIndex,
                [ALL_BASES_DATA_KEY] = allBasesSaveData
            };

            data.Add(nameof(PlayerBasesProgressionSaver), storageData);
        }

        public void SetCurrentBaseIndex(int index)
        {
            CurrentBaseIndex = index;
        }

        public void SetSelectedBaseIndex(int index)
        {
            SelectedBaseIndex = index;
        }
    }
}