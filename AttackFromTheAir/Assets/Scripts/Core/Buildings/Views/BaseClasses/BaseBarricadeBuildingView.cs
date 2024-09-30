namespace Core.Buildings
{
    public abstract class BaseBarricadeBuildingView : BaseUpgradableBuildingView, IBarricadeBuildingView
    {
        public override BuildingType BuildingType => BuildingType.Barricade;
    }
}