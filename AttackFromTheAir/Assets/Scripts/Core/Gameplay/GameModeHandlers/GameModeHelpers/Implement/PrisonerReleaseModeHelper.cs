using Core.Events;
using Core.Level;
using Core.MVP;
using System;
using Zenject;

namespace Core.GameLogic
{
    public class PrisonerReleaseModeHelper : AttackGameModeHelper
    {
        private IGameScreenPresenter _gameScreenPresenter;
        private PrisonerReleaseScreenOverride _prisonerReleaseScreenOverride;
        private ILevelController _levelController;
        private int _releaseAmountTarget;
        private int _releaseAmount;
        public override AttackGameMode AttackGameMode => AttackGameMode.PrisonerRelease;
        public override void InitDI(DiContainer container)
        {
            base.InitDI(container);
            _gameScreenPresenter = container.Resolve<IGameScreenPresenter>();
            _levelController = container.Resolve<ILevelController>();
        }

        public override void Start()
        {
            _gameScreenPresenter.Init();
            var screenOverride = _gameScreenPresenter.ProxyView.View.SetAttackGameModeScreenOverride(AttackGameMode);
            if (screenOverride is PrisonerReleaseScreenOverride prisonerReleaseScreenOverride)
            {
                _prisonerReleaseScreenOverride = prisonerReleaseScreenOverride;
                foreach (var item in _levelController.LevelDescriptor.GameModeSettings.EventsToWin.Events)
                {
                    switch (item.GameEventType)
                    {
                        case GameEventType.PrisonerReleased:
                            _releaseAmountTarget = item.Amount;
                            _releaseAmount = 0;
                            _prisonerReleaseScreenOverride.SetStatus(_releaseAmount, _releaseAmountTarget);
                            break;
                    }
                }
                EventAggregator.Subscribe<GameEvent>(OnGameEvent);
            }
        }

        private void OnGameEvent(object arg1, GameEvent @event)
        {
            if (@event.GameEventType == GameEventType.PrisonerReleased)
            {
                _releaseAmount++;
                _prisonerReleaseScreenOverride.SetStatus(_releaseAmount, _releaseAmountTarget);
            }
        }

        public override void Stop()
        {
            EventAggregator.Unsubscribe<GameEvent>(OnGameEvent);
            _gameScreenPresenter.ProxyView.View.ClearGameModeOverrides();
        }
    }
}