namespace Core.Weapon
{
    public interface IBulletsWeapon : IWeapon 
    {
        BulletsWeaponType BulletsWeaponType { get; }
    }
}