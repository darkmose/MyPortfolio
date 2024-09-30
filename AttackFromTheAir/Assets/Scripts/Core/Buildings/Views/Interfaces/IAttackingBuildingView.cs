using Core.GameLogic;

namespace Core.Buildings
{
    public interface IAttackingBuildingView : IUpgradableBuildingView 
    {
        AttackBuildingType AttackBuildingType { get; }
        void OnStartAttacking(IDamagableObject damagableObject);
        void OnStopAttacking();
    }
}