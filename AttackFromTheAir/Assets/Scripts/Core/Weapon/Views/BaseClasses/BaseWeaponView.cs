using Core.GameLogic;
using Core.Units;
using UnityEngine;

namespace Core.Weapon
{
    public abstract class BaseWeaponView : MonoBehaviour, IWeaponView
    {
        private int _damage;
        protected int Damage => _damage;
        public abstract WeaponProjectileType WeaponProjectileType { get; }
        public UnitFraction WeaponFraction { get; set; }
        public abstract void OnFire(IDamagableObject damagableObject);
        public abstract void OnStopFire();
        public void InitDamage(int damage)
        {
            _damage = damage;   
        }
    }
}