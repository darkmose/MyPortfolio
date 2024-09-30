namespace Core.Buildings
{
    public abstract class BaseSimpleBuildingView : BaseBuildingView
    {
        public override BuildingType BuildingType => BuildingType.Simple;
    }
}