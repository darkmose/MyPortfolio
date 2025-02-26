using Core.Resourses;
using Core.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.LobbyBase
{
    public class BaseObjectInfoAggregator
    {
        private IntProperty _sameObjectsCount = new IntProperty(0);
        private CustomProperty<Sprite> _currentBuildingIcon = new CustomProperty<Sprite>(null);
        public IPropertyReadOnly<int> CurrentObjectResources { get; }
        public IPropertyReadOnly<int> GoalObjectResources { get; }
        public IPropertyReadOnly<float> UpgradeProgress { get; }
        public IPropertyReadOnly<bool> IsObjectUnlocked { get; }
        public IPropertyReadOnly<int> ObjectLevel { get; }
        public IPropertyReadOnly<int> SameObjectsCount => _sameObjectsCount;

        public IPropertyReadOnly<float> RepairDuration;

        public IPropertyReadOnly<int> SoftCurrencyToUpgrade { get; }
        public IPropertyReadOnly<int> HardCurrencyToUpgrade { get; }

        public IPropertyReadOnly<bool> IsAdvLevelUpButtonShown { get; }
        public IPropertyReadOnly<bool> IsSoftCurrencyLevelUpButtonShown { get; }
        public IPropertyReadOnly<bool> IsHardCurrencyLevelUpButtonShown { get; }

        public IPropertyReadOnly<bool> IsAdvAvailable { get; }
        public IPropertyReadOnly<bool> IsEnoughSoftCurrency { get; }
        public IPropertyReadOnly<bool> IsEnoughHardCurrency { get; }

        public IPropertyReadOnly<Sprite> CurrentBuildingIcon => _currentBuildingIcon;

        private PlayerBaseUpgradeSystem _playerBaseUpgradeSystem;
        private BaseObjectSelector _baseObjectSelector;
        private BaseObject _currentSelectedObject;
        private BaseObjectUpgradeHelper _baseObjectUpgradeHelper;
        private BuildingSpriteProvider _buildingSpriteProvider;

        public BaseObjectInfoAggregator(BaseObjectSelector baseObjectSelector, PlayerBaseUpgradeSystem playerBaseUpgradeSystem, BaseObjectUpgradeHelper baseObjectUpgradeHelper)
        {
            _baseObjectSelector = baseObjectSelector;
            _playerBaseUpgradeSystem = playerBaseUpgradeSystem;

            SoftCurrencyToUpgrade = _playerBaseUpgradeSystem.SoftCurrencyLevelUpCost;
            HardCurrencyToUpgrade = _playerBaseUpgradeSystem.HardCurrencyLevelUpCost;
            IsSoftCurrencyLevelUpButtonShown = _playerBaseUpgradeSystem.IsSoftCurrencyBuyButtonShown;
            IsHardCurrencyLevelUpButtonShown = _playerBaseUpgradeSystem.IsHardCurrencyBuyButtonShown;
            IsEnoughSoftCurrency = _playerBaseUpgradeSystem.IsSoftCurrencyEnough;
            IsEnoughHardCurrency = _playerBaseUpgradeSystem.IsHardCurrencyEnough;

            _baseObjectSelector.SelectedObjects.RegisterValueChangeListener(OnSameObjectsListChanged);
            _baseObjectSelector.CurrentBaseObject.RegisterValueChangeListener(OnCurrentBaseObjectChanged);
            _baseObjectUpgradeHelper = baseObjectUpgradeHelper;

            CurrentObjectResources = _baseObjectUpgradeHelper.CurrentResources;
            GoalObjectResources = _baseObjectUpgradeHelper.GoalResources;
            UpgradeProgress = _baseObjectUpgradeHelper.ResourcesProgress;
            IsObjectUnlocked = _baseObjectUpgradeHelper.IsUnlocked;
            ObjectLevel = _baseObjectUpgradeHelper.BuildingLevel;

            _buildingSpriteProvider = Resources.Load<BuildingSpriteProvider>("ScriptableObjects/" + nameof(BuildingSpriteProvider));
        }

        private void OnCurrentBaseObjectChanged(BaseObject @object)
        {
            var buildingType = @object.BaseObjectType;
            var buildingSprite = _buildingSpriteProvider.ProvideByType(buildingType);
            _currentBuildingIcon.SetValue(buildingSprite, false);
        }

        private void OnSameObjectsListChanged(List<BaseObject> list)
        {
            _sameObjectsCount.SetValue(list.Count, false);
        }
    }
}