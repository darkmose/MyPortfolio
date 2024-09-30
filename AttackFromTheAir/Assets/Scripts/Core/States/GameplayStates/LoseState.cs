using Core.Level;
using Core.MVP;

namespace Core.States
{
    public class LoseState : BaseState<GameplayStates>
    {
        private ILoseScreenPresenter _loseScreenPresenter;
        private ILevelController _levelController;
        public override GameplayStates State => GameplayStates.Lose;

        public LoseState(ILoseScreenPresenter loseScreenPresenter, ILevelController levelController)
        {
            _loseScreenPresenter = loseScreenPresenter;
            _levelController = levelController;
        }

        public override void Enter()
        {
            _loseScreenPresenter.Init();
            _loseScreenPresenter.Show();
            _loseScreenPresenter.RetryButtonClickEvent.AddListener(OnRetryButtonClick);
            _levelController.PlayerData.CameraController.EnableRootRotation();
        }

        private void OnRetryButtonClick()
        {
            stateMachine.SwitchToState(GameplayStates.Unload);
        }

        public override void Exit()
        {
            _loseScreenPresenter.Hide();
            _loseScreenPresenter.RetryButtonClickEvent.RemoveListener(OnRetryButtonClick);
            _levelController.PlayerData.CameraController.DisableRootRotation();
        }
    }
}