using static Core.GameLogic.PlayerDroneProgressionService;

namespace Core.GameLogic
{
    public class PlayerDroneUpgradeDecorator : BasePlayerDroneDecorator
    {
        private PlayerDroneConfig _config;
        private int _speedLevel;
        private int _reloadLevel;
        private int _armorLevel;

        public PlayerDroneUpgradeDecorator(IPlayerDroneDecorator decorator) : base(decorator)
        {
            _config = new PlayerDroneConfig();
        }

        protected override PlayerDroneConfig GetDroneConfigInner()
        {
            return _config;
        }

        public void SetConfig(PlayerDroneConfig config)
        {
            _config = config;
            RaiseDataChangedEvent();
        }

        public void AddSpeed(float speed)
        {
            _config.Speed += speed;
            _speedLevel++;
            RaiseDataChangedEvent();
        }

        public void AddReloadSpeed(float reloadSpeed)
        {
            _config.Reload += reloadSpeed;
            _reloadLevel++;
            RaiseDataChangedEvent();
        }

        public void AddArmor(float armor)
        {
            _config.Armor += armor;
            _armorLevel++;
            RaiseDataChangedEvent();
        }

        public int GetStatLevel(DroneStats droneStat)
        {
            switch (droneStat)
            {
                case DroneStats.Speed:
                    return _speedLevel;
                case DroneStats.Reload:
                    return _reloadLevel;
                case DroneStats.Armor:
                    return _armorLevel;
                default:
                    return 0;
            }
        }

        public void SetStatLevel(DroneStats droneStat, int level)
        {
            switch (droneStat)
            {
                case DroneStats.Speed:
                    _speedLevel = level;
                    break;
                case DroneStats.Reload:
                    _reloadLevel = level;
                    break;
                case DroneStats.Armor:
                    _armorLevel = level;
                    break;
            }
        }
    }
}