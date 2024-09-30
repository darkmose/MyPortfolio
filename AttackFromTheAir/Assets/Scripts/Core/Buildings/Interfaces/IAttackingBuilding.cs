using Core.GameLogic;
using Core.Utilities;

namespace Core.Buildings
{
    public interface IAttackingBuilding : IUpgradableBuilding
    {
        SimpleEvent<IDamagableObject> StartAttackingEvent { get; }
        SimpleEvent StopAttacking { get; }
        AttackBuildingType AttackBuildingType { get; }
        void Attack(IDamagableObject target);
        void StopAttack();
    }
}