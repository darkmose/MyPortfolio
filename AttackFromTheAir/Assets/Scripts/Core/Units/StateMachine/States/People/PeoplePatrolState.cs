using Core.Level;
using Core.States;
using Core.Tools;
using DG.Tweening;
using UnityEngine;

namespace Core.Units
{
    public class PeoplePatrolState : BaseState<UnitStates>
    {
        public override UnitStates State => UnitStates.Patrol;
        private readonly IUnit _unit;
        private readonly ILevelController _levelController;
        private PatrolPointsDescriptor _patrolPointsDescriptor;
        private Tweener _stayTweener;
        private int _patrolPointIndex = 0;

        public PeoplePatrolState(IUnit unit, ILevelController levelController)
        {
            _unit = unit;
            _levelController = levelController;
        }

        public override void Enter()
        {
            var patrolPoints = _levelController.LevelData.PatrolPointsDescriptors;
            var acceptedPoints = patrolPoints.FindAll(point=>point.UnitsFraction==_unit.UnitFraction);
            float distance = float.MaxValue;

            foreach (var point in acceptedPoints) 
            {
                var dist = Vector3.Distance(point.PatrolPoints[0].position, _unit.UnitView.transform.position);
                if (dist < distance)
                {
                    _patrolPointsDescriptor = point;
                    distance = dist;
                }                
            }

            if (_patrolPointsDescriptor != null)
            {
                _unit.MoveTo(_patrolPointsDescriptor.PatrolPoints[_patrolPointIndex].position);
                _unit.UnitView.UnitSystems.MoveSystem.UnitReachedDestinationEvent.AddListener(OnPatrolPointReached);
            }
            else
            {
                stateMachine.SwitchToState(UnitStates.Idle);
            }
        }

        private void OnPatrolPointReached()
        {
            _patrolPointIndex++;
            if (_patrolPointIndex >= _patrolPointsDescriptor.PatrolPoints.Count)
            {
                _patrolPointIndex = 0;
            }

            var stayDuration = UnityEngine.Random.Range(_patrolPointsDescriptor.StayDurationMin, _patrolPointsDescriptor.StayDurationMax);
            _stayTweener = Timer.SetTimer(stayDuration, OnStayTimerOver);            
        }

        private void OnStayTimerOver()
        {
            _unit.MoveTo(_patrolPointsDescriptor.PatrolPoints[_patrolPointIndex].position);
        }

        public override void Exit()
        {
            _patrolPointsDescriptor = null;
            _patrolPointIndex = 0;
            _stayTweener?.Kill();
            _unit.UnitView.UnitSystems.MoveSystem.UnitReachedDestinationEvent.RemoveListener(OnPatrolPointReached);
            _unit.StopMoving();
        }
    }
}
