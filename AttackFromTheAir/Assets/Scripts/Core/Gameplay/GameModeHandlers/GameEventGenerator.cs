using Core.Events;
using Core.Level;
using System.Collections.Generic;

namespace Core.GameLogic
{
    public class GameEventGenerator
    {
        private Dictionary<GameEventType, BaseGameEventObserver> _observersDict;
        private GameEvent _gameEvent;

        public GameEventGenerator()
        {
            _gameEvent = new GameEvent();
            _observersDict = new Dictionary<GameEventType, BaseGameEventObserver>();
        }

        public void InitEvents(List<GameEventType> events) 
        {
            foreach (var @event in events)
            {
                var observer = GameEventObserversFactory.CreateObserver(@event);
                observer.GameEvent.AddListener(OnGameEvent);
                _observersDict.Add(@event, observer);
            }    
        }

        public void StartEventsObserve()
        {
            foreach (var observer in _observersDict.Values)
            {
                observer.StartObserve();
            }
        }

        public void StopEventsObserve()
        {
            foreach (var observer in _observersDict.Values)
            {
                observer.StopObserve();
            }
        }

        private void OnGameEvent(GameEventType type)
        {
            _gameEvent.GameEventType = type;
            EventAggregator.Post(this, _gameEvent);
        }

        public void ClearEvents()
        {
            foreach (var observer in _observersDict.Values)
            {
                observer.GameEvent.RemoveListener(OnGameEvent);
            }
            _observersDict.Clear();
        }
    }
}