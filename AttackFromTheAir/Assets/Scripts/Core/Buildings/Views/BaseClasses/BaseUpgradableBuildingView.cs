namespace Core.Buildings
{
    public abstract class BaseUpgradableBuildingView : BaseBuildingView, IUpgradableBuildingView
    {
        public abstract void OnLevelChange(int level);
    }
}