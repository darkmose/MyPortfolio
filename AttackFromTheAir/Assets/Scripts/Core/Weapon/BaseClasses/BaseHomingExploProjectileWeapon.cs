namespace Core.Weapon
{
    public abstract class BaseHomingExploProjectileWeapon : BaseWeapon, IHomingExploProjectileWeapon
    {
        public override WeaponProjectileType WeaponProjectileType => WeaponProjectileType.HomingExplosiveProjectiles;
        public abstract HomingExploProjectileWeaponType HomingExploProjectileWeaponType { get; }

        protected BaseHomingExploProjectileWeapon(WeaponConfig weaponConfig) : base(weaponConfig)
        {
        }
    }
}