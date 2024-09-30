using Core.Level;
using Core.MVP;
using Core.PlayerModule;

namespace Core.States
{
    public class WinState : BaseState<GameplayStates>
    {
        private IWinScreenPresenter _winScreenPresenter;
        private ILevelProgression _levelProgression;
        private ILevelController _levelController;
        private LevelSelector _levelSelector;
        public override GameplayStates State => GameplayStates.Win;

        public WinState(IWinScreenPresenter winScreenPresenter, ILevelProgression levelProgression, ILevelController levelController, LevelSelector levelSelector)
        {
            _winScreenPresenter = winScreenPresenter;
            _levelProgression = levelProgression;
            _levelController = levelController;
            _levelSelector = levelSelector;
        }

        public override void Enter()
        {
            _winScreenPresenter.Init();
            _winScreenPresenter.Show();
            _winScreenPresenter.NextLevelButtonClickEvent.AddListener(OnNextLevelButtonClick);
            _levelController.PlayerData.CameraController.EnableRootRotation();
        }

        private void OnNextLevelButtonClick()
        {
            if (_levelProgression.CurrentMaxLevel.Value == _levelSelector.CurrentLevel.Value)
            {
                var nextLevel = _levelProgression.CurrentMaxLevel.Value + 1;
                _levelProgression.SetLevelNumber(nextLevel);
            }

            stateMachine.SwitchToState(GameplayStates.Unload);
        }

        public override void Exit()
        {
            _winScreenPresenter.Hide();
            _winScreenPresenter.NextLevelButtonClickEvent.RemoveListener(OnNextLevelButtonClick);
            _levelController.PlayerData.CameraController.DisableRootRotation();
        }
    }
}