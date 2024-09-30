using Core.UI;

namespace Core.Buildings
{
    public abstract class BaseUpgradableBuilding : BaseBuilding, IUpgradableBuilding
    {
        private IntProperty _level = new IntProperty(1);
        public abstract override BuildingType BuildingType { get; }

        public IPropertyReadOnly<int> Level => _level;

        public void SetLevel(int level)
        {
            _level.SetValue(level, true);
        }

        public void Upgrade()
        {
            var nextLevel = _level.Value + 1;
            _level.SetValue(nextLevel, true);
        }
    }
}