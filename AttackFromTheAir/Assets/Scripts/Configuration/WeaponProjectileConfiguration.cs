using UnityEngine;

namespace Configuration
{
    [CreateAssetMenu(fileName = "WeaponProjectileConfiguration", menuName ="ScriptableObjects/WeaponProjectileConfiguration")]
    public sealed class WeaponProjectileConfiguration : ScriptableObject
    {
        public float SmallBulletSpeed = 10f;
        public float MediumBulletSpeed = 7f;
        public float HeavyProjectileSpeed = 5f;
    }
}