using Core.Buildings;
using Core.Units;

namespace Core.GameLogic
{
    public abstract class BaseHeavyEquipmentSpawner : BaseUnitSpawner, IHeavyEquipmentSpawner
    {
        public override UnitSpawnerType UnitSpawnerType => UnitSpawnerType.HeavyEquipment;
        protected override UnitCategory UnitCategory => UnitCategory.HeavyEquipment;
        public abstract HeavyEquipmentType HeavyEquipmentType { get; }
    }
}