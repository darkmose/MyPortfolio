using Core.Level;
using System;
using System.Collections.Generic;
using Zenject;

namespace Core.GameLogic
{
    public class GameEventObserversFactory
    {
        private static GameEventObserversFactory _instance;
        private Dictionary<GameEventType, BaseGameEventObserver> _cachedObservers;
        private DiContainer _diContainer;

        public GameEventObserversFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
            _cachedObservers = new Dictionary<GameEventType, BaseGameEventObserver>();
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public static BaseGameEventObserver CreateObserver(GameEventType gameEventType)
        {
            if (_instance._cachedObservers.TryGetValue(gameEventType, out var observer))
            {
                return observer;
            }
            else
            {
                BaseGameEventObserver eventObserver = null;

                switch (gameEventType)
                {
                    case GameEventType.EnemyInfantryKilled:
                        eventObserver = new EnemyInfantryKilledEventObserver();
                        break;
                    case GameEventType.EnemyMediumEquipmentDestroyed:
                        eventObserver = new EnemyMediumEquipmentDestroyedEventObserver();
                        break;
                    case GameEventType.EnemyHeavyEquipmentDestroyed:
                        eventObserver = new EnemyHeavyEquipmentDestroyedEventObserver();
                        break;
                    case GameEventType.AllyInfantryKilled:
                        eventObserver = new AllyInfantryKilledEventObserver();
                        break;
                    case GameEventType.AllyMediumEquipmentDestroyed:
                        eventObserver = new AllyMediumEquipmentDestroyedEventObserver();
                        break;
                    case GameEventType.AllyHeavyEquipmentDestroyed:
                        eventObserver = new AllyHeavyEquipmentDestroyedEventObserver();
                        break;
                    case GameEventType.TimerEnd:
                        eventObserver = null;
                        break;
                    case GameEventType.EnemyBuildingDestroyed:
                        eventObserver = new EnemyBuildingDestroyedEventObserver();
                        break;
                    case GameEventType.PlayerHealthDown:
                        eventObserver = null;
                        break;
                    case GameEventType.TestTimeDelay:
                        eventObserver = new TestTimeDelayEventObserver();
                        break;
                    case GameEventType.PrisonerKilled:
                        eventObserver = new PrisonerKilledEventObserver();
                        break;
                    case GameEventType.AllyBuildingDestroyed:
                        eventObserver = null;
                        break;
                    default:
                        eventObserver = null;
                        break;
                }

                if (eventObserver == null)
                {
                    throw new Exception();
                }

                eventObserver.Prepare(_instance._diContainer);
                _instance._cachedObservers.Add(gameEventType, eventObserver);
                return eventObserver;
            }
        }
    }
}