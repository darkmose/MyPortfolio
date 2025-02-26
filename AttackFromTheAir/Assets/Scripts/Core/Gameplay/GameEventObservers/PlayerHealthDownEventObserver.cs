using Core.Level;
using Zenject;

namespace Core.GameLogic
{
    public class PlayerHealthDownEventObserver : BaseGameEventObserver
    {
        private PlayerData _playerData;
        public override GameEventType GameEventType => GameEventType.PlayerHealthDown;

        public override void Prepare(DiContainer diContainer)
        {
            _playerData = diContainer.Resolve<PlayerData>();
        }

        public override void StartObserve()
        {
            _playerData.PlayerHealth.ObjectDestroyed.AddListener(OnPlayerDroneDestroyed);
        }

        private void OnPlayerDroneDestroyed(IDamagableObject playerDrone)
        {
            RaiseGameEvent();
        }

        public override void StopObserve()
        {
            _playerData.PlayerHealth.ObjectDestroyed.RemoveListener(OnPlayerDroneDestroyed);
        }
    }
}