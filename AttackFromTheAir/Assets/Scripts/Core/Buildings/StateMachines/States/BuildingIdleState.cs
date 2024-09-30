using Core.States;

namespace Core.Buildings
{
    public class BuildingIdleState : BaseState<BuildingStates>
    {
        private IBuilding _building;
        public override BuildingStates State => BuildingStates.Idle;

        public BuildingIdleState(IBuilding building)
        {
            _building = building;
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}