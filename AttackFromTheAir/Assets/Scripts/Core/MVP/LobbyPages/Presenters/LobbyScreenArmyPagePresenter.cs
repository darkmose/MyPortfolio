using Core.GameLogic;
using System;
using Zenject;

namespace Core.MVP
{
    public class LobbyScreenArmyPagePresenter : BasePagePresenterModelUseCases<LobbyScreenArmyPageView, LobbyScreenArmyPageModel, LobbyScreenArmyPageUseCases>
    {
        private IPlayerWeaponSelector _playerWeaponSelector;
        private DroneContainerView _droneContainerView;

        public LobbyScreenArmyPagePresenter(DiContainer diContainer) : base(diContainer)
        {
            _playerWeaponSelector = diContainer.Resolve<IPlayerWeaponSelector>();
            _droneContainerView = diContainer.Resolve<DroneContainerView>();
        }

        protected override void InitInner(LobbyScreenArmyPageView view, LobbyScreenArmyPageModel model, LobbyScreenArmyPageUseCases useCases)
        {
            _playerWeaponSelector.LinkView(view.PlayerWeaponSelectorView);
            SubscribeDataChange();
        }

        protected override void OnShowPage()
        {
            base.OnShowPage();
            _droneContainerView.Enable();
            RefreshData();
        }

        protected override void OnHidePage()
        {
            base.OnHidePage();
            _droneContainerView.Disable();
        }

        private void SubscribeDataChange()
        {
            Model.DroneName.RegisterValueChangeListener(View.SetDroneName);
            Model.DroneLevel.RegisterValueChangeListener(View.SetDroneLevel);
            Model.DronePower.RegisterValueChangeListener(View.SetDronePower);

            Model.SpeedValue.RegisterValueChangeListener(View.SetSpeedValue);
            Model.ReloadValue.RegisterValueChangeListener(View.SetReloadValue);
            Model.ArmorValue.RegisterValueChangeListener(View.SetArmorValue);

            Model.SpeedSoftCurrency.RegisterValueChangeListener(View.SetSpeedSoftCurrency);
            Model.ReloadSoftCurrency.RegisterValueChangeListener(View.SetReloadSoftCurrency);
            Model.ArmorSoftCurrency.RegisterValueChangeListener(View.SetArmorSoftCurrency);
            Model.SpeedHardCurrency.RegisterValueChangeListener(View.SetSpeedHardCurrency);
            Model.ReloadHardCurrency.RegisterValueChangeListener(View.SetReloadHardCurrency);
            Model.ArmorHardCurrency.RegisterValueChangeListener(View.SetArmorHardCurrency);

            Model.SpeedSoftCost.RegisterValueChangeListener(View.SetSpeedSoftCost);
            Model.ReloadSoftCost.RegisterValueChangeListener(View.SetReloadSpeedSoftCost);
            Model.ArmorSoftCost.RegisterValueChangeListener(View.SetArmorSoftCost);
            Model.SpeedHardCost.RegisterValueChangeListener(View.SetSpeedHardCost);
            Model.ReloadHardCost.RegisterValueChangeListener(View.SetReloadSpeedHardCost);
            Model.ArmorHardCost.RegisterValueChangeListener(View.SetArmorHardCost);

            Model.EnoughSoftCurrencyForSpeed.RegisterValueChangeListener(View.SetInteractableSpeedSoftUpgradeButton);
            Model.EnoughSoftCurrencyForReload.RegisterValueChangeListener(View.SetInteractableReloadSoftUpgradeButton);
            Model.EnoughSoftCurrencyForArmor.RegisterValueChangeListener(View.SetInteractableArmorSoftUpgradeButton);
            Model.EnoughHardCurrencyForSpeed.RegisterValueChangeListener(View.SetInteractableSpeedHardUpgradeButton);
            Model.EnoughHardCurrencyForReload.RegisterValueChangeListener(View.SetInteractableReloadHardUpgradeButton);
            Model.EnoughHardCurrencyForArmor.RegisterValueChangeListener(View.SetInteractableArmorHardUpgradeButton);

            Model.IsCurrentDroneAvailable.RegisterValueChangeListener(View.SetVisibleAvailableDronePanel);
            Model.DronePlayerLevelRequired.RegisterValueChangeListener(View.SetDroneRequiredLevel);
        }

        private void RefreshData()
        {
            View.SetDroneName(Model.DroneName.Value);
            View.SetDroneLevel(Model.DroneLevel.Value);
            View.SetDronePower(Model.DronePower.Value);

            View.SetSpeedValue(Model.SpeedValue.Value);
            View.SetReloadValue(Model.ReloadValue.Value);
            View.SetArmorValue(Model.ArmorValue.Value);

            View.SetSpeedSoftCurrency(Model.SpeedSoftCurrency.Value);
            View.SetReloadSoftCurrency(Model.ReloadSoftCurrency.Value);
            View.SetArmorSoftCurrency(Model.ArmorSoftCurrency.Value);
            View.SetSpeedHardCurrency(Model.SpeedHardCurrency.Value);
            View.SetReloadHardCurrency(Model.ReloadHardCurrency.Value);
            View.SetArmorHardCurrency(Model.ArmorHardCurrency.Value);

            View.SetSpeedSoftCost(Model.SpeedSoftCost.Value);
            View.SetReloadSpeedSoftCost(Model.ReloadSoftCost.Value);
            View.SetArmorSoftCost(Model.ArmorSoftCost.Value);
            View.SetSpeedHardCost(Model.SpeedHardCost.Value);
            View.SetReloadSpeedHardCost(Model.ReloadHardCost.Value);
            View.SetArmorHardCost(Model.ArmorHardCost.Value);

            View.SetVisibleAvailableDronePanel(Model.IsCurrentDroneAvailable.Value);
            View.SetDroneRequiredLevel(Model.DronePlayerLevelRequired.Value);
        }

        public void OnDroneUpgradeButtonClick()
        {
            View.SetUpgradePanelVisible(true);
        }

        public void OnDroneUpgradePanelCloseButtonClick()
        {
            View.SetUpgradePanelVisible(false);
        }
    }
}