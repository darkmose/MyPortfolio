using Core.GameLogic;

namespace Core.Buildings
{
    public abstract class BaseUnitSpawnBuilding : BaseUpgradableBuilding, IUnitSpawnBuilding
    {
        private IUnitSpawner _unitSpawner;
        public abstract override BuildingType BuildingType { get; }
        public IUnitSpawner Spawner => _unitSpawner;

        public virtual void InitSpawner(IUnitSpawner spawner)
        {
            _unitSpawner = spawner;
        }
    }
}