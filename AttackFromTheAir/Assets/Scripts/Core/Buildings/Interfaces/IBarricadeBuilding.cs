namespace Core.Buildings
{
    public interface IBarricadeBuilding : IUpgradableBuilding
    {
        BarricadeType BarricadeType { get; }
    }
}