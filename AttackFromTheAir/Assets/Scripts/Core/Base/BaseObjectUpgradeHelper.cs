using Configuration;
using Core.UI;
using System;
using UnityEngine;

namespace Core.LobbyBase
{
    public class BaseObjectUpgradeHelper
    {
        private BoolProperty _isUnlocked = new BoolProperty();
        private IntProperty _currentResources = new IntProperty(0);
        private IntProperty _goalResources = new IntProperty(1);
        private IntProperty _buildingLevel = new IntProperty(0);
        private FloatProperty _resourcesProgress = new FloatProperty(); 
        public IPropertyReadOnly<bool> IsUnlocked => _isUnlocked;
        public IPropertyReadOnly<int> CurrentResources => _currentResources;
        public IPropertyReadOnly<int> GoalResources => _goalResources;
        public IPropertyReadOnly<int> BuildingLevel => _buildingLevel;
        public IPropertyReadOnly<float> ResourcesProgress => _resourcesProgress;

        private readonly BaseObjectSelector _baseObjectSelector;
        private readonly BaseObjectsUpgradeConfiguration _baseObjectsUpgradeConfiguration;
        private readonly PlayerBaseUpgradeSystem _playerBaseUpgradeSystem;
        private BaseObject _currentBaseObject;

        public BaseObjectUpgradeHelper(BaseObjectSelector baseObjectSelector, PlayerBaseUpgradeSystem playerBaseUpgradeSystem)
        {
            _baseObjectSelector = baseObjectSelector;
            var economicConfiguration = Resources.Load<EconomicConfiguration>("ScriptableObjects/" + nameof(EconomicConfiguration));
            _baseObjectsUpgradeConfiguration = economicConfiguration.BaseObjectsUpgradeConfiguration;
            _playerBaseUpgradeSystem = playerBaseUpgradeSystem;
            _baseObjectSelector.CurrentBaseObject.RegisterValueChangeListener(OnCurrentBaseObjectChanged);
        }

        private void OnCurrentBaseObjectChanged(BaseObject @object)
        {
            if (_currentBaseObject != null)
            {
                _currentBaseObject.CurrencyToUnlock.UnregisterValueChangeListener(OnCurrencyToUnlockChanged);
                _currentBaseObject.CurrencyAmount.UnregisterValueChangeListener(OnCurrentCurrencyAmountChanged);
                _currentBaseObject.IsUnlocked.UnregisterValueChangeListener(OnUnlockedStatusChanged);
                _currentBaseObject.UpgradableBuilding.Level.UnregisterValueChangeListener(OnBuildingLevelChanged);
                _currentBaseObject.UnlockProgress.UnregisterValueChangeListener(OnUnlockProgressChanged);
                _currentBaseObject.ObjectUnlockedEvent.RemoveListener(OnLevelUp);
                _currentBaseObject.LevelUpEvent.RemoveListener(OnLevelUp);
            }

            _currentBaseObject = @object;
            _currentBaseObject.CurrencyToUnlock.RegisterValueChangeListener(OnCurrencyToUnlockChanged);
            _currentBaseObject.CurrencyAmount.RegisterValueChangeListener(OnCurrentCurrencyAmountChanged);
            _currentBaseObject.IsUnlocked.RegisterValueChangeListener(OnUnlockedStatusChanged);
            _currentBaseObject.UpgradableBuilding.Level.RegisterValueChangeListener(OnBuildingLevelChanged);
            _currentBaseObject.UnlockProgress.RegisterValueChangeListener(OnUnlockProgressChanged);
            _currentBaseObject.ObjectUnlockedEvent.AddListener(OnLevelUp);
            _currentBaseObject.LevelUpEvent.AddListener(OnLevelUp);

            OnCurrencyToUnlockChanged(_currentBaseObject.CurrencyToUnlock.Value);
            OnCurrentCurrencyAmountChanged(_currentBaseObject.CurrencyAmount.Value);
            OnUnlockedStatusChanged(_currentBaseObject.IsUnlocked.Value);
            _buildingLevel.SetValue(_currentBaseObject.UpgradableBuilding.Level.Value, false);
            OnUnlockProgressChanged(_currentBaseObject.UnlockProgress.Value);
        }

        private void OnLevelUp()
        {
            _playerBaseUpgradeSystem.UpgradeCurrentBaseObject();
            RefreshNextLevelHammersCost();
        }

        private void RefreshNextLevelHammersCost()
        {
            var currentResources = _currentBaseObject.CurrencyAmount.Value;
            var nextLevel = _currentBaseObject.UpgradableBuilding.Level.Value + 1;
            var nextLevelHammersResourceGoal = _baseObjectsUpgradeConfiguration.ProvideHammersCostFor(_currentBaseObject.BaseObjectType, nextLevel);
            if (currentResources == _currentBaseObject.CurrencyToUnlock.Value)
            {
                _currentBaseObject.InitResourceGoal(nextLevelHammersResourceGoal, 0);
            }
            else
            {
                _currentBaseObject.InitResourceGoal(nextLevelHammersResourceGoal, currentResources);
            }
        }

        private void OnUnlockProgressChanged(float progress)
        {
            _resourcesProgress.SetValue(progress, false);
        }

        private void OnBuildingLevelChanged(int level)
        {
            _buildingLevel.SetValue(level, false);
            RefreshNextLevelHammersCost();
        }

        private void OnUnlockedStatusChanged(bool isUnlocked)
        {
            _isUnlocked.SetValue(isUnlocked);
        }

        private void OnCurrentCurrencyAmountChanged(int currency)
        {
            _currentResources.SetValue(currency, false);
        }

        private void OnCurrencyToUnlockChanged(int currency)
        {
            _goalResources.SetValue(currency, false);
        }
    }
}