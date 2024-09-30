using UnityEngine;

namespace Core.Weapon
{
    public abstract class BaseBulletWeaponView : BaseWeaponView, IBulletsWeaponView
    {
        [SerializeField] protected Transform _bulletStartPoint;
        public override WeaponProjectileType WeaponProjectileType => WeaponProjectileType.Bullets;
        public abstract BulletsWeaponType BulletsWeaponType { get; }
    }
}