using Configuration;
using Core.PlayerModule;
using Core.UI;
using System;
using UnityEngine;

namespace Core.LobbyBase
{
    public class PlayerBaseUpgradeSystem
    {
        private BaseObjectSelector _baseObjectSelector;
        private BaseObjectsUpgradeConfiguration _upgradeConfiguration;
        private IWallet _wallet;

        private BoolProperty _isSoftCurrencyBuyButtonShown = new BoolProperty();
        private BoolProperty _isHardCurrencyBuyButtonShown = new BoolProperty();

        private IntProperty _softCurrencyLevelUpCost = new IntProperty(0);
        private IntProperty _hardCurrencyLevelUpCost = new IntProperty(0);

        private BoolProperty _isSoftCurrencyEnough = new BoolProperty();
        private BoolProperty _isHardCurrencyEnough = new BoolProperty();

        private CostDescriptor _currentSoftCurrencyCost;
        private CostDescriptor _currentHardCurrencyCost;

        private BaseObject _currentSelectedObject;

        public IPropertyReadOnly<bool> IsSoftCurrencyBuyButtonShown => _isSoftCurrencyBuyButtonShown;
        public IPropertyReadOnly<bool> IsHardCurrencyBuyButtonShown => _isHardCurrencyBuyButtonShown;

        public IPropertyReadOnly<int> SoftCurrencyLevelUpCost => _softCurrencyLevelUpCost;
        public IPropertyReadOnly<int> HardCurrencyLevelUpCost => _hardCurrencyLevelUpCost;

        public IPropertyReadOnly<bool> IsSoftCurrencyEnough => _isSoftCurrencyEnough;
        public IPropertyReadOnly<bool> IsHardCurrencyEnough => _isHardCurrencyEnough;


        public PlayerBaseUpgradeSystem(BaseObjectSelector baseObjectSelector, IWallet wallet)
        {
            _baseObjectSelector = baseObjectSelector;
            _wallet = wallet;
            var economicConfiguration = Resources.Load<EconomicConfiguration>("ScriptableObjects/"+nameof(EconomicConfiguration));
            _upgradeConfiguration = economicConfiguration.BaseObjectsUpgradeConfiguration;
            _baseObjectSelector.CurrentBaseObject.RegisterValueChangeListener(OnCurrentBaseObjectChanged);
        }

        private void OnCurrentBaseObjectChanged(BaseObject @object)
        {
            _currentSelectedObject?.IsUnlocked.UnregisterValueChangeListener(OnCurrentObjectUnlockedStatusChanged);
            _currentSelectedObject = @object;
            if (!@object.IsUnlocked.Value)
            {
                SetActiveBuyButtons(false);
                _currentSelectedObject.IsUnlocked.RegisterValueChangeListener(OnCurrentObjectUnlockedStatusChanged);
            }
            else
            {
                SetActiveBuyButtons(true);
                RefreshLevelUpCosts();
            }
        }

        private void OnCurrentObjectUnlockedStatusChanged(bool status)
        {
            if (status)
            {
                _currentSelectedObject.IsUnlocked.UnregisterValueChangeListener(OnCurrentObjectUnlockedStatusChanged);
                SetActiveBuyButtons(true);
                RefreshLevelUpCosts();
            }
        }

        private void SetActiveBuyButtons(bool isActive)
        {
            _isSoftCurrencyBuyButtonShown.SetValue(isActive);
            _isHardCurrencyBuyButtonShown.SetValue(isActive);
        }

        private void RefreshLevelUpCosts()
        {
            var currentBaseObject = _baseObjectSelector.CurrentBaseObject.Value;
            var baseObjectBuilding = currentBaseObject.UpgradableBuilding;
            var currentBuildingLevel = baseObjectBuilding.Level.Value;
            var buildingType = baseObjectBuilding.BuildingType;
            var nextLevel = currentBuildingLevel + 1;

            _currentSoftCurrencyCost = _upgradeConfiguration.ProvideSoftCurrencyCostFor(buildingType, nextLevel);
            _currentHardCurrencyCost = _upgradeConfiguration.ProvideHardCurrencyCostFor(buildingType, nextLevel);

            _softCurrencyLevelUpCost.SetValue(_currentSoftCurrencyCost.Amount, false);
            _hardCurrencyLevelUpCost.SetValue(_currentHardCurrencyCost.Amount, false);

            CheckCurrencyEnough();
        }

        private void CheckCurrencyEnough()
        {
            var isSoftCurrencyEnough = _wallet.HasEnoughMoney(_currentSoftCurrencyCost.MoneyType, _currentSoftCurrencyCost.Amount);
            var isHardCurrencyEnough = _wallet.HasEnoughMoney(_currentHardCurrencyCost.MoneyType, _currentHardCurrencyCost.Amount);
            _isSoftCurrencyEnough.SetValue(isSoftCurrencyEnough);
            _isHardCurrencyEnough.SetValue(isHardCurrencyEnough);
        }

        private void Upgrade()
        {
            var currentBaseObject = _baseObjectSelector.CurrentBaseObject.Value;
            currentBaseObject.UpgradeBuilding();
        }

        public void UpgradeBySoftCurrency()
        {
            if (_wallet.TryToSpend(_currentSoftCurrencyCost.MoneyType, _currentSoftCurrencyCost.Amount))
            {
                Upgrade();
                RefreshLevelUpCosts();
            }
        }

        public void UpgradeByHardCurrency()
        {
            if (_wallet.TryToSpend(_currentHardCurrencyCost.MoneyType, _currentHardCurrencyCost.Amount))
            {
                Upgrade();
                RefreshLevelUpCosts();
            }
        }

        public void UpgradeCurrentBaseObject()
        {
            //For hammers currency and advertisement
            Upgrade();
            RefreshLevelUpCosts();
        }
    }
}