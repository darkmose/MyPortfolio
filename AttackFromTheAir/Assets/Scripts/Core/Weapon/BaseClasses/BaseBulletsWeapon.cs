namespace Core.Weapon
{
    public abstract class BaseBulletsWeapon : BaseWeapon, IBulletsWeapon
    {
        public override WeaponProjectileType WeaponProjectileType => WeaponProjectileType.Bullets;
        public abstract BulletsWeaponType BulletsWeaponType { get; }

        protected BaseBulletsWeapon(WeaponConfig weaponConfig) : base(weaponConfig)
        {
        }
    }
}