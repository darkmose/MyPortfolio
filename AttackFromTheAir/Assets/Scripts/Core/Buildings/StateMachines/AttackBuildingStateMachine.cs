using Core.Level;
using Zenject;

namespace Core.Buildings
{
    public class AttackBuildingStateMachine : BaseBuildingStateMachine
    {
        public AttackBuildingStateMachine(DiContainer diContainer) : base(diContainer)
        {
        }

        protected override void PrepareStateMachine(IBuilding building)
        {
            var levelController = _diContainer.Resolve<ILevelController>();
            var idleState = new BuildingIdleState(building);
            var attackState = new BuildingAttackState(building, levelController);
            var destroyState = new BuildingDestroyState(building);
            InitiateStateMachine(idleState, destroyState);
        }
    }
}