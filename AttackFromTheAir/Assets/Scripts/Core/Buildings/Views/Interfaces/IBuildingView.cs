using Core.GameLogic;

namespace Core.Buildings
{
    public interface IBuildingView : IDamagableObjectView
    {
        BuildingSystems BuildingSystems { get; }
        BuildingType BuildingType { get; }
    }
}