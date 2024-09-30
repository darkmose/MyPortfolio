using Core.GameLogic;
using Core.Weapon;
using UnityEngine;

namespace Core.Units
{
    public abstract class BaseAttackingUnitView : BaseUnitView, IAttackingUnitView
    {
        [SerializeField] private Transform _weaponContainer;
        public override UnitBehaviourType UnitBehaviourType => UnitBehaviourType.Attacking;
        public abstract void OnStartAttacking(IDamagableObject unit);
        public abstract void OnStopAttacking();

        public void InitWeapon(BaseWeaponView weapon)
        {
            weapon.transform.SetParent(_weaponContainer);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localEulerAngles = Vector3.zero;
            weapon.transform.localScale = Vector3.one;
        }
    }
}