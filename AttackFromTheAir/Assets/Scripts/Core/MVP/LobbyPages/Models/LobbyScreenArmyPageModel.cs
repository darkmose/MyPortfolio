using Core.GameLogic;
using Core.PlayerModule;
using Core.UI;
using Zenject;

namespace Core.MVP
{
    public class LobbyScreenArmyPageModel : IPageModel
    {
        public IPropertyReadOnly<string> DroneName { get; private set; }
        public IPropertyReadOnly<int> DroneLevel { get; private set; }
        public IPropertyReadOnly<float> DronePower { get; private set; }

        public IPropertyReadOnly<float> SpeedValue { get; private set; }
        public IPropertyReadOnly<float> ReloadValue { get; private set; }
        public IPropertyReadOnly<float> ArmorValue { get; private set; }

        public IPropertyReadOnly<int> SpeedSoftCost { get; private set; }
        public IPropertyReadOnly<int> ReloadSoftCost { get; private set; }
        public IPropertyReadOnly<int> ArmorSoftCost { get; private set; }
        public IPropertyReadOnly<int> SpeedHardCost { get; private set; }
        public IPropertyReadOnly<int> ReloadHardCost { get; private set; }
        public IPropertyReadOnly<int> ArmorHardCost { get; private set; }

        public IPropertyReadOnly<bool> EnoughSoftCurrencyForSpeed { get; private set; }
        public IPropertyReadOnly<bool> EnoughSoftCurrencyForReload { get; private set; }
        public IPropertyReadOnly<bool> EnoughSoftCurrencyForArmor { get; private set; }
        public IPropertyReadOnly<bool> EnoughHardCurrencyForSpeed { get; private set; }
        public IPropertyReadOnly<bool> EnoughHardCurrencyForReload { get; private set; }
        public IPropertyReadOnly<bool> EnoughHardCurrencyForArmor { get; private set; }

        public IPropertyReadOnly<MoneyType> SpeedSoftCurrency { get; private set; }
        public IPropertyReadOnly<MoneyType> ReloadSoftCurrency { get; private set; }
        public IPropertyReadOnly<MoneyType> ArmorSoftCurrency { get; private set; }
        public IPropertyReadOnly<MoneyType> SpeedHardCurrency { get; private set; }
        public IPropertyReadOnly<MoneyType> ReloadHardCurrency { get; private set; }
        public IPropertyReadOnly<MoneyType> ArmorHardCurrency { get; private set; }

        public IPropertyReadOnly<bool> IsCurrentDroneAvailable { get; private set; }
        public IPropertyReadOnly<int> DronePlayerLevelRequired { get; private set; }

        public void Init(DiContainer diContainer)
        {
            var playerDroneProgressionService = diContainer.Resolve<IPlayerDroneProgressionService>();
            var playerDroneSelector = diContainer.Resolve<IPlayerDroneSelector>();
            
            DroneName = playerDroneProgressionService.DroneName;
            DroneLevel = playerDroneProgressionService.DroneLevel;
            DronePower = playerDroneProgressionService.DronePower;
            SpeedValue = playerDroneProgressionService.SpeedValue;
            ReloadValue = playerDroneProgressionService.ReloadValue;
            ArmorValue = playerDroneProgressionService.ArmorValue;
            SpeedSoftCost = playerDroneProgressionService.SpeedSoftCost;
            ReloadSoftCost = playerDroneProgressionService.ReloadSoftCost;
            ArmorSoftCost = playerDroneProgressionService.ArmorSoftCost;
            SpeedHardCost = playerDroneProgressionService.SpeedHardCost;
            ReloadHardCost = playerDroneProgressionService.ReloadHardCost;
            ArmorHardCost = playerDroneProgressionService.ArmorHardCost;
            EnoughSoftCurrencyForSpeed = playerDroneProgressionService.EnoughSoftCurrencyForSpeed;
            EnoughSoftCurrencyForReload = playerDroneProgressionService.EnoughSoftCurrencyForReload;
            EnoughSoftCurrencyForArmor = playerDroneProgressionService.EnoughSoftCurrencyForArmor;
            EnoughHardCurrencyForSpeed = playerDroneProgressionService.EnoughHardCurrencyForSpeed;
            EnoughHardCurrencyForReload = playerDroneProgressionService.EnoughHardCurrencyForReload;
            EnoughHardCurrencyForArmor = playerDroneProgressionService.EnoughHardCurrencyForArmor;
            SpeedSoftCurrency = playerDroneProgressionService.SpeedSoftCurrency;
            ReloadSoftCurrency = playerDroneProgressionService.ReloadSoftCurrency;
            ArmorSoftCurrency = playerDroneProgressionService.ArmorSoftCurrency;
            SpeedHardCurrency = playerDroneProgressionService.SpeedHardCurrency;
            ReloadHardCurrency = playerDroneProgressionService.ReloadHardCurrency;
            ArmorHardCurrency = playerDroneProgressionService.ArmorHardCurrency;
            IsCurrentDroneAvailable = playerDroneSelector.IsCurrentDroneAvailable;
            DronePlayerLevelRequired = playerDroneSelector.CurrentDroneRequiredPlayerLevel;
        }
    }
}