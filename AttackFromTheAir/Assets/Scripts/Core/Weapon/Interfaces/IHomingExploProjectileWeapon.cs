namespace Core.Weapon
{
    public interface IHomingExploProjectileWeapon : IWeapon
    {
        HomingExploProjectileWeaponType HomingExploProjectileWeaponType { get; }
    }
}