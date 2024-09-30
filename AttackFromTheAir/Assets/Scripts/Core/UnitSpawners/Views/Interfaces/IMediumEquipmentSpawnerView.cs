using Core.Buildings;

namespace Core.GameLogic
{
    public interface IMediumEquipmentSpawnerView : IUnitSpawnerView
    {
        MediumEquipmentType MediumEquipmentType { get; }
    }
}