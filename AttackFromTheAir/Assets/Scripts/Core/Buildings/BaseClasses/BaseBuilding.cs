using Core.GameLogic;
using Core.Units;

namespace Core.Buildings
{
    public abstract class BaseBuilding : BaseDamagableObject, IBuilding
    {
        public BaseBuildingView BuildingView { get; set; }
        public abstract BuildingType BuildingType { get; }
        public UnitFraction UnitFraction { get; set; }
    }
}