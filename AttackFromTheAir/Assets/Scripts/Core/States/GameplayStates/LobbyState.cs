using Core.MVP;
using Core.Level;
using Core.Tools;
using Core.Events;
using System;

namespace Core.States
{
    public class LobbyState : BaseState<GameplayStates>
    {
        private ILobbyScreenPresenter _lobbyScreenPresenter;
        public override GameplayStates State => GameplayStates.Lobby;

        public LobbyState(ILobbyScreenPresenter lobbyScreenPresenter)
        {
            _lobbyScreenPresenter = lobbyScreenPresenter;
        }

        public override void Enter()
        {
            _lobbyScreenPresenter.Init();
            _lobbyScreenPresenter.Show();
            _lobbyScreenPresenter.PlayButtonTapEvent.AddListener(OnPlayButtonClickHandler);
            EventAggregator.Subscribe<LevelStartRequestEvent>(OnLevelStartRequest);
        }

        private void OnLevelStartRequest(object arg1, LevelStartRequestEvent @event)
        {
            stateMachine.SwitchToState(GameplayStates.LoadLevel);
        }

        private void OnPlayButtonClickHandler()
        {
            if (_lobbyScreenPresenter.ProxyView.View.TabToggles.CurrentTab == UI.LobbyTabs.LevelStart)
            {
                stateMachine.SwitchToState(GameplayStates.LoadLevel);
            }
            else
            {
                _lobbyScreenPresenter.ProxyView.View.TabToggles.SwitchTab(UI.LobbyTabs.LevelStart);
            }
        }

        public override void Exit()
        {
            _lobbyScreenPresenter.Hide();
            _lobbyScreenPresenter.PlayButtonTapEvent.RemoveListener(OnPlayButtonClickHandler);
            EventAggregator.Unsubscribe<LevelStartRequestEvent>(OnLevelStartRequest);
        }
    }
}