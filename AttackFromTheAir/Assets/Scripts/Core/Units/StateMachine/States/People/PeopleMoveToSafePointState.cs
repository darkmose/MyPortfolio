using Core.Events;
using Core.Level;
using Core.States;
using Core.Tools;
using UnityEngine;

namespace Core.Units
{
    public class PeopleMoveToSafePointState : BaseState<UnitStates>
    {
        private IUnit _unit;
        private ILevelController _levelController;
        private Vector3 _safePosition;
        public override UnitStates State => UnitStates.MoveToSafePoint;

        public PeopleMoveToSafePointState(IUnit unit, ILevelController levelController)
        {
            _unit = unit;
            _levelController = levelController;
        }

        public override void Enter()
        {
            var allSafePoints = _levelController.LevelData.SafePointsDescriptors.Find(pred => pred.UnitFraction == _unit.UnitFraction);
            var safePointIndex = UnityEngine.Random.Range(0, allSafePoints.SafePoints.Count);
            _safePosition = allSafePoints.SafePoints[safePointIndex].position;

            _unit.MoveTo(_safePosition);
            MonoUpdater.Instance.AddMonoUpdateListener(OnMonoUpdate);
        }

        private void OnMonoUpdate()
        {
            var distance = Vector3.Distance(_safePosition, _unit.UnitView.transform.position);
            if (distance < 5f)
            {
                MonoUpdater.Instance.RemoveUpdateListener(OnMonoUpdate);
                stateMachine.SwitchToState(UnitStates.Crawl);
            }
        }

        public override void Exit()
        {
            _unit.StopMoving();
            MonoUpdater.Instance.RemoveUpdateListener(OnMonoUpdate);
        }
    }
}
