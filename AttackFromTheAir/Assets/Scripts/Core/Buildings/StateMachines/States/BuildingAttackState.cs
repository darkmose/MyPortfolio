using Core.Level;
using Core.States;

namespace Core.Buildings
{
    public class BuildingAttackState : BaseState<BuildingStates>
    {
        private IBuilding _building;
        private ILevelController _levelController;
        public override BuildingStates State => BuildingStates.Attacking;

        public BuildingAttackState(IBuilding building, ILevelController levelController)
        {
            _building = building;
            _levelController = levelController;
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }
    }
}