using Core.GameLogic;

namespace Core.Weapon
{
    public class RocketTurretView : BaseExploProjectileWeaponView
    {
        public override ExploProjectileWeaponType ExploProjectileWeaponType => ExploProjectileWeaponType.RocketTurret;

        public override void OnFire(IDamagableObject damagableObject)
        {
            throw new System.NotImplementedException();
        }

        public override void OnStopFire()
        {
            throw new System.NotImplementedException();
        }
    }
}