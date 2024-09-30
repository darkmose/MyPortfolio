using Core.GameLogic;
using Core.Utilities;
using Core.Weapon;

namespace Core.Units
{
    public abstract class BaseAttackingUnit : BaseUnit, IAttackingUnit
    {
        private SimpleEvent<IDamagableObject> _startAttackingEvent = new SimpleEvent<IDamagableObject>();
        private SimpleEvent _stopAttackingEvent = new SimpleEvent();
        private IWeapon _weapon;
        public SimpleEvent<IDamagableObject> StartAttackingEvent => _startAttackingEvent;
        public SimpleEvent StopAttackingEvent => _stopAttackingEvent;
        public abstract override UnitType UnitType { get; }
        public override UnitBehaviourType UnitBehaviourType => UnitBehaviourType.Attacking;
        public IWeapon Weapon => _weapon;

        public virtual void Attack(IDamagableObject damagableObject)
        {
            _startAttackingEvent.Notify(damagableObject);   
        }

        public void InitWeapon(IWeapon weapon)
        {
            _weapon = weapon;
        }

        public virtual void StopAttack()
        {
            _stopAttackingEvent.Notify();
        }
    }
}