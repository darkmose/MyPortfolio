namespace Core.Units
{
    public abstract class BaseNonAttackingUnitView : BaseUnitView, INonAttackingUnitView
    {
        public override UnitBehaviourType UnitBehaviourType => UnitBehaviourType.NonAttacking;
    }
}