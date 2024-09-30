namespace Core.Units
{
    public abstract class BaseNonAttackingUnit : BaseUnit, INonAttackingUnit
    {
        public override UnitBehaviourType UnitBehaviourType => UnitBehaviourType.NonAttacking;

    }
}