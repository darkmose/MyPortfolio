using Core.Buildings;

namespace Core.GameLogic
{
    public class StrykerHeavyEquipmentSpawner : T90HeavyEquipmentSpawner
    {
        public override HeavyEquipmentType HeavyEquipmentType => HeavyEquipmentType.Stryker;
    }
}