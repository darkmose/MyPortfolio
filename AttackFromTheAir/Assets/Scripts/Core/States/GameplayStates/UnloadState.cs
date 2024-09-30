using Core.Events;
using Core.Level;

namespace Core.States
{
    public class UnloadState : BaseState<GameplayStates>
    {
        public override GameplayStates State => GameplayStates.Unload;
        private ILevelController _levelController;

        public UnloadState(ILevelController levelController)
        {
            _levelController = levelController;
        }

        public override void Enter()
        {
            EventAggregator.Post(this, new LevelDisposeEvent());
            _levelController.UnloadLevel();
            stateMachine.SwitchToState(GameplayStates.Lobby);
        }

        public override void Exit()
        {            
        }
    }
}

