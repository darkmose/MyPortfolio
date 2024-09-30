using Core.Buildings;

namespace Core.GameLogic
{
    public class StrykerHeavyEquipmentSpawnerView : BaseHeavyEquipmentSpawnerView
    {
        public override HeavyEquipmentType HeavyEquipmentType => HeavyEquipmentType.Stryker;

        private void Awake()
        {
            _prefab = _unitsHolder.GetHeavyEquipmentUnit(HeavyEquipmentType.Stryker);
        }
    }
}