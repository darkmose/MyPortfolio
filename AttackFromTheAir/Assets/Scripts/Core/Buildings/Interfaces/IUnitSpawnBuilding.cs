using Core.GameLogic;

namespace Core.Buildings
{
    public interface IUnitSpawnBuilding : IUpgradableBuilding
    {
        void InitSpawner(IUnitSpawner spawner);
        IUnitSpawner Spawner { get; }
    }
}