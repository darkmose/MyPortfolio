using Core.Weapon;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Resourses
{
    [CreateAssetMenu(fileName ="WeaponConfigsHolder", menuName ="ScriptableObjects/WeaponConfigsHolder")]
    public class WeaponConfigsHolder : ScriptableObject
    {
        public List<WeaponConfigDescriptor> WeaponConfigDescriptors;

        public WeaponConfig GetBulletsWeapon(BulletsWeaponType bulletsWeaponType)
        {
            var config = WeaponConfigDescriptors.Find(descr=>descr.ProjectileType == WeaponProjectileType.Bullets && descr.BulletsWeaponType == bulletsWeaponType);
            if (config != null)
            {
                return config.WeaponConfig;
            }
            else
            {
                throw new System.Exception($"Could not find Bullet Weapon Config of type {bulletsWeaponType}");
            }
        }

        public WeaponConfig GetExploProjectileWeapon(ExploProjectileWeaponType exploProjectileWeaponType)
        {
            var config = WeaponConfigDescriptors.Find(descr => descr.ProjectileType == WeaponProjectileType.ExplosiveProjectiles && descr.ExploProjectileWeaponType == exploProjectileWeaponType);
            if (config != null)
            {
                return config.WeaponConfig;
            }
            else
            {
                throw new System.Exception($"Could not find Explosive Projectiles Weapon Config of type {exploProjectileWeaponType}");
            }
        }

        public WeaponConfig GetHomingExploProjectileWeapon(HomingExploProjectileWeaponType homingExploProjectileWeaponType)
        {
            var config = WeaponConfigDescriptors.Find(descr => descr.ProjectileType == WeaponProjectileType.HomingExplosiveProjectiles && descr.HomingExploProjectileWeaponType == homingExploProjectileWeaponType);
            if (config != null)
            {
                return config.WeaponConfig;
            }
            else
            {
                throw new System.Exception($"Could not find Homing Explo Projectile Weapon Config of type {homingExploProjectileWeaponType}");
            }
        }
    }

    [System.Serializable]
    public class WeaponConfigDescriptor
    {
        public WeaponProjectileType ProjectileType;
        public BulletsWeaponType BulletsWeaponType;
        public ExploProjectileWeaponType ExploProjectileWeaponType;
        public HomingExploProjectileWeaponType HomingExploProjectileWeaponType;
        public WeaponConfig WeaponConfig;
    }
}