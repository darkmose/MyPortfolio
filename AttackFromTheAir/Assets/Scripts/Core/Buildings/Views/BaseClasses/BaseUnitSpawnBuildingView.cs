using Core.GameLogic;

namespace Core.Buildings
{
    public abstract class BaseUnitSpawnBuildingView : BaseUpgradableBuildingView, IUnitSpawnBuildingView
    {
        private IUnitSpawnerView _spawner;
        public IUnitSpawnerView Spawner => _spawner;

        public virtual void InitSpawner(IUnitSpawnerView spawner)
        {
            if (spawner is BaseUnitSpawnerView unitSpawnerView)
            {
                unitSpawnerView.transform.position = this.transform.position;
            }
            _spawner = spawner;
        }
    }
}