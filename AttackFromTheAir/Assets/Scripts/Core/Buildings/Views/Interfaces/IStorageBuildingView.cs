namespace Core.Buildings
{
    public interface IStorageBuildingView : IUpgradableBuildingView
    {
        StorageBuildingType StorageBuildingType { get; }
    }
}