using Core.Buildings;
using Core.UI;
using Core.Utilities;
using System;

namespace Core.LobbyBase
{
    public class BaseObject : IDisposable
    {
        private BaseObjectView _view;

        private IntProperty _currencyAmountToUnlock = new IntProperty(1);
        private IntProperty _currentCurrencyAmount = new IntProperty(0);
        private FloatProperty _unlockProgress = new FloatProperty();
        private BoolProperty _isObjectUnlocked = new BoolProperty();
        private BaseObjectStateMachine _stateMachine;
        private IUpgradableBuilding _upgradableBuilding;
        private BaseObjectUpgradableData _objectUpgradableData;
        public IPropertyReadOnly<float> UnlockProgress => _unlockProgress;
        public BuildingType BaseObjectType { get; private set; }
        public IPropertyReadOnly<int> CurrencyAmount => _currentCurrencyAmount;
        public IPropertyReadOnly<int> CurrencyToUnlock => _currencyAmountToUnlock;
        public IUpgradableBuilding UpgradableBuilding => _upgradableBuilding;
        public IPropertyReadOnly<bool> IsUnlocked => _isObjectUnlocked;
        public BaseObjectView View => _view;
        public SimpleEvent ObjectUnlockedEvent { get; } = new SimpleEvent();
        public SimpleEvent LevelUpEvent { get; } = new SimpleEvent();
        public BaseObjectUpgradableData BaseObjectUpgradableData => _objectUpgradableData;

        public void LinkView(BaseObjectView view, BuildingCreateSystem buildingCreateSystem)
        {
            _view = view;
            _upgradableBuilding = (IUpgradableBuilding)buildingCreateSystem.GetModelForView(view.UpgradableBuildingView, true);
            _view.LinkModel(this);
        }

        public void InitLoadData(BaseObjectSaveData saveData)
        {
            _stateMachine = new BaseObjectStateMachine(_view, _currencyAmountToUnlock, _currentCurrencyAmount, 
                _unlockProgress, _isObjectUnlocked, _upgradableBuilding, saveData);
            _stateMachine.Init();
            _stateMachine.SwitchToState(BaseObjectStates.Init);

            _stateMachine.ObjectUnlockedEvent.AddListener(OnObjectUnlocked);
            _stateMachine.LevelUpEvent.AddListener(OnLevelUp);
        }

        public void InitUpgradableData(BaseObjectUpgradableData upgradableData)
        {
            _objectUpgradableData = upgradableData;
        }

        private void OnLevelUp()
        {
            LevelUpEvent.Notify();
        }

        public void UpgradeBuilding()
        {
            _view.AnimateObjectLevelUp();
            _upgradableBuilding.Upgrade();
        }

        private void OnObjectUnlocked()
        {
            ObjectUnlockedEvent.Notify();
        }

        public void InitResourceGoal(int targetAmount, int currentResources = 0)
        {
            _stateMachine.InitResourceGoal(targetAmount, currentResources);
        }

        public int AddResource(int amount)
        {
            return _stateMachine.AddResource(amount);
        }

        public void InitObjectDescriptor(BaseObjectDescriptor descriptor)
        {
            BaseObjectType = descriptor.BuildingType;
        }

        public void Dispose()
        {
            _unlockProgress.RemoveAllListeners();
            _objectUpgradableData.Dispose();
            _view?.Dispose();
        }
    }
}