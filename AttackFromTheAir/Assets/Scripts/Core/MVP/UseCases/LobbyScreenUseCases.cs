using Core.States;

namespace Core.MVP
{
    public class LobbyScreenUseCases 
    {
        private IStateMachine<GameplayStates> _stateMachine;

        public LobbyScreenUseCases(IStateMachine<GameplayStates> gameplayStateMachine)
        {
            _stateMachine = gameplayStateMachine;
        }

    }
}