using Core.Buildings;

namespace Core.GameLogic
{
    public class T90HeavyEquipmentSpawnerView : BaseHeavyEquipmentSpawnerView
    {
        public override HeavyEquipmentType HeavyEquipmentType => HeavyEquipmentType.T90;

        private void Awake()
        {
            _prefab = _unitsHolder.GetHeavyEquipmentUnit(HeavyEquipmentType.T90);
        }
    }
}