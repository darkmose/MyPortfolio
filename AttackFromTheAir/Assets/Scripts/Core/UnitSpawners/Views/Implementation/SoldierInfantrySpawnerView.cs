using Core.Buildings;

namespace Core.GameLogic
{
    public class SoldierInfantrySpawnerView : BaseInfantryUnitSpawnerView
    {
        public override InfantryType InfantryType => InfantryType.Soldier;

        private void Awake()
        {
            _prefab = _unitsHolder.GetInfantryUnit(InfantryType.Soldier);    
        }
    }
}