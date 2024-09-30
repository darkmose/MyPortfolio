using Core.Level;
using Core.MVP;
using Core.Tools;
using DG.Tweening;
using System;

namespace Core.States
{
    public class LoadLevelState : BaseState<GameplayStates>
    {
        private const float LEVEL_START_MIN_DELAY = 4f;
        private ILevelController _levelController;
        private ILoadingScreenPresenter _loadingScreenPresenter;
        private Tweener _startLevelDelayTween;
        private bool _levelLoaded;
        private bool _minDelayPassed;
        public override GameplayStates State => GameplayStates.LoadLevel;

        public LoadLevelState(ILevelController levelController, ILoadingScreenPresenter loadingScreenPresenter)
        {
            _levelController = levelController;
            _loadingScreenPresenter = loadingScreenPresenter;
        }

        public override void Enter()
        {
            _loadingScreenPresenter.Init();
            _loadingScreenPresenter.Show();
            _loadingScreenPresenter.StartLevelEvent.AddListener(OnLevelStart);
            _levelController.LevelLoadedEvent.AddListener(OnLevelLoadedHandler);
            _levelController.PrepareNextLevel();

            _loadingScreenPresenter.ProxyView.View.SetInteractableStartButton(false);
            _startLevelDelayTween = Timer.SetTimer(LEVEL_START_MIN_DELAY, () =>
            {
                _minDelayPassed = true;
                _loadingScreenPresenter.ProxyView.View.SetInteractableStartButton(true);
            });
        }

        private void OnLevelStart()
        {
            if (_levelLoaded && _minDelayPassed)
            {
                stateMachine.SwitchToState(GameplayStates.Game);
            }
        }

        private void OnLevelLoadedHandler()
        {
            _levelLoaded = true;
        }

        public override void Exit()
        {
            _startLevelDelayTween?.Kill();
            _loadingScreenPresenter.StartLevelEvent.RemoveListener(OnLevelStart);
            _levelController.LevelLoadedEvent.RemoveListener(OnLevelLoadedHandler);
            _loadingScreenPresenter.ProxyView.View.SetInteractableStartButton(true);
            _loadingScreenPresenter.Hide();
            _levelLoaded = false;
            _minDelayPassed = false;
        }
    }
}