using Core.GameLogic;
using DG.Tweening;
using UnityEngine;

namespace Core.Units
{
    public class PeopleAttackSystem : UnitAttackSystem
    {
        [SerializeField] private Transform _unitTransform;
        [SerializeField] private Animator _animator;
        private int _fireTrigger = Animator.StringToHash("WeaponFire");
        private Tweener _aimTweener;

        public override void AimAtTarget(float duration)
        {
            var direction = Target.View.transform.position - _unitTransform.position;
            direction.y = 0f;
            var rotation = Quaternion.LookRotation(direction);
            Weapon.WeaponView.transform.rotation = rotation;
            _aimTweener = _unitTransform.DORotateQuaternion(rotation, duration);
        }

        public override void ResetAiming()
        {
            _aimTweener?.Kill();
        }

        protected override void OnFire(IDamagableObject damagable)
        {
            _animator.SetTrigger(_fireTrigger);
        }

        private void OnDestroy()
        {
            ResetAiming();
        }
    }
}