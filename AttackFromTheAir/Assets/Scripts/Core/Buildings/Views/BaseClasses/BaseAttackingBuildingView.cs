using Core.GameLogic;

namespace Core.Buildings
{
    public abstract class BaseAttackingBuildingView : BaseUpgradableBuildingView, IAttackingBuildingView
    {
        public override BuildingType BuildingType => BuildingType.AttackBuilding;
        public abstract AttackBuildingType AttackBuildingType { get; }
        public abstract void OnStartAttacking(IDamagableObject damagableObject);
        public abstract void OnStopAttacking();
    }
}