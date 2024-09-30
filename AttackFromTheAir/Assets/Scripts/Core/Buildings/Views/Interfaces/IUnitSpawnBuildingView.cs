using Core.GameLogic;

namespace Core.Buildings
{
    public interface IUnitSpawnBuildingView : IUpgradableBuildingView
    {
        void InitSpawner(IUnitSpawnerView spawner);
        IUnitSpawnerView Spawner { get; }
    }
}