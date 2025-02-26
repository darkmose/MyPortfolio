using Core.LobbyBase;
using Core.UI;
using UnityEngine;
using Zenject;

namespace Core.MVP
{
    public class LobbyScreenBasePageModel : IPageModel
    {
        private BaseObjectInfoAggregator _infoAggregator;
        private PlayerBaseUnlockHelper _playerBaseUnlockHelper;
        private PlayerBaseSwitcher _playerBaseSwitcher;
        public IPropertyReadOnly<int> CurrentObjectResources { get; private set; }
        public IPropertyReadOnly<int> GoalObjectResources { get; private set; }
        public IPropertyReadOnly<float> UpgradeProgress { get; private set; }
        public IPropertyReadOnly<bool> IsObjectUnlocked { get; private set; }
        public IPropertyReadOnly<int> ObjectLevel { get; private set; }
        public IPropertyReadOnly<int> SameObjectsCount { get; private set; }

        public IPropertyReadOnly<int> SoftCurrencyToUpgrade { get; private set; }
        public IPropertyReadOnly<int> HardCurrencyToUpgrade { get; private set; }

        public IPropertyReadOnly<bool> IsSoftCurrencyLevelUpButtonShown { get; private set; }
        public IPropertyReadOnly<bool> IsHardCurrencyLevelUpButtonShown { get; private set; }

        public IPropertyReadOnly<bool> IsEnoughSoftCurrency { get; private set; }
        public IPropertyReadOnly<bool> IsEnoughHardCurrency { get; private set; }

        public IPropertyReadOnly<Sprite> BuildingIcon { get; private set; }

        public IPropertyReadOnly<float> BaseUnlockProgress { get; private set; }
        public IPropertyReadOnly<string> BaseName { get; private set; } 

        public IPropertyReadOnly<bool> BaseObserveMode { get; private set; }
        public IPropertyReadOnly<bool> CanSwitchNextBase { get; private set; }
        public IPropertyReadOnly<bool> CanSwitchPreviousBase { get; private set; }


        public void Init(DiContainer diContainer)
        {
            _infoAggregator = diContainer.Resolve<BaseObjectInfoAggregator>();
            _playerBaseUnlockHelper = diContainer.Resolve<PlayerBaseUnlockHelper>();
            _playerBaseSwitcher = diContainer.Resolve<PlayerBaseSwitcher>();

            CurrentObjectResources = _infoAggregator.CurrentObjectResources;
            GoalObjectResources = _infoAggregator.GoalObjectResources;
            UpgradeProgress = _infoAggregator.UpgradeProgress;
            IsObjectUnlocked = _infoAggregator.IsObjectUnlocked;
            ObjectLevel = _infoAggregator.ObjectLevel;
            SameObjectsCount = _infoAggregator.SameObjectsCount;

            SoftCurrencyToUpgrade = _infoAggregator.SoftCurrencyToUpgrade;
            HardCurrencyToUpgrade = _infoAggregator.HardCurrencyToUpgrade;

            IsSoftCurrencyLevelUpButtonShown = _infoAggregator.IsSoftCurrencyLevelUpButtonShown;
            IsHardCurrencyLevelUpButtonShown = _infoAggregator.IsHardCurrencyLevelUpButtonShown;

            IsEnoughSoftCurrency = _infoAggregator.IsEnoughSoftCurrency;
            IsEnoughHardCurrency = _infoAggregator.IsEnoughHardCurrency;

            BuildingIcon = _infoAggregator.CurrentBuildingIcon;

            BaseUnlockProgress = _playerBaseUnlockHelper.UnlockProgress;
            BaseName = _playerBaseUnlockHelper.BaseName;

            BaseObserveMode = _playerBaseSwitcher.BasesObserveMode;
            CanSwitchNextBase = _playerBaseSwitcher.CanSwitchNext;
            CanSwitchPreviousBase = _playerBaseSwitcher.CanSwitchPrevious;
        }
    }
}