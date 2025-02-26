using Core.Utilities;

namespace Core.GameLogic
{
    public class InitPlayerDroneDecorator : IPlayerDroneDecorator
    {
        private PlayerDroneConfig _playerDroneConfig;
        private SimpleEvent _dataChangedEvent = new SimpleEvent();
        public SimpleEvent DataChangedEvent => _dataChangedEvent;

        public InitPlayerDroneDecorator(PlayerDroneConfig initPlayerDroneConfig)
        {
            _playerDroneConfig = initPlayerDroneConfig;
        }

        public PlayerDroneConfig GetDroneConfig()
        {
            return _playerDroneConfig;
        }
    }
}