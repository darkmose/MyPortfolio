using Core.Level;

namespace Core.Units
{
    public class VehicleAttackState : UnitAttackState
    {
        public VehicleAttackState(IUnit unit, ILevelController levelController, TargetContainer targetContainer) : base(unit, levelController, targetContainer)
        {
        }

        protected override void EnterInner()
        {
            AttackingVehicleUnitSystems vehicleSystem = Unit.UnitView.UnitSystems as AttackingVehicleUnitSystems;
            vehicleSystem.UnitAttackSystem.InitWeapon(Weapon);
            vehicleSystem.UnitAttackSystem.InitTarget(TargetContainer.Target);
            vehicleSystem.UnitAttackSystem.AttackTarget();
        }

        protected override void ExitInner()
        {
            AttackingVehicleUnitSystems vehicleSystem = Unit.UnitView.UnitSystems as AttackingVehicleUnitSystems;
            vehicleSystem.UnitAttackSystem.ResetAiming();
            vehicleSystem.UnitAttackSystem.StopAttack();
        }
    }
}
