using Core.UI;
using Core.Utilities;

namespace Core.GameLogic
{
    public abstract class BasePlayerDroneDecorator : IPlayerDroneDecorator
    {
        private SimpleEvent _dataChangedEvent = new SimpleEvent();
        private IPlayerDroneDecorator _playerWeaponDecorator;
        protected IPlayerDroneDecorator PlayerWeaponDecorator => _playerWeaponDecorator;
        public SimpleEvent DataChangedEvent => _dataChangedEvent;

        protected BasePlayerDroneDecorator(IPlayerDroneDecorator decorator)
        {
            _playerWeaponDecorator = decorator;
            _playerWeaponDecorator.DataChangedEvent.AddListener(OnDataChange);
        }

        private void OnDataChange()
        {
            RaiseDataChangedEvent();
        }

        public PlayerDroneConfig GetDroneConfig()
        {
            return GetDroneConfigInner();
        }

        protected void RaiseDataChangedEvent()
        {
            _dataChangedEvent.Notify();
        }

        protected abstract PlayerDroneConfig GetDroneConfigInner();
    }
}