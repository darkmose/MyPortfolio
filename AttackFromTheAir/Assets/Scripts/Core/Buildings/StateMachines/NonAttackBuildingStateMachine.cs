using Zenject;

namespace Core.Buildings
{
    public class NonAttackBuildingStateMachine : BaseBuildingStateMachine
    {
        public NonAttackBuildingStateMachine(DiContainer diContainer) : base(diContainer)
        {
        }

        protected override void PrepareStateMachine(IBuilding building)
        {
            var idleState = new BuildingIdleState(building);
            var destroyState = new BuildingDestroyState(building);
            InitiateStateMachine(idleState, destroyState);
        }
    }
}