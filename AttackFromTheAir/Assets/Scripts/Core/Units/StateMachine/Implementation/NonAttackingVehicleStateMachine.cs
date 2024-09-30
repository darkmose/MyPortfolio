using Core.Level;
using Zenject;

namespace Core.Units
{
    public class NonAttackingVehicleStateMachine : BaseUnitStateMachine
    {
        public NonAttackingVehicleStateMachine(DiContainer diContainer) : base(diContainer)
        {
        }

        protected override void PrepareStateMachine(IUnit unit)
        {
            var levelController = _diContainer.Resolve<ILevelController>();
            var idleState = new VehicleIdleState(unit);
            var patrolState = new VehiclePatrolState(unit, levelController);
            var vehicleDestroyState = new VehicleDestroyState(unit);
            InitiateStateMachine(idleState, patrolState, vehicleDestroyState);
        }
    }
}
