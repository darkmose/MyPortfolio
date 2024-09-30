using Core.Buildings;

namespace Core.GameLogic
{
    public abstract class BaseInfantryUnitSpawnerView : BaseUnitSpawnerView, IInfantryUnitSpawnerView
    {
        public override UnitSpawnerType UnitSpawnerType => UnitSpawnerType.Infantry;
        public abstract InfantryType InfantryType { get; }
    }
}