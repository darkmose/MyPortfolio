using Core.Utilities;
using Core.Weapon;

namespace Core.GameLogic
{
    public class InitPlayerWeaponDecorator : IPlayerWeaponDecorator
    {
        private PlayerWeaponConfig _weaponConfig;
        public SimpleEvent DataChangedEvent { get; } = new SimpleEvent();

        public InitPlayerWeaponDecorator(PlayerWeaponConfig weaponConfig)
        {
            _weaponConfig = weaponConfig;
        }

        public PlayerWeaponConfig GetWeaponConfig()
        {
            return _weaponConfig;
        }
    }
}