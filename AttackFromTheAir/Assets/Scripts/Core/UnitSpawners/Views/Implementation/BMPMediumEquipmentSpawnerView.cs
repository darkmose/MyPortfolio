using Core.Buildings;

namespace Core.GameLogic
{
    public class BMPMediumEquipmentSpawnerView : BaseMediumEquipmentSpawnerView
    {
        public override MediumEquipmentType MediumEquipmentType => MediumEquipmentType.BMP;

        private void Awake()
        {
            _prefab = _unitsHolder.GetMediumEquipmentUnit(MediumEquipmentType.BMP);
        }
    }
}