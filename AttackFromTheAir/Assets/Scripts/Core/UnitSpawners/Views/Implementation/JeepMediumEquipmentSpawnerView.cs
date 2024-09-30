using Core.Buildings;

namespace Core.GameLogic
{
    public class JeepMediumEquipmentSpawnerView : BaseMediumEquipmentSpawnerView
    {
        public override MediumEquipmentType MediumEquipmentType => MediumEquipmentType.Jeep;

        private void Awake()
        {
            _prefab = _unitsHolder.GetMediumEquipmentUnit(MediumEquipmentType.Jeep);
        }
    }
}