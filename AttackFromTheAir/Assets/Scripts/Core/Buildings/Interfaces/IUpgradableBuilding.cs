using Core.UI;

namespace Core.Buildings
{
    public interface IUpgradableBuilding : IBuilding
    {
        IPropertyReadOnly<int> Level { get; }
        void SetLevel(int level);
        void Upgrade();
    }
}