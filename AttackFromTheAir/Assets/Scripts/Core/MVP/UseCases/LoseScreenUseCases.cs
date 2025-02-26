using Core.States;

namespace Core.MVP
{
    public class LoseScreenUseCases
    {
        private IStateMachine<GameplayStates> _stateMachine;

        public LoseScreenUseCases(IStateMachine<GameplayStates> gameplayStateMachine)
        {
            _stateMachine = gameplayStateMachine;
        }

        public void OnSettingButtonClick()
        {
            //Setting Menu
        }

        public void OnMultiplierXButtonButtonClick()
        {
            //вызвать рекламу 
        }

    }
}