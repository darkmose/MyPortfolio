using Core.Buildings;
using Core.Units;
using Core.Weapon;
using Zenject;

namespace Core.GameLogic
{
    public class T90HeavyEquipmentSpawner : BaseHeavyEquipmentSpawner
    {
        private WeaponCreateSystem _weaponCreateSystem;
        private DiContainer _diContainer;
        public override HeavyEquipmentType HeavyEquipmentType => HeavyEquipmentType.T90;

        public override void Prepare(DiContainer diContainer)
        {
            base.Prepare(diContainer);
            _weaponCreateSystem = diContainer.Resolve<WeaponCreateSystem>();
            _diContainer = diContainer;
        }

        protected override IUnitStateMachine CreateStateMachine()
        {
            return new AttackingVehicleStateMachine(_diContainer);
        }

        protected override IUnit CreateUnitModel()
        {
            var unit = new VehicleAttackingUnit();
            unit.SetMaxHealth(150);
            unit.SetHealth(150);
            return unit;
        }

        protected override IWeapon CreateWeapon()
        {
            var weapon = _weaponCreateSystem.GetExploProjectileWeapon(ExploProjectileWeaponType.TankTurret);
            return weapon;
        }
    }
}