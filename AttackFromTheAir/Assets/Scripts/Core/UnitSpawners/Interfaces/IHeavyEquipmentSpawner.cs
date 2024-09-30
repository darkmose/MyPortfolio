using Core.Buildings;

namespace Core.GameLogic
{
    public interface IHeavyEquipmentSpawner : IUnitSpawner
    {
        HeavyEquipmentType HeavyEquipmentType { get; }
    }
}