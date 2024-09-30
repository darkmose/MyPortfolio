using Core.Resourses;
using Core.Units;
using UnityEngine;

namespace Core.GameLogic
{
    public abstract class BaseUnitSpawnerView : MonoBehaviour, IUnitSpawnerView
    {
        [SerializeField] protected UnitsHolder _unitsHolder;
        protected BaseUnitView _prefab;
        public abstract UnitSpawnerType UnitSpawnerType { get; }

        public void OnUnitSpawn(IUnit unit, Transform point)
        {
            var view = Instantiate(_prefab);
            view.transform.position = point.position;
            view.transform.rotation = point.rotation;
            view.gameObject.name = $"{unit.UnitFraction}_{_prefab.name}_{view.GetHashCode()}";
            view.gameObject.tag = unit.UnitFraction.ToString(); 
            view.UnitSystems.MoveSystem.WarpAgentTo(point.position);
            unit.UnitView = view;
            unit.MoveToEvent.AddListener(view.OnMoveTo);
            unit.StopMovingEvent.AddListener(view.OnStopMoving);
            var damagable = unit as IDamagableObject;
            damagable.View = view as BaseDamagableObjectView;
            damagable.View.GetDamageEvent.AddListener(damagable.TakeDamage);
            damagable.View.ExplodedEvent.AddListener(damagable.HandleExplosion);
            damagable.ObjectDamaged.AddListener(damagable.View.OnObjectDamaged);
            damagable.ObjectDestroyed.AddListener(damagable.View.OnObjectDestroy);
            damagable.NormalizedHealth.RegisterValueChangeListener(damagable.View.SetHealthNormalized);
            view.Model = damagable;
            
            if (unit is IAttackingUnit attackingUnit && view is BaseAttackingUnitView attackingUnitView)
            {
                attackingUnitView.InitWeapon(attackingUnit.Weapon.WeaponView);
                attackingUnit.Weapon.WeaponView.WeaponFraction = unit.UnitFraction;
                attackingUnit.StartAttackingEvent.AddListener(attackingUnitView.OnStartAttacking);
                attackingUnit.StopAttackingEvent.AddListener(attackingUnitView.OnStopAttacking);
            }
        }
    }
}