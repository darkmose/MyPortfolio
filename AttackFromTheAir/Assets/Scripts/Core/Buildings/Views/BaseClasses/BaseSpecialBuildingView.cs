namespace Core.Buildings
{
    public abstract class BaseSpecialBuildingView : BaseBuildingView, ISpecialBuildingView
    {
        public abstract SpecialBuildingType SpecialBuildingType { get; }
    }
}