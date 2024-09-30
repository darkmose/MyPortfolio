using Core.GameLogic;
using Core.Weapon;
using UnityEngine;

namespace Core.Units
{
    public abstract class UnitAttackSystem : MonoBehaviour
    {
        private IWeapon _weapon;
        private IDamagableObject _target;
        protected IDamagableObject Target => _target;
        protected IWeapon Weapon => _weapon;

        public void InitWeapon(IWeapon weapon) 
        {
            _weapon = weapon;
        }

        public void InitTarget(IDamagableObject target)
        {
            _target = target;
        }

        public abstract void ResetAiming();

        public abstract void AimAtTarget(float duration);

        public void AttackTarget()
        {
            _weapon.AimEvent.AddListener(AimAtTarget);
            _weapon.FireEvent.AddListener(OnFire);
            _weapon.StartFire(_target);
        }

        protected abstract void OnFire(IDamagableObject damagable);

        public void StopAttack()
        {
            _weapon?.StopFire();
            _weapon?.AimEvent.RemoveListener(AimAtTarget);
            _weapon?.FireEvent.RemoveListener(OnFire);
        }
    }
}