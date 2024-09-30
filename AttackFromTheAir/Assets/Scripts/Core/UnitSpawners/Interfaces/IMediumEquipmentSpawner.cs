using Core.Buildings;

namespace Core.GameLogic
{
    public interface IMediumEquipmentSpawner : IUnitSpawner
    {
        MediumEquipmentType MediumEquipmentType { get; }
    }
}