using Core.Buildings;
using Core.Units;

namespace Core.GameLogic
{
    public abstract class BaseMediumEquipmentSpawner : BaseUnitSpawner, IMediumEquipmentSpawner
    {
        public override UnitSpawnerType UnitSpawnerType => UnitSpawnerType.MediumEquipment;
        protected override UnitCategory UnitCategory => UnitCategory.MediumEquipment;
        public abstract MediumEquipmentType MediumEquipmentType { get; }
    }
}