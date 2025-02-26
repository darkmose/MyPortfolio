namespace Core.Buildings
{
    public class DroneSiteBuildingView : BaseUpgradableBuildingView
    {
        public override BuildingType BuildingType => BuildingType.DroneSite;

        public override void OnLevelChange(int level)
        {
        }
    }
}