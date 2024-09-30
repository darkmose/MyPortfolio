using Core.Level;
using Core.Utilities;
using Zenject;

namespace Core.GameLogic
{
    public abstract class BaseGameEventObserver
    {
        private SimpleEvent<GameEventType> _gameEvent = new SimpleEvent<GameEventType>();
        public SimpleEvent<GameEventType> GameEvent => _gameEvent;
        public abstract GameEventType GameEventType { get; }

        protected void RaiseGameEvent()
        {
            _gameEvent.Notify(GameEventType);
        }

        public virtual void Prepare(DiContainer diContainer)
        {
        }

        public abstract void StartObserve();
        public abstract void StopObserve();
    }
}