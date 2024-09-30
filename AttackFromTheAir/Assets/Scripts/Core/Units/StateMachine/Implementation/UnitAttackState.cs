using Core.GameLogic;
using Core.Level;
using Core.States;
using Core.Tools;
using Core.Weapon;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Core.Units
{
    public abstract class UnitAttackState : BaseState<UnitStates>
    {
        private readonly IUnit _unit;
        private readonly ILevelController _levelController;
        private readonly TargetContainer _targetContainer;
        private readonly IWeapon _weapon;
        private Transform _unitTransform;
        private Transform _targetTranform;
        private float _chaseDistance;
        protected IUnit Unit => _unit;
        protected TargetContainer TargetContainer => _targetContainer;
        protected IWeapon Weapon => _weapon;
        protected Transform TargetTranform => _targetTranform;
        protected Transform UnitTransform => _unitTransform;

        public override UnitStates State => UnitStates.Attack;

        public UnitAttackState(IUnit unit, ILevelController levelController, TargetContainer targetContainer)
        {
            _unit = unit;
            _levelController = levelController;
            _targetContainer = targetContainer;
            if (_unit is IAttackingUnit attackingUnit)
            {
                _weapon = attackingUnit.Weapon;
                var fireDistance = attackingUnit.Weapon.Config.FireDistance;
                _chaseDistance = fireDistance + fireDistance * 0.2f;
            }
        }

        public override void Enter()
        {
            if (_targetContainer.Target != null && _targetContainer.Target.IsDestroyed)
            {
                _targetContainer.Target = null;
            }
            if (_targetContainer.Target == null)
            {
                stateMachine.SwitchToState(UnitStates.TargetChase);
                return;
            }

            _unitTransform = _unit.UnitView.transform;
            _targetTranform = _targetContainer.Target.View.transform;
            _targetContainer.Target.ObjectDestroyed.AddListener(OnTargetDestroyed);
            MonoUpdater.Instance.AddMonoUpdateListener(OnMonoUpdate);

            EnterInner();
        }

        protected abstract void EnterInner();

        private void OnTargetDestroyed(IDamagableObject target)
        {
            _targetContainer.Target?.ObjectDestroyed.RemoveListener(OnTargetDestroyed);
            MonoUpdater.Instance.RemoveUpdateListener(OnMonoUpdate);
            _targetContainer.Target = null;
            stateMachine.SwitchToState(UnitStates.TargetChase);
        }

        private void OnMonoUpdate()
        {
            var distance = Vector3.Distance(_unitTransform.position, _targetTranform.position);
            if (distance > _chaseDistance)
            {
                MonoUpdater.Instance.RemoveUpdateListener(OnMonoUpdate);
                stateMachine.SwitchToState(UnitStates.TargetChase);
            }
        }

        protected abstract void ExitInner();

        public override void Exit()
        {
            MonoUpdater.Instance.RemoveUpdateListener(OnMonoUpdate);
            _targetContainer.Target?.ObjectDestroyed.RemoveListener(OnTargetDestroyed);
            
            ExitInner();
        }
    }
}
