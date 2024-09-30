using Core.Weapon;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Resourses
{
    [CreateAssetMenu(fileName ="WeaponsHolder", menuName ="ScriptableObjects/WeaponsHolder")]
    public class WeaponsHolder : ScriptableObject
    {
        public List<WeaponDescriptor> WeaponDescriptors;

        public BaseWeaponView GetBulletsWeapon(BulletsWeaponType bulletsWeaponType)
        {
            var weapon = WeaponDescriptors.Find(descr=>descr.ProjectileType == WeaponProjectileType.Bullets && descr.BulletsWeaponType == bulletsWeaponType);
            if (weapon != null)
            {
                return weapon.Prefab;
            }
            else
            {
                throw new System.Exception($"Could not find Bullet Weapon of type {bulletsWeaponType}");
            }
        }

        public BaseWeaponView GetExploProjectileWeapon(ExploProjectileWeaponType exploProjectileWeaponType)
        {
            var weapon = WeaponDescriptors.Find(descr => descr.ProjectileType == WeaponProjectileType.ExplosiveProjectiles && descr.ExploProjectileWeaponType == exploProjectileWeaponType);
            if (weapon != null)
            {
                return weapon.Prefab;
            }
            else
            {
                throw new System.Exception($"Could not find Explosive Projectiles Weapon of type {exploProjectileWeaponType}");
            }
        }

        public BaseWeaponView GetHomingExploProjectileWeapon(HomingExploProjectileWeaponType homingExploProjectileWeaponType)
        {
            var weapon = WeaponDescriptors.Find(descr => descr.ProjectileType == WeaponProjectileType.HomingExplosiveProjectiles && descr.HomingExploProjectileWeaponType == homingExploProjectileWeaponType);
            if (weapon != null)
            {
                return weapon.Prefab;
            }
            else
            {
                throw new System.Exception($"Could not find Homing Explo Projectile Weapon of type {homingExploProjectileWeaponType}");
            }
        }
    }

    [System.Serializable]
    public class WeaponDescriptor
    {
        public WeaponProjectileType ProjectileType;
        public BulletsWeaponType BulletsWeaponType;
        public ExploProjectileWeaponType ExploProjectileWeaponType;
        public HomingExploProjectileWeaponType HomingExploProjectileWeaponType;
        public BaseWeaponView Prefab;
    }
}