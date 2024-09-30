using Core.GameLogic;
using Core.Utilities;

namespace Core.Weapon
{
    public enum WeaponProjectileType
    {
        Bullets,
        ExplosiveProjectiles,
        HomingExplosiveProjectiles,
    }

    public enum BulletsWeaponType
    {
        Rifle
    }

    public enum ExploProjectileWeaponType
    {
        TankTurret,
        RocketTurret
    }

    public enum HomingExploProjectileWeaponType
    {
        AirDefence
    }

    public interface IWeapon
    {
        BaseWeaponView WeaponView { get; set; }
        WeaponProjectileType WeaponProjectileType { get; }
        SimpleEvent<IDamagableObject> FireEvent { get; }
        SimpleEvent<float> AimEvent { get; }
        SimpleEvent FireStopEvent { get; }
        WeaponConfig Config { get; }
        void StartFire(IDamagableObject damagableObject);
        void StopFire();
    }
}