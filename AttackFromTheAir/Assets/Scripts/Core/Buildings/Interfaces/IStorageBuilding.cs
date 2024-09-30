namespace Core.Buildings
{
    public interface IStorageBuilding : IUpgradableBuilding
    {
        StorageBuildingType StorageBuildingType { get; }
    }
}