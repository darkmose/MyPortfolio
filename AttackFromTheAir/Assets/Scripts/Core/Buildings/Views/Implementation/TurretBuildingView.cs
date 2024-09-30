using Core.GameLogic;

namespace Core.Buildings
{
    public class TurretBuildingView : BaseAttackingBuildingView, IAttackingBuildingView
    {
        public override AttackBuildingType AttackBuildingType => AttackBuildingType.Turret;

        public override void OnLevelChange(int level)
        {
            throw new System.NotImplementedException();
        }

        public override void OnObjectDamaged(IDamagableObject damagableObject)
        {
            throw new System.NotImplementedException();
        }

        public override void OnObjectDestroy(IDamagableObject damagableObject)
        {
            throw new System.NotImplementedException();
        }

        public override void OnStartAttacking(IDamagableObject damagableObject)
        {
            throw new System.NotImplementedException();
        }

        public override void OnStopAttacking()
        {
            throw new System.NotImplementedException();
        }

        public override void SetHealthNormalized(float health)
        {
            throw new System.NotImplementedException();
        }
    }
}