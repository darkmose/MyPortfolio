namespace Core.Weapon
{
    public abstract class BaseExploProjectileWeapon : BaseWeapon, IExploProjectileWeapon
    {
        public override WeaponProjectileType WeaponProjectileType => WeaponProjectileType.ExplosiveProjectiles;
        public abstract ExploProjectileWeaponType ExploProjectileWeaponType { get; }

        protected BaseExploProjectileWeapon(WeaponConfig weaponConfig) : base(weaponConfig)
        {
        }
    }
}