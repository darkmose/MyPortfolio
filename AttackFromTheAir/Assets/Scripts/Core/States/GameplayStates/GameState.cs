using Core.MVP;
using Core.Level;
using UnityEngine;
using Core.GameLogic;
using Core.Tools;
using Core.Units;

namespace Core.States
{
    public class GameState : BaseState<GameplayStates>
    {
        public override GameplayStates State => GameplayStates.Game;
        private IGameScreenPresenter _gameScreen;
        private IControlPanelScreenPresenter _controlPanelScreenPresenter;
        private ILevelController _levelController;
        private ILevelProgressionHelper _levelProgressionHelper;
        private CameraZoomService _cameraZoomService;
        private UnitStateMachinesHandler _unitStateMachinesHandler;
        public GameState(IGameScreenPresenter gameScreenPresenter, IControlPanelScreenPresenter controlPanelScreenPresenter,
        ILevelController levelController, CameraZoomService cameraZoomService, ILevelProgressionHelper levelProgressionHelper, 
        UnitStateMachinesHandler unitStateMachinesHandler)
        {
            _gameScreen = gameScreenPresenter;
            _controlPanelScreenPresenter = controlPanelScreenPresenter;
            _levelController = levelController;
            _cameraZoomService = cameraZoomService;
            _levelProgressionHelper = levelProgressionHelper;
            _unitStateMachinesHandler = unitStateMachinesHandler;
        }

        public override void Enter()
        {
            _cameraZoomService.Init();
            _gameScreen.Init();
            _gameScreen.Show();
            _controlPanelScreenPresenter.Init();
            _controlPanelScreenPresenter.Show();
            var controlPanel = _controlPanelScreenPresenter.ControlPanel;
            controlPanel.Enable();
            _levelController.PlayerData.MovementSystem.InitControlPanel(controlPanel);
            _levelController.PlayerData.CameraController.EnableRootRotation();

            _levelProgressionHelper.LoseEvent.AddListener(OnLose);
            _levelProgressionHelper.MainGoalsAchievedEvent.AddListener(OnWin);
            _levelProgressionHelper.StartObserving();
            _gameScreen.ProxyView.View.TargetingService.EnableTargeting();

            _unitStateMachinesHandler.StartStateMachines();
        }

        private void OnWin()
        {
            Debug.Log("Win");
            stateMachine.SwitchToState(GameplayStates.Win);
        }

        private void OnLose()
        {
            Debug.Log("Lose");
            stateMachine.SwitchToState(GameplayStates.Lose);
        }

        public override void Exit()
        {
            _gameScreen.ProxyView.View.TargetingService.DisableTargeting();
            _controlPanelScreenPresenter.ControlPanel.Disable();
            _levelController.PlayerData.CameraController.DisableRootRotation();
            _levelProgressionHelper.LoseEvent.RemoveListener(OnLose);
            _levelProgressionHelper.MainGoalsAchievedEvent.RemoveListener(OnWin);
            _levelProgressionHelper.StopObserving();
            _gameScreen.Hide();
        }
    }
}