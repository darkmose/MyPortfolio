using Core.Buildings;

namespace Core.GameLogic
{
    public class M1AbramsHeavyEquipmentSpawnerView : BaseHeavyEquipmentSpawnerView
    {
        public override HeavyEquipmentType HeavyEquipmentType => HeavyEquipmentType.M1Abrams;

        private void Awake()
        {
            _prefab = _unitsHolder.GetHeavyEquipmentUnit(HeavyEquipmentType.M1Abrams);
        }
    }
}