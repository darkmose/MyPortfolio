using Core.Events;
using Core.GameLogic;
using Core.Weapon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Units
{
    public interface ILevelUnitsCollector
    {
        List<IUnit> AllyUnits { get; }
        List<IUnit> EnemyUnits { get; }
        void ClearUnits();
        void EnemyUnitsAction(Action<List<IUnit>> action);
        void AllyUnitsAction(Action<List<IUnit>> action);
        public List<IUnit> GetAllUnits();
    }

    public class LevelUnitsCollector : ILevelUnitsCollector
    {
        private WeaponCreateSystem _weaponCreateSystem;
        private readonly List<IUnit> _allyUnits;
        private readonly List<IUnit> _enemyUnits;
        public List<IUnit> AllyUnits => _allyUnits;
        public List<IUnit> EnemyUnits => _enemyUnits;

        public LevelUnitsCollector(WeaponCreateSystem weaponCreateSystem)
        {
            _allyUnits = new List<IUnit>();
            _enemyUnits = new List<IUnit>();
            _weaponCreateSystem = weaponCreateSystem;
            EventAggregator.Subscribe<UnitWasSpawnedEvent>(OnUnitSpawned);
        }

        private void OnUnitSpawned(object sender, UnitWasSpawnedEvent data)
        {
            if (data.Unit.UnitFraction == UnitFraction.Ally)
            {
                _allyUnits.Add(data.Unit);
            }
            else 
            { 
                _enemyUnits.Add(data.Unit);
            }            
        }

        public void ClearUnits()
        {
            foreach (var unit in _allyUnits) 
            {
                DestroyUnit(unit);
            }
            foreach (var unit in _enemyUnits)
            {
                DestroyUnit(unit);
            }
            _allyUnits.Clear();
            _enemyUnits.Clear();
        }

        private void DestroyUnit(IUnit unit)
        {
            var view = unit.UnitView;
            unit.MoveToEvent.RemoveAllListeners();
            unit.StopMovingEvent.RemoveAllListeners();
            var damagable = unit as IDamagableObject;
            damagable.ObjectDamaged.RemoveAllListeners();
            damagable.ObjectDestroyed.RemoveAllListeners();
            damagable.View.GetDamageEvent.RemoveAllListeners();
            damagable.View.ExplodedEvent.RemoveAllListeners();
            damagable.NormalizedHealth.UnregisterValueChangeListener(damagable.View.SetHealthNormalized);
            if (unit is IAttackingUnit attackingUnit)
            {
                attackingUnit.StartAttackingEvent.RemoveAllListeners();
                attackingUnit.StopAttackingEvent.RemoveAllListeners();
                var weapon = attackingUnit.Weapon;
                _weaponCreateSystem.DestroyWeapon(weapon);
            }
            GameObject.Destroy(view.gameObject);
        }

        public void EnemyUnitsAction(Action<List<IUnit>> action)
        {
            action?.Invoke(_enemyUnits);
        }

        public void AllyUnitsAction(Action<List<IUnit>> action)
        {
            action?.Invoke(_allyUnits);
        }

        public List<IUnit> GetAllUnits()
        {
            var units = new List<IUnit>();
            units.AddRange(_enemyUnits);
            units.AddRange(_allyUnits);
            return units;
        }
    }
}