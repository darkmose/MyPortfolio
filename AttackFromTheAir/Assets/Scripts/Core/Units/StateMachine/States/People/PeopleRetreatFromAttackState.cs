using Core.Level;
using Core.States;
using Core.Tools;
using UnityEngine;
using Core.Events;

namespace Core.Units
{
    public class PeopleRetreatFromAttackState : BaseState<UnitStates>
    {
        private IUnit _unit;
        private Vector3 _retreatPosition;
        private const float RetreatRadiusMin = 20f; 
        private const float RetreatRadiusMax = 25f; 
        private const float StopDistance = 0.2f; 

        public override UnitStates State => UnitStates.RetreatFromAttack;

        public PeopleRetreatFromAttackState(IUnit unit)
        {
            _unit = unit;
        }        

        public override void Enter()
        {
            Debug.Log("PeopleRetreatEnter");
            CalculateRetreatPosition();
            _unit.MoveTo(_retreatPosition);
            MonoUpdater.Instance.AddMonoUpdateListener(OnMonoUpdate);
        }

        private void OnMonoUpdate()
        {
            float distance = Vector3.Distance(_unit.UnitView.transform.position, _retreatPosition);
            if (distance <= StopDistance)
            {               
                stateMachine.SwitchToState(UnitStates.Attack);
                MonoUpdater.Instance.RemoveUpdateListener(OnMonoUpdate);
            }
        }

        public override void Exit()
        {
            Debug.Log("PeopleRetreatExit");
            
            _unit.StopMoving();
            MonoUpdater.Instance.RemoveUpdateListener(OnMonoUpdate);
        }

        private void CalculateRetreatPosition()
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float randomDistance = Random.Range(RetreatRadiusMin, RetreatRadiusMax);
            Vector3 offset = new Vector3(randomDirection.x, 0, randomDirection.y) * randomDistance;
            _retreatPosition = _unit.UnitView != null ? _unit.UnitView.transform.position + offset : Vector3.zero;
        }
    }
}
