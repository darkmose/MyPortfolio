using Core.Level;

namespace Core.Units
{
    public class PeopleAttackState : UnitAttackState
    {
        public PeopleAttackState(IUnit unit, ILevelController levelController, TargetContainer targetContainer) : base(unit, levelController, targetContainer)
        {
        }

        protected override void EnterInner()
        {
            AttackingPeopleUnitSystems peopleSystem = Unit.UnitView.UnitSystems as AttackingPeopleUnitSystems;
            peopleSystem.UnitAttackSystem.InitWeapon(Weapon);
            peopleSystem.UnitAttackSystem.InitTarget(TargetContainer.Target);
            peopleSystem.UnitAttackSystem.AttackTarget();
        }

        protected override void ExitInner()
        {
            AttackingPeopleUnitSystems peopleSystem = Unit.UnitView.UnitSystems as AttackingPeopleUnitSystems;
            peopleSystem.UnitAttackSystem.ResetAiming();
            peopleSystem.UnitAttackSystem.StopAttack();
        }
    }
}
