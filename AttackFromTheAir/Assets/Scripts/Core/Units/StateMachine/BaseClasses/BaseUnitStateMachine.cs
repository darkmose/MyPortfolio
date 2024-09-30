using Core.Events;
using Core.States;
using Core.Tools;
using Zenject;

namespace Core.Units
{
    public abstract class BaseUnitStateMachine : BaseStateMachine<UnitStates>, IUnitStateMachine
    {
        protected IUnit _unit;
        protected DiContainer _diContainer;
        public UnitStates InitialState { get; set; }
        public TriggerToUnitStateConfiguration TriggerToUnitStateMatrix { get; set; }

        protected BaseUnitStateMachine(DiContainer diContainer)
        {
            _diContainer = diContainer;
            EventAggregator.Subscribe<LevelDisposeEvent>(OnLevelDispose);
        }

        private void OnLevelDispose(object sender, LevelDisposeEvent data)
        {
            EventAggregator.Unsubscribe<LevelDisposeEvent>(OnLevelDispose);
            _stateMachine?.Current?.Exit();
        }

        public void InitStateMachine(IUnit unit)
        {
            _unit = unit;
            PrepareStateMachine(unit);  
        }

        protected abstract void PrepareStateMachine(IUnit unit);
    }
}
