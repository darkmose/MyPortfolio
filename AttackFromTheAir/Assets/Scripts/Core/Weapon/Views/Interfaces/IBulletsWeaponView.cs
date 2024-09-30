namespace Core.Weapon
{
    public interface IBulletsWeaponView : IWeaponView
    {
        BulletsWeaponType BulletsWeaponType { get; }
    }
}