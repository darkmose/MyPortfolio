namespace Core.Weapon
{
    public class Rifle : BaseBulletsWeapon
    {
        public override BulletsWeaponType BulletsWeaponType => BulletsWeaponType.Rifle;

        public Rifle(WeaponConfig weaponConfig) : base(weaponConfig)
        {
        }
    }
}