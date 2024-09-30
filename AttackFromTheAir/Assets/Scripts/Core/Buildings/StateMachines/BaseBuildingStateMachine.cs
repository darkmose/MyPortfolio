using Core.Events;
using Core.States;
using System;
using Zenject;

namespace Core.Buildings
{
    public enum BuildingStates
    {
        Idle,
        Attacking,
        Destroy
    }

    public interface IBuildingStateMachine
    {
        void InitStateMachine(IBuilding unit);
        void SwitchToState(BuildingStates state);
        bool HasState(BuildingStates state);
    }

    public abstract class BaseBuildingStateMachine : BaseStateMachine<BuildingStates>, IBuildingStateMachine
    {
        protected IBuilding _building;
        protected DiContainer _diContainer;

        protected BaseBuildingStateMachine(DiContainer diContainer)
        {
            _diContainer = diContainer;
            EventAggregator.Subscribe<LevelDisposeEvent>(OnLevelDispose);
        }

        private void OnLevelDispose(object sender, LevelDisposeEvent data)
        {
            EventAggregator.Unsubscribe<LevelDisposeEvent>(OnLevelDispose);
            _stateMachine?.Current?.Exit();
        }

        public void InitStateMachine(IBuilding building)
        {
            _building = building;
            PrepareStateMachine(building);
        }

        protected abstract void PrepareStateMachine(IBuilding building);
    }
}