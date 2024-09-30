namespace Core.Buildings
{
    public abstract class BaseSimpleBuilding : BaseBuilding
    {
        public override BuildingType BuildingType => BuildingType.Special;
    }
}