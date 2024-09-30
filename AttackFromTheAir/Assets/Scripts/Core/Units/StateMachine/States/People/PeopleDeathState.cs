using Core.Events;
using Core.GameLogic;
using Core.States;
using UnityEngine;

namespace Core.Units
{
    public class PeopleDeathState : BaseState<UnitStates>
    {
        public override UnitStates State => UnitStates.Dead;
        private int _deathAnimatorTrigger = Animator.StringToHash("Death");
        private readonly IUnit _unit;

        public PeopleDeathState(IUnit unit)
        {
            _unit = unit;
            _unit.ObjectDestroyed.AddListener(OnPeopleDead);
        }

        private void OnPeopleDead(IDamagableObject damagableObject)
        {
            _unit.ObjectDestroyed.RemoveListener(OnPeopleDead);
            stateMachine.SwitchToState(UnitStates.Dead);
            EventAggregator.Post(this, new UnitDeadEvent { Unit = _unit });
        }

        public override void Enter()
        {
            Debug.Log($"{_unit.UnitView.gameObject.name}___Death");
            if (_unit.UnitView.UnitSystems is NonAttackingPeopleUnitSystems systems)
            {
                systems.UnitAnimator.enabled = false;
                if (systems is AttackingPeopleUnitSystems attackingSystems)
                {
                    attackingSystems.UnitAttackSystem.StopAttack();
                    attackingSystems.UnitAttackSystem.ResetAiming();
                }
                _unit.StopMoving();
                systems.MoveSystem.SetActiveNavMeshAgent(false);
                var ragdollController = systems.RagdollController;
                ragdollController.EnableRagdoll();
            }
        }

        public override void Exit()
        {
        }
    }
}
