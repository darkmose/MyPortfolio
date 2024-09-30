using Core.Buildings;

namespace Core.GameLogic
{
    public interface IHeavyEquipmentSpawnerView : IUnitSpawnerView
    {
        HeavyEquipmentType HeavyEquipmentType { get; }
    }
}