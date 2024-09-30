namespace Core.Weapon
{
    public abstract class BaseExploProjectileWeaponView : BaseWeaponView, IExploProjectileWeaponView
    {
        public override WeaponProjectileType WeaponProjectileType => WeaponProjectileType.ExplosiveProjectiles;
        public abstract ExploProjectileWeaponType ExploProjectileWeaponType { get; }
    }
}