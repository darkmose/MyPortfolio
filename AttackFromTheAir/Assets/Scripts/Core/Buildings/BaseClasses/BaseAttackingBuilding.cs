using Core.GameLogic;
using Core.Utilities;

namespace Core.Buildings
{
    public abstract class BaseAttackingBuilding : BaseUpgradableBuilding, IAttackingBuilding
    {
        private SimpleEvent<IDamagableObject> _startAttackingEvent;
        private SimpleEvent _stopAttackingEvent;
        public override BuildingType BuildingType => BuildingType.AttackBuilding;
        public abstract AttackBuildingType AttackBuildingType { get; }
        public SimpleEvent<IDamagableObject> StartAttackingEvent => _startAttackingEvent;
        public SimpleEvent StopAttacking => _stopAttackingEvent;

        public virtual void Attack(IDamagableObject target)
        {
            _startAttackingEvent.Notify(target);
        }

        public virtual void StopAttack()
        {
            _stopAttackingEvent.Notify();
        }
    }
}