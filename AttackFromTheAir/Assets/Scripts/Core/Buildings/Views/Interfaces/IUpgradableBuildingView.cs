namespace Core.Buildings
{
    public interface IUpgradableBuildingView : IBuildingView
    {
        void OnLevelChange(int level);   
    }
}