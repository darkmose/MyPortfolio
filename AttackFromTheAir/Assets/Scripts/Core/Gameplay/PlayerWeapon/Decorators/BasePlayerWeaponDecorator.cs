using Core.Utilities;
using Core.Weapon;
using System;

namespace Core.GameLogic
{
    public interface IPlayerWeaponDecorator
    {
        SimpleEvent DataChangedEvent { get; }
        PlayerWeaponConfig GetWeaponConfig();
    }

    public abstract class BasePlayerWeaponDecorator : IPlayerWeaponDecorator
    {
        private SimpleEvent _dataChangedEvent = new SimpleEvent();
        private IPlayerWeaponDecorator _playerWeaponDecorator;
        protected IPlayerWeaponDecorator PlayerWeaponDecorator => _playerWeaponDecorator;
        public SimpleEvent DataChangedEvent => _dataChangedEvent;

        protected BasePlayerWeaponDecorator(IPlayerWeaponDecorator decorator)
        {
            _playerWeaponDecorator = decorator;
            _playerWeaponDecorator.DataChangedEvent.AddListener(OnDataChange);
        }

        private void OnDataChange()
        {
            RaiseDataChangedEvent();
        }

        public PlayerWeaponConfig GetWeaponConfig()
        {
            return GetWeaponConfigInner();
        }

        protected void RaiseDataChangedEvent()
        {
            _dataChangedEvent.Notify();
        }

        protected abstract PlayerWeaponConfig GetWeaponConfigInner();
    }
}