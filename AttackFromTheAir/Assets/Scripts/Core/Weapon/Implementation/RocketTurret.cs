namespace Core.Weapon
{
    public class RocketTurret : BaseExploProjectileWeapon
    {
        public override ExploProjectileWeaponType ExploProjectileWeaponType => ExploProjectileWeaponType.RocketTurret;

        public RocketTurret(WeaponConfig weaponConfig) : base(weaponConfig)
        {
        }
    }
}