using Core.Buildings;

namespace Core.GameLogic
{
    public class MilitaryTruckMediumEquipmentSpawnerView : BaseMediumEquipmentSpawnerView
    {
        public override MediumEquipmentType MediumEquipmentType => MediumEquipmentType.MilitaryTruck;

        private void Awake()
        {
            _prefab = _unitsHolder.GetMediumEquipmentUnit(MediumEquipmentType.MilitaryTruck);
        }
    }
}