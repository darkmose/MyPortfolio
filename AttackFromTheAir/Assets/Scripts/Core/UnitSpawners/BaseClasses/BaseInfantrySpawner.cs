using Core.Buildings;
using Core.Units;

namespace Core.GameLogic
{
    public abstract class BaseInfantrySpawner : BaseUnitSpawner, IInfantrySpawner
    {
        public override UnitSpawnerType UnitSpawnerType => UnitSpawnerType.Infantry;
        protected override UnitCategory UnitCategory => UnitCategory.Infantry;
        public abstract InfantryType InfantryType { get; }
    }
}