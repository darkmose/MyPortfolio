using Core.Events;
using Core.GameLogic;
using Core.States;
using Core.Tools;
using UnityEngine;

namespace Core.Units
{
    public class VehicleDestroyState : BaseState<UnitStates>
    {
        public override UnitStates State => UnitStates.Dead;
        private IUnit _unit;

        public VehicleDestroyState(IUnit unit)
        {
            _unit = unit;
            _unit.ObjectDestroyed.AddListener(OnVehicleDestroyed);
        }

        private void OnVehicleDestroyed(IDamagableObject damagable)
        {
            _unit.ObjectDestroyed.RemoveListener(OnVehicleDestroyed);
            stateMachine.SwitchToState(UnitStates.Dead);
            EventAggregator.Post(this, new UnitDeadEvent { Unit = _unit });
        }

        public override void Enter()
        {
            Debug.Log($"{_unit.UnitView.gameObject.name}___Destroy");
            if (_unit.UnitView.UnitSystems is NonAttackingVehicleUnitSystems systems)
            {
                if (systems is AttackingVehicleUnitSystems attackingSystems)
                {
                    attackingSystems.UnitAttackSystem.StopAttack();
                    attackingSystems.UnitAttackSystem.ResetAiming();
                }
                _unit.StopMoving();
                systems.MoveSystem.SetActiveNavMeshAgent(false);
                systems.VehicleDestroyService.Destroy();

                Timer.SetTimer(5f, () => { systems.VehicleDestroyService.ResetPhysics(); });
            }
        }

        public override void Exit()
        {
            
        }
    }
}
