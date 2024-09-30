using Core.GameLogic;
using Core.Units;
using UnityEngine;

namespace Core.UnitsImplementation
{
    public class NonAttackingPeopleView : BaseNonAttackingUnitView
    {
        [SerializeField] private NonAttackingPeopleUnitSystems _systems;
        public override UnitType UnitType => UnitType.People;
        public override BaseUnitSystems UnitSystems => _systems;

        public override void OnMoveTo(Vector3 pos)
        {
            _systems.MoveSystem.MoveTo(pos);
        }

        public override void OnObjectDamaged(IDamagableObject damagableObject)
        {
            throw new System.NotImplementedException();
        }

        public override void OnObjectDestroy(IDamagableObject damagableObject)
        {
            throw new System.NotImplementedException();
        }

        public override void OnStopMoving()
        {
            _systems.MoveSystem.StopMove();
        }

        public override void SetHealthNormalized(float health)
        {
            throw new System.NotImplementedException();
        }
    }
}