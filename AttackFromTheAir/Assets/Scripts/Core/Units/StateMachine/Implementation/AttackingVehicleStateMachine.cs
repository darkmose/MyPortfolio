using Core.GameLogic;
using Core.Level;
using Zenject;

namespace Core.Units
{
    public class AttackingVehicleStateMachine : BaseUnitStateMachine
    {
        private TargetContainer _targetContainer;

        public AttackingVehicleStateMachine(DiContainer diContainer) : base(diContainer)
        {
            _targetContainer = new TargetContainer();
        }

        protected override void PrepareStateMachine(IUnit unit)
        {
            var levelController = _diContainer.Resolve<ILevelController>();
            var idleState = new VehicleIdleState(unit);
            var patrolState = new VehiclePatrolState(unit, levelController);
            var vehicleChaseState = new UnitTargetChaseState(unit, levelController, _targetContainer);
            var vehicleAttackState = new VehicleAttackState(unit, levelController, _targetContainer);
            var vehicleDestroyState = new VehicleDestroyState(unit);
            InitiateStateMachine(idleState, patrolState, vehicleChaseState, vehicleAttackState, vehicleDestroyState);
        }
    }

    public class TargetContainer
    {
        public IDamagableObject Target { get; set; }
    }
}
