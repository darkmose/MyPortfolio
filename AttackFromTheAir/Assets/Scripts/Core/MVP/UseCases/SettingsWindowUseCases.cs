using Core.States;

namespace Core.MVP
{
    public class SettingsWindowUseCases
    {
        private IStateMachine<GameplayStates> _gameplayStateMachine;

        public SettingsWindowUseCases(IStateMachine<GameplayStates> gameplayStateMachine)
        {
            _gameplayStateMachine = gameplayStateMachine;
        }

        public void OnGoToLobbyButtonClick()
        {
            _gameplayStateMachine.SwitchToState(GameplayStates.Unload);
        }
    }
}