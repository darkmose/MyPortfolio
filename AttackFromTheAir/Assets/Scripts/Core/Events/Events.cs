using Core.Buildings;
using Core.Level;
using Core.Tools;
using Core.Units;

namespace Core.Events
{
    public class StorageEvent { }

    public class GameEvent
    {
        public GameEventType GameEventType;
    }

    public class UnitWasSpawnedEvent
    {
        public IUnit Unit;
    }

    public class BuildingWasSpawnedEvent
    {
        public IBuilding Building;
    }

    public class UnitDeadEvent
    {
        public IUnit Unit;
    }

    public class BuildingDestroyedEvent
    {
        public IBuilding Building;
    }

    public class LevelDisposeEvent
    {
    }

    public class LevelStartRequestEvent
    {
    }

    public class GameTriggerEvent
    {
        public IGameTrigger GameTrigger;
    }
}