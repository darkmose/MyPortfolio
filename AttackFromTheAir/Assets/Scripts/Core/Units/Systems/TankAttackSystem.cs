using Core.GameLogic;
using UnityEngine;

namespace Core.Units
{
    public class TankAttackSystem : UnitAttackSystem
    {
        [SerializeField] private Animator _unitAnimator;
        [SerializeField] private TankTurretAimingSystem _tankTurretAimingSystem;
        private int _fireTrigger = Animator.StringToHash("WeaponFire");

        public override void AimAtTarget(float duration)
        {
            _tankTurretAimingSystem.AimAtTarget(Target, duration);
        }

        public override void ResetAiming()
        {
            _tankTurretAimingSystem.ResetAim();
        }

        protected override void OnFire(IDamagableObject damagable)
        {
            _unitAnimator.SetTrigger(_fireTrigger);
        }
    }
}