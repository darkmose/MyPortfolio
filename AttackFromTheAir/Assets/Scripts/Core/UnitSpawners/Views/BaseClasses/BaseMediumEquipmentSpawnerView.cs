using Core.Buildings;

namespace Core.GameLogic
{
    public abstract class BaseMediumEquipmentSpawnerView : BaseUnitSpawnerView, IMediumEquipmentSpawnerView
    {
        public override UnitSpawnerType UnitSpawnerType => UnitSpawnerType.MediumEquipment;
        public abstract MediumEquipmentType MediumEquipmentType { get; }
    }
}