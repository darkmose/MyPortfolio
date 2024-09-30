namespace Core.Buildings
{
    public abstract class BaseStorageBuildingView : BaseUpgradableBuildingView, IStorageBuildingView
    {
        public override BuildingType BuildingType => BuildingType.Storage;
        public abstract StorageBuildingType StorageBuildingType { get; }
    }
}