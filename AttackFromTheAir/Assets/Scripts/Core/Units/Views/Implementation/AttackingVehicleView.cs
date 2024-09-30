using Core.GameLogic;
using Core.Units;
using UnityEngine;

namespace Core.UnitsImplementation
{
    public class AttackingVehicleView : BaseAttackingUnitView
    {
        [SerializeField] private AttackingVehicleUnitSystems _systems;
        public override UnitType UnitType => UnitType.Vehicle;
        public override BaseUnitSystems UnitSystems => _systems;

        public override void OnMoveTo(Vector3 pos)
        {
            _systems.MoveSystem.MoveTo(pos);
        }

        public override void OnObjectDamaged(IDamagableObject damagableObject)
        {
        }

        public override void OnObjectDestroy(IDamagableObject damagableObject)
        {
        }

        public override void OnStartAttacking(IDamagableObject unit)
        {
        }

        public override void OnStopAttacking()
        {
        }

        public override void OnStopMoving()
        {
            _systems.MoveSystem.StopMove();
        }

        public override void SetHealthNormalized(float health)
        {
        }
    }
}