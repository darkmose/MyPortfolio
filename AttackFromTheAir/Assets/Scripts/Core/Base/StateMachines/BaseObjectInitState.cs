using Configuration;
using Core.Buildings;
using Core.States;
using Core.UI;
using Core.Utilities;
using UnityEngine;

namespace Core.LobbyBase
{
    public class BaseObjectInitState : BaseState<BaseObjectStates>
    {
        private readonly BaseObjectSaveData _saveData;
        private readonly IUpgradableBuilding _upgradableBuilding;
        private readonly BaseObjectView _view;
        private readonly BoolProperty _isObjectUnlocked;
        private readonly IntProperty _resourcesGoal;
        private readonly IntProperty _currentResources;
        private readonly BaseObjectsUpgradeConfiguration _upgradeConfiguration;
        public override BaseObjectStates State => BaseObjectStates.Init;

        public BaseObjectInitState(BaseObjectSaveData saveData, BaseObjectView view, IUpgradableBuilding upgradableBuilding, BoolProperty isObjectUnlocked, IntProperty currencyAmountToUnlock, IntProperty currentCurrencyAmount)
        {
            _saveData = saveData;
            _view = view;
            _upgradableBuilding = upgradableBuilding;
            _isObjectUnlocked = isObjectUnlocked;
            _resourcesGoal = currencyAmountToUnlock;
            _currentResources = currentCurrencyAmount;

            EconomicConfiguration economicConfiguration = Resources.Load<EconomicConfiguration>("ScriptableObjects/" + nameof(EconomicConfiguration));
            _upgradeConfiguration = economicConfiguration.BaseObjectsUpgradeConfiguration;
        }

        public override void Enter()
        {
            _upgradableBuilding.SetLevel(_saveData.BuildingLevel);
            _currentResources.SetValue(_saveData.CurrentResourceAmount, false);
            RefreshResourceGoal(_saveData.IsUnlocked);
            RefreshProgress();
            if (_saveData.IsUnlocked)
            {
                _isObjectUnlocked.SetValue(true);
                stateMachine.SwitchToState(BaseObjectStates.Unlocked);
            }
            else
            {
                stateMachine.SwitchToState(BaseObjectStates.Locked);
            }
        }

        private void RefreshResourceGoal(bool isUnlocked)
        {
            if (isUnlocked)
            {
                if (_saveData.ResourceGoal == 0)
                {
                    var hammerCurrency = _upgradeConfiguration.ProvideHammersCostFor(_view.ObjectDescriptor.BuildingType, _upgradableBuilding.Level.Value + 1);
                    _resourcesGoal.SetValue(hammerCurrency, false);
                }
                else
                {
                    _resourcesGoal.SetValue(_saveData.ResourceGoal, false);
                }
            }
            else
            {
                if (_saveData.ResourceGoal == 0)
                {
                    var startHammerCurrency = _upgradeConfiguration.ProvideStartHammerCurrencyCost(_view.ObjectDescriptor.BuildingType);
                    _resourcesGoal.SetValue(startHammerCurrency, false);
                }
                else
                {
                    _resourcesGoal.SetValue(_saveData.ResourceGoal, false);
                }
            }
        }

        private void RefreshProgress()
        {
            float progress = 0f;
            if (_resourcesGoal.Value > 0)
            {
                progress = (float)_currentResources.Value / (float)_resourcesGoal.Value;
            }
            _view.SetProgress(progress);
        }

        public override void Exit()
        {
        }
    }
}