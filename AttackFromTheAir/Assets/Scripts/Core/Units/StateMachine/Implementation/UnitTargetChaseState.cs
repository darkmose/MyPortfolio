using Core.GameLogic;
using Core.Level;
using Core.States;
using Core.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Units
{
    public class UnitTargetChaseState : BaseState<UnitStates>
    {
        private const float REPATH_RATE = .4f;
        private readonly IUnit _unit;
        private readonly ILevelController _levelController;
        private Vector3 _movePos = Vector3.zero;
        private Transform _unitTransform;
        private TargetContainer _targetContainer;
        private float _chaseStopDistance = 1f;
        private float _repathCounter;
        public override UnitStates State => UnitStates.TargetChase;

        public UnitTargetChaseState(IUnit unit, ILevelController levelController, TargetContainer targetContainer)
        {
            _unit = unit;
            _levelController = levelController;
            _targetContainer = targetContainer;
            if (_unit is IAttackingUnit attackingUnit)
            {
                _chaseStopDistance = attackingUnit.Weapon.Config.FireDistance;
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
                _unitTransform = _unit.UnitView.transform;
                var unitsCollector = _levelController.UnitsCollector;

                List<IUnit> targets;
                if (_unit.UnitFraction == UnitFraction.Ally)
                {
                    targets = unitsCollector.EnemyUnits;
                }
                else
                {
                    targets = unitsCollector.AllyUnits;
                }

                targets = targets.FindAll(pred => !pred.IsDestroyed && pred.UnitType == UnitType.People);

                if (targets.Count == 0)
                {
                    stateMachine.SwitchToState(UnitStates.Patrol);
                    return;
                }

                var randIndex = Random.Range(0, targets.Count);
                var target = targets[randIndex];
                _targetContainer.Target = target;
            }

            _movePos = _targetContainer.Target.View.transform.position;
            _unit.MoveTo(_movePos);
            _targetContainer.Target.ObjectDestroyed.AddListener(OnChaseTargetDestroyed);

            MonoUpdater.Instance.AddMonoUpdateListener(OnMonoUpdate);
        }

        private void OnChaseTargetDestroyed(IDamagableObject target)
        {
            _targetContainer.Target?.ObjectDestroyed.RemoveListener(OnChaseTargetDestroyed);
            MonoUpdater.Instance.RemoveUpdateListener(OnMonoUpdate);
            _targetContainer.Target = null;
            stateMachine.SwitchToState(UnitStates.TargetChase);
        }

        private void OnMonoUpdate()
        {
            var distance = Vector3.Distance(_movePos, _unitTransform.position);
            if (distance < _chaseStopDistance)
            {
                _unit.StopMoving();
                MonoUpdater.Instance.RemoveUpdateListener(OnMonoUpdate);
                stateMachine.SwitchToState(UnitStates.Attack);
            }
            else
            {
                _repathCounter += Time.deltaTime;
                if (_repathCounter >= 1f / REPATH_RATE)
                {
                    var target = _targetContainer.Target.View.transform;
                    _movePos = target.position;
                    _unit.MoveTo(_movePos);                    
                    _repathCounter = 0f;
                }
            }
        }

        public override void Exit()
        {
            _targetContainer.Target?.ObjectDestroyed.RemoveListener(OnChaseTargetDestroyed);
            MonoUpdater.Instance.RemoveUpdateListener(OnMonoUpdate);
        }
    }
    
}
