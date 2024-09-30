using Core.GameLogic;
using Core.States;
using Core.Tools;

namespace Core.Units
{
    public class PeopleExplodedState : BaseState<UnitStates>
    {
        public override UnitStates State => UnitStates.Exploded;
        private IUnit _unit;
        private ExplosionData _explosionData;

        public PeopleExplodedState(IUnit unit)
        {
            _unit = unit;
            _unit.ObjectExploded.AddListener(OnObjectExploded);
        }

        private void OnObjectExploded(ExplosionData data)
        { 
            _explosionData = data;
            stateMachine.SwitchToState(UnitStates.Exploded);
        }

        public override void Enter()
        {
            if (_unit.UnitView.UnitSystems is NonAttackingPeopleUnitSystems systems)
            {
                systems.UnitAnimator.enabled = false;
                _unit.StopMoving();
                systems.MoveSystem.SetActiveNavMeshAgent(false);
                var ragdollController = systems.RagdollController;
                ragdollController.EnableRagdoll();
                ragdollController.AddExplosion(_explosionData);
                Timer.SetTimer(7f, () =>
                {
                    if (!_unit.IsDestroyed)
                    {
                        stateMachine.SwitchToState(UnitStates.TargetChase);
                    }
                });
            }
        }

        public override void Exit()
        {
            if (_unit.UnitView.UnitSystems is NonAttackingPeopleUnitSystems systems)
            {
                var ragdollController = systems.RagdollController;
                ragdollController.DisableRagdoll();
                systems.UnitAnimator.enabled = true;
                systems.MoveSystem.SetActiveNavMeshAgent(true);
            }
        }
    }
}
