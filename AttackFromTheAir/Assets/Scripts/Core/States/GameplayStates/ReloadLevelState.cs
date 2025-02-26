using Core.Events;
using Core.Level;

namespace Core.States
{
    public class ReloadLevelState : BaseState<GameplayStates>
    {
        private ILevelController _levelController;
        public override GameplayStates State => GameplayStates.ReloadLevel;

        public ReloadLevelState(ILevelController levelController)
        {
            _levelController = levelController;
        }

        public override void Enter()
        {
            EventAggregator.Post(this, new LevelDisposeEvent());
            _levelController.UnloadLevel();
            stateMachine.SwitchToState(GameplayStates.LoadLevel);
        }

        public override void Exit()
        {
        }
    }
}

