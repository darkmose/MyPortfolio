using Core.Buildings;
using Core.Units;
using Core.Weapon;
using Zenject;

namespace Core.GameLogic
{
    public class SoldierInfantrySpawner : BaseInfantrySpawner
    {
        private WeaponCreateSystem _weaponCreateSystem;
        private DiContainer _diContainer;
        public override InfantryType InfantryType => InfantryType.Soldier;

        public override void Prepare(DiContainer diContainer)
        {
            base.Prepare(diContainer);
            _weaponCreateSystem = diContainer.Resolve<WeaponCreateSystem>();
            _diContainer = diContainer;
        }

        protected override IUnitStateMachine CreateStateMachine()
        {
            return new AttackingPeopleStateMachine(_diContainer);
        }

        protected override IUnit CreateUnitModel()
        {
            var unit = new PeopleAttackingUnit();
            unit.SetMaxHealth(50);
            unit.SetHealth(50);
            return unit;
        }

        protected override IWeapon CreateWeapon()
        {
            var weapon = _weaponCreateSystem.GetBulletsWeapon(BulletsWeaponType.Rifle);
            return weapon;
        }
    }
}