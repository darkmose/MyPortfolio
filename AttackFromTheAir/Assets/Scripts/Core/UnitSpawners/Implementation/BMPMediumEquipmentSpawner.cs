using Core.Buildings;
using Core.Units;
using Zenject;

namespace Core.GameLogic
{
    public class BMPMediumEquipmentSpawner : BaseMediumEquipmentSpawner
    {
        private DiContainer _diContainer;
        public override MediumEquipmentType MediumEquipmentType => MediumEquipmentType.BMP;

        protected override IUnitStateMachine CreateStateMachine()
        {
            return new NonAttackingVehicleStateMachine(_diContainer);
        }

        public override void Prepare(DiContainer diContainer)
        {
            base.Prepare(diContainer);
            _diContainer = diContainer;
        }

        protected override IUnit CreateUnitModel()
        {
            var unit = new VehicleNonAttackingUnit();
            unit.SetMaxHealth(20);
            unit.SetHealth(20);
            return unit;
        }
    }

}