using Zenject;

namespace Core.States
{
    public class GameplayState : BaseState<ApplicationStates>
    {
        public override ApplicationStates State => ApplicationStates.Gameplay;

        private IStateMachine<GameplayStates> _gameplayStateMachine;
        [Inject] private DiContainer _diContainer;
        public IStateMachine<GameplayStates> StateMachine => _gameplayStateMachine;

        public GameplayState()
        {
        }

        public override void Enter()
        {
            PrepareGameplayStateMachine();
            _gameplayStateMachine.SwitchToState(GameplayStates.Lobby);
        }

        public override void Exit()
        {
        }

        private void PrepareGameplayStateMachine()
        {
            _gameplayStateMachine = _diContainer.Resolve<IStateMachine<GameplayStates>>();
            var unloadState = _diContainer.Resolve<UnloadState>();
            var lobbyState = _diContainer.Resolve<LobbyState>();
            var gameState = _diContainer.Resolve<GameState>();
            var winState = _diContainer.Resolve<WinState>();
            var loseState = _diContainer.Resolve<LoseState>();
            var loadState = _diContainer.Resolve<LoadLevelState>();
            _gameplayStateMachine.InitiateStateMachine(unloadState, loadState, lobbyState, gameState, winState, loseState);
        }

    }

}