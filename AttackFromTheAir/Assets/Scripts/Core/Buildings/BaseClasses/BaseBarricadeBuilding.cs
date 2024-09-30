namespace Core.Buildings
{
    public abstract class BaseBarricadeBuilding : BaseUpgradableBuilding, IBarricadeBuilding
    {
        public override BuildingType BuildingType => BuildingType.Barricade;
        public abstract BarricadeType BarricadeType { get; }
    }
}