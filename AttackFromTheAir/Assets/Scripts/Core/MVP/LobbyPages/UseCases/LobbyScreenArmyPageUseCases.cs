using Core.GameLogic;
using UnityEditor.Rendering;
using Zenject;

namespace Core.MVP
{
    public class LobbyScreenArmyPageUseCases : IPageUseCases
    {
        private IPlayerDroneProgressionService _playerDroneProgressionService;
        private IPlayerDroneSelector _playerDroneSelector;

        public void Init(DiContainer diContainer)
        {
            _playerDroneProgressionService = diContainer.Resolve<IPlayerDroneProgressionService>();
            _playerDroneSelector = diContainer.Resolve<IPlayerDroneSelector>();
        }

        public void OnSpeedSoftUpgradeButtonClick()
        {
            _playerDroneProgressionService.UpgradeStat(PlayerDroneProgressionService.DroneStats.Speed);
        }

        public void OnSpeedHardUpgradeButtonClick()
        {
            _playerDroneProgressionService.UpgradeStat(PlayerDroneProgressionService.DroneStats.Speed, true);
        }

        public void OnReloadSoftUpgradeButtonClick()
        {
            _playerDroneProgressionService.UpgradeStat(PlayerDroneProgressionService.DroneStats.Reload);
        }

        public void OnReloadHardUpgradeButtonClick()
        {
            _playerDroneProgressionService.UpgradeStat(PlayerDroneProgressionService.DroneStats.Reload, true);
        }

        public void OnArmorSoftUpgradeButtonClick()
        {
            _playerDroneProgressionService.UpgradeStat(PlayerDroneProgressionService.DroneStats.Armor);

        }

        public void OnArmorHardUpgradeButtonClick()
        {
            _playerDroneProgressionService.UpgradeStat(PlayerDroneProgressionService.DroneStats.Armor, true);
        }

        public void OnNextDroneButtonClick()
        {
            _playerDroneSelector.NextDrone();
        }

        public void OnPreviousDroneButtonClick()
        {
            _playerDroneSelector.PreviousDrone();
        }
    }
}