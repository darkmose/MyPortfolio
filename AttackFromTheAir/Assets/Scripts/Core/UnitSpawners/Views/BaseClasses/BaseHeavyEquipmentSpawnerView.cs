using Core.Buildings;

namespace Core.GameLogic
{
    public abstract class BaseHeavyEquipmentSpawnerView : BaseUnitSpawnerView, IHeavyEquipmentSpawnerView
    {
        public override UnitSpawnerType UnitSpawnerType => UnitSpawnerType.HeavyEquipment;
        public abstract HeavyEquipmentType HeavyEquipmentType { get; }
    }
}