namespace Core.Buildings
{
    public interface ISpecialBuildingView : IBuildingView
    {
        SpecialBuildingType SpecialBuildingType { get; }    
    }
}