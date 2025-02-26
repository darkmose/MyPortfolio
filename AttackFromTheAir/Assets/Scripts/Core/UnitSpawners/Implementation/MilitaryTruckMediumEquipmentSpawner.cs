using Core.Buildings;
using Core.Units;
using Zenject;

namespace Core.GameLogic
{
    public class MilitaryTruckMediumEquipmentSpawner : BaseMediumEquipmentSpawner
    {
        private DiContainer _diContainer;
        public override MediumEquipmentType MediumEquipmentType => MediumEquipmentType.MilitaryTruck;

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
            unit.SetMaxHealth(70);
            unit.SetHealth(70);
            return unit;
        }
    }

}