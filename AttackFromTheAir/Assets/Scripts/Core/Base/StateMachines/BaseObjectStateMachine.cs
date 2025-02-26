using Core.Buildings;
using Core.States;
using Core.UI;
using Core.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace Core.LobbyBase
{
    public enum BaseObjectStates
    {
        Init,
        Locked,
        Unlocked
    }

    public class BaseObjectStateMachine : BaseStateMachine<BaseObjectStates>, IDisposable
    {
        private BaseObjectView _view;
        private IntProperty _currencyAmountToUnlock;
        private IntProperty _currentCurrencyAmount;
        private FloatProperty _unlockProgress;
        private BoolProperty _isObjectUnlocked;
        private BaseObjectSaveData _saveData;
        private IUpgradableBuilding _upgradableBuilding;
        private BaseObjectLockedState _baseObjectLockedState;
        private BaseObjectUnlockedState _baseObjectUnlockedState;
        public SimpleEvent ObjectUnlockedEvent { get; } = new SimpleEvent();
        public SimpleEvent LevelUpEvent { get; } = new SimpleEvent();   

        public BaseObjectStateMachine(BaseObjectView view, IntProperty currencyAmountToUnlock, IntProperty currentCurrencyAmount,
        FloatProperty unlockProgress, BoolProperty isObjectUnlocked, IUpgradableBuilding upgradableBuilding, BaseObjectSaveData saveData)
        {
            _view = view;
            _currencyAmountToUnlock = currencyAmountToUnlock;
            _currentCurrencyAmount = currentCurrencyAmount;
            _unlockProgress = unlockProgress;
            _isObjectUnlocked = isObjectUnlocked;
            _upgradableBuilding = upgradableBuilding;
            _saveData = saveData;
        }

        public void Init()
        {
            var initState = new BaseObjectInitState(_saveData, _view, _upgradableBuilding, _isObjectUnlocked, _currencyAmountToUnlock, _currentCurrencyAmount);
            _baseObjectLockedState = new BaseObjectLockedState(_view, _isObjectUnlocked, _currencyAmountToUnlock, _currentCurrencyAmount, _unlockProgress);
            _baseObjectUnlockedState = new BaseObjectUnlockedState(_view, _currencyAmountToUnlock, _currentCurrencyAmount, _unlockProgress);

            _baseObjectLockedState.ObjectUnlockedEvent.AddListener(OnObjectUnlocked);
            _baseObjectUnlockedState.LevelUpEvent.AddListener(OnLevelUp);

            InitiateStateMachine(initState, _baseObjectLockedState, _baseObjectUnlockedState);
        }

        private void OnLevelUp()
        {
            Debug.Log("OnLevelUp");
            LevelUpEvent.Notify();
        }

        private void OnObjectUnlocked()
        {
            Debug.Log("OnObjectUnlockedUp");
            ObjectUnlockedEvent.Notify();
        }

        public void InitResourceGoal(int targetAmount, int currentResources = 0)
        {
            _currencyAmountToUnlock.SetValue(targetAmount, false);
            _currentCurrencyAmount.SetValue(currentResources, false);
            AddResource(0);
        }

        public int AddResource(int amount)
        {
            switch (Current.State)
            {
                case BaseObjectStates.Locked:
                    return _baseObjectLockedState.AddResource(amount);
                case BaseObjectStates.Unlocked:
                    return _baseObjectUnlockedState.AddResource(amount);
                default:
                    throw new System.ArgumentException(nameof(amount));
            }
        }

        public void Dispose()
        {
            Current.Exit();
            _baseObjectLockedState.ObjectUnlockedEvent.RemoveAllListeners();
            _baseObjectUnlockedState.LevelUpEvent.RemoveAllListeners();
            ObjectUnlockedEvent.RemoveAllListeners();
            LevelUpEvent.RemoveAllListeners();
        }
    }
}