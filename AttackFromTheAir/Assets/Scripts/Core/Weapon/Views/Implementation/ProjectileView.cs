using Core.Units;
using UnityEngine;

namespace Core.Weapon
{
    public class ProjectileView : MonoBehaviour
    {
        private int _damage;
        private UnitFraction _weaponFraction;
        public int Damage => _damage;
        protected UnitFraction WeaponFraction => _weaponFraction;

        public void InitFraction(UnitFraction weaponFraction)
        {
            _weaponFraction = weaponFraction;
        }

        public void InitDamage(int damage)
        {
            _damage = damage;
        }
    }
}