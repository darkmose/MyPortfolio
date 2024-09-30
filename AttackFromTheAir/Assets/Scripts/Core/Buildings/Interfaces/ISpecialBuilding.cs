namespace Core.Buildings
{
    public interface ISpecialBuilding : IBuilding
    {
        SpecialBuildingType SpecialBuildingType { get; }
    }
}