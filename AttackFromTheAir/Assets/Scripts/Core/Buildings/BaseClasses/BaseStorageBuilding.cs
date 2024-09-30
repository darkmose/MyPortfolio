namespace Core.Buildings
{
    public abstract class BaseStorageBuilding : BaseUpgradableBuilding, IStorageBuilding
    {
        public override BuildingType BuildingType => BuildingType.Storage;
        public abstract StorageBuildingType StorageBuildingType { get; }
    }
}