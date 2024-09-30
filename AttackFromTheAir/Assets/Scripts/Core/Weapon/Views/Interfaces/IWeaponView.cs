using Core.GameLogic;

namespace Core.Weapon
{
    public interface IWeaponView
    {
        WeaponProjectileType WeaponProjectileType { get; }
        void OnFire(IDamagableObject damagableObject);
        void OnStopFire();
    }
}