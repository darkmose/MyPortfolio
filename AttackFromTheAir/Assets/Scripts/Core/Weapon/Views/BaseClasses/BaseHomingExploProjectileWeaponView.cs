namespace Core.Weapon
{
    public abstract class BaseHomingExploProjectileWeaponView : BaseWeaponView, IHomingExploProjectileWeaponView
    {
        public override WeaponProjectileType WeaponProjectileType => WeaponProjectileType.HomingExplosiveProjectiles;
        public abstract HomingExploProjectileWeaponType HomingExploProjectileWeaponType { get; }
    }
}