using Core.DISimple;
using Core.Events;
using Core.States;
using UnityEngine;
using Zenject;

namespace Core
{
    public class ApplicationStart : MonoBehaviour
    {
        private IStateMachine<ApplicationStates> _stateMachine;
        private DiContainer _diContainer;
        private GameplayState _gameplayState;

        void Awake()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            _diContainer = ServiceLocator.Resolve<DiContainer>();
            PrepareApplicationStateMachine();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                EventAggregator.Post(this, new StorageEvent());
                _gameplayState.StateMachine.SwitchToState(GameplayStates.Unload);
            }
        }

        private void OnApplicationQuit()
        {
            EventAggregator.Post(this, new StorageEvent());
            _gameplayState.StateMachine.SwitchToState(GameplayStates.Unload);
        }

        private void PrepareApplicationStateMachine()
        {            
            var loadingState = _diContainer.Resolve<LoadingState>();
            _gameplayState = _diContainer.Resolve<GameplayState>();
            _stateMachine = _diContainer.Resolve<IStateMachine<ApplicationStates>>();
            _stateMachine.InitiateStateMachine(loadingState, _gameplayState);
            _stateMachine.SwitchToState(ApplicationStates.Loading);
        }
    }
}