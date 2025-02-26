using Core.Buildings;
using Core.LobbyBase;
using Zenject;

namespace Core.MVP
{
    public class LobbyScreenBasePageUseCases : IPageUseCases
    {
        private PlayerBaseResourceSendingSystem _playerBaseUnlockSystem;
        private PlayerBaseUpgradeSystem _playerBaseUpgradeSystem;
        private BaseObjectSelector _baseObjectSelector;
        private PlayerBaseSwitcher _playerBaseSwitcher;

        public void Init(DiContainer diContainer)
        {
            _playerBaseUnlockSystem = diContainer.Resolve<PlayerBaseResourceSendingSystem>();
            _baseObjectSelector = diContainer.Resolve<BaseObjectSelector>();
            _playerBaseUpgradeSystem = diContainer.Resolve<PlayerBaseUpgradeSystem>();
            _playerBaseSwitcher = diContainer.Resolve<PlayerBaseSwitcher>();
        }

        public void OnBaseBuildingTabStateChanged(BuildingType baseObjectType, bool state)
        {
            if (state)
            {
                _baseObjectSelector.SelectBaseObject(baseObjectType);
            }
        }

        public void OnSendResourceButtonDown()
        {
            _playerBaseUnlockSystem.StartSendingResources();
        }

        public void OnSendResourceButtonUp()
        {
            _playerBaseUnlockSystem.StopSendingResources();
        }


        public void OnAdvLevelUpButtonClick()
        {

        }

        public void OnSoftCurrencyLevelUpButtonClick()
        {
            _playerBaseUpgradeSystem.UpgradeBySoftCurrency();
        }

        public void OnHardCurrencyLevelUpButtonClick()
        {
            _playerBaseUpgradeSystem.UpgradeByHardCurrency();
        }

        public void OnPreviousObjectButtonClick()
        {
            _baseObjectSelector.PrevSelectedObject();
        }

        public void OnNextObjectButtonClick()
        {
            _baseObjectSelector.NextSelectedObject();
        }

        public void OnSwitchBaseObserveModeButtonClick()
        {
            _playerBaseSwitcher.SwitchBaseObserveMode();
        }

        public void OnSwitchBaseToNext()
        {
            _playerBaseSwitcher.SwitchNext();
        }

        public void OnSwitchBaseToPrevious()
        {
            _playerBaseSwitcher.SwitchPrev();
        }
    }
}