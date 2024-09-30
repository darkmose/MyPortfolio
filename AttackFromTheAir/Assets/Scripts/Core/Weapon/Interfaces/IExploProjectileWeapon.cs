namespace Core.Weapon
{
    public interface IExploProjectileWeapon : IWeapon
    {
        ExploProjectileWeaponType ExploProjectileWeaponType { get; }
    }
}