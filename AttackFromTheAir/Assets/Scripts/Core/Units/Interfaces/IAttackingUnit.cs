using Core.GameLogic;
using Core.Utilities;
using Core.Weapon;

namespace Core.Units
{
    public interface IAttackingUnit : IUnit
    {
        IWeapon Weapon { get; }
        SimpleEvent<IDamagableObject> StartAttackingEvent { get; }
        SimpleEvent StopAttackingEvent { get; }
        void Attack(IDamagableObject damagableObject);
        void StopAttack();
        void InitWeapon(IWeapon weapon);
    }
}