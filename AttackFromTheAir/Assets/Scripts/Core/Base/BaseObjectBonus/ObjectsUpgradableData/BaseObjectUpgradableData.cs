using Core.Buildings;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.LobbyBase
{
    public abstract class BaseObjectUpgradableData : IDisposable
    {
        public abstract BuildingType BuildingType { get; }
        private BaseObjectUpgradableDataConfiguration _baseObjectUpgradableDataConfiguration;
        private int BaseObjectHealth;
        private float RepairDuration;
        private BaseObject _baseObject;
        private BaseObjectBonusVocabulary _baseObjectBonusVocabulary;
        private int _currentLevel;
        protected int CurrentLevel => _currentLevel;
        protected BaseObjectBonusVocabulary BaseObjectBonusVocabulary => _baseObjectBonusVocabulary;
        protected BaseObjectUpgradableDataConfiguration BaseObjectUpgradableDataConfiguration => _baseObjectUpgradableDataConfiguration;

        protected BaseObjectUpgradableData(BaseObject baseObject)
        {
            _baseObject = baseObject;
            _baseObject.UpgradableBuilding.Level.RegisterValueChangeListener(OnBuildingLevelChanged);
            _baseObjectUpgradableDataConfiguration = Resources.Load<BaseObjectUpgradableDataConfiguration>("ScriptableObjects/"+nameof(BaseObjectUpgradableDataConfiguration));
        }

        private void OnBuildingLevelChanged(int level)
        {
            InitData(level);
        }

        public void InitData(int currentLevel)
        {
            _currentLevel = currentLevel;
            BaseObjectHealth = (int)_baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.Health, currentLevel);
            RepairDuration = _baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.RepairDuration, currentLevel);
            InitDataInner(_baseObjectUpgradableDataConfiguration, currentLevel);
        }

        public Dictionary<BaseObjectDataType, object> CalculateNextLevelData()
        {
            var dict = new Dictionary<BaseObjectDataType, object>();
            var health = (int)_baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.Health, _currentLevel + 1);
            var repairDuration = _baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.RepairDuration, _currentLevel + 1);
            dict.Add(BaseObjectDataType.Health, health);
            dict.Add(BaseObjectDataType.RepairDuration, repairDuration);    
            CalculateNextLevelDataInner(dict);

            return dict;
        }

        public void InitVocabulary(BaseObjectBonusVocabulary baseObjectBonusVocabulary)
        {
            _baseObjectBonusVocabulary = baseObjectBonusVocabulary;
        }

        public Dictionary<BaseObjectSpecialBonusType, object> CalculateNextLevelSpecialBonusData()
        {
            var dict = new Dictionary<BaseObjectSpecialBonusType, object>();
            CalculateNextLevelSpecialBonusDataInner(dict);
            return dict;
        }

        protected abstract void InitDataInner(BaseObjectUpgradableDataConfiguration baseObjectUpgradableDataConfiguration, int currentLevel);

        public Dictionary<BaseObjectDataType, object> GetData()
        {
            var dict = new Dictionary<BaseObjectDataType, object>();
            dict.Add(BaseObjectDataType.Health, BaseObjectHealth);
            dict.Add(BaseObjectDataType.RepairDuration, RepairDuration);
            AddInnerData(dict);
            return dict;
        }

        public Dictionary<BaseObjectSpecialBonusType, object> GetSpecialBonusData()
        {
            var dict = new Dictionary<BaseObjectSpecialBonusType, object>();
            AddInnerSpecialBonusData(dict);
            return dict;
        }

        protected abstract void CalculateNextLevelDataInner(Dictionary<BaseObjectDataType, object> data);
        protected abstract void CalculateNextLevelSpecialBonusDataInner(Dictionary<BaseObjectSpecialBonusType, object> data);
        protected abstract void AddInnerData(Dictionary<BaseObjectDataType, object> data);
        protected abstract void AddInnerSpecialBonusData(Dictionary<BaseObjectSpecialBonusType, object> data);

        public void Dispose()
        {
            _baseObject.UpgradableBuilding.Level.UnregisterValueChangeListener(OnBuildingLevelChanged);
        }
    }
}