using Core.Events;
using Core.GameLogic;
using Core.States;

namespace Core.Buildings
{
    public class BuildingDestroyState : BaseState<BuildingStates>
    {
        private IBuilding _building;
        public override BuildingStates State => BuildingStates.Destroy;

        public BuildingDestroyState(IBuilding building)
        {
            _building = building;
            _building.ObjectDestroyed.AddListener(OnObjectDestroyed);
        }

        private void OnObjectDestroyed(IDamagableObject damagableObject)
        {
            damagableObject.ObjectDestroyed.RemoveListener(OnObjectDestroyed);
            stateMachine.SwitchToState(BuildingStates.Destroy);
            EventAggregator.Post(this, new BuildingDestroyedEvent { Building = _building });
        }

        public override void Enter()
        {
            _building.BuildingView.gameObject.SetActive(false);
        }

        public override void Exit()
        {

        }
    }
}