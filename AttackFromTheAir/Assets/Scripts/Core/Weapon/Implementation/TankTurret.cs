namespace Core.Weapon
{
    public class TankTurret : BaseExploProjectileWeapon
    {
        public override ExploProjectileWeaponType ExploProjectileWeaponType => ExploProjectileWeaponType.TankTurret;

        public TankTurret(WeaponConfig weaponConfig) : base(weaponConfig)
        {
        }
    }
}