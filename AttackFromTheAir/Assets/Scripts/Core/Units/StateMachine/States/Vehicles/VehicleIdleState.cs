using Core.States;

namespace Core.Units
{
    public class VehicleIdleState : BaseState<UnitStates>
    {
        public override UnitStates State => UnitStates.Idle;
        private IUnit _unit;

        public VehicleIdleState(IUnit unit)
        {
            _unit = unit;
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }
    }
}
