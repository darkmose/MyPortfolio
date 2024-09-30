using Core.Events;
using Core.Level;
using Core.Tools;
using Core.Units;
using Core.Utilities;
using Core.Weapon;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.GameLogic
{
    public abstract class BaseUnitSpawner : IUnitSpawner
    {
        private UnitWasSpawnedEvent _unitWasSpawnedEvent = new UnitWasSpawnedEvent();
        private SimpleEvent<IUnit, Transform> _unitSpawnedEvent = new SimpleEvent<IUnit, Transform>();
        protected abstract UnitCategory UnitCategory { get; }
        public abstract UnitSpawnerType UnitSpawnerType { get; }
        public SimpleEvent<IUnit, Transform> UnitSpawnedEvent => _unitSpawnedEvent;
        public IUnitSpawnerView View { get; set; }

        public virtual void Prepare(DiContainer diContainer)
        {
        }

        public void Spawn(UnitFraction unitFraction, UnitInitStateDescriptor initState, Transform position)
        {
            var unit = CreateUnitModel();
            unit.UnitFraction = unitFraction;
            unit.UnitCategory = UnitCategory;

            if (unit is IAttackingUnit)
            {
                var attackingUnit = (IAttackingUnit) unit;
                var weapon = CreateWeapon();
                attackingUnit.InitWeapon(weapon);
            }
            var stateMachine = CreateStateMachine();
            stateMachine.InitStateMachine(unit);

            unit.StateMachine = stateMachine;
            _unitSpawnedEvent.Notify(unit, position);
            _unitWasSpawnedEvent.Unit = unit;
            stateMachine.InitialState = initState.UnitInitState;    
            stateMachine.TriggerToUnitStateMatrix = initState.TriggerToUnitStateMatrix;
            EventAggregator.Post(this, _unitWasSpawnedEvent);
        }

        private void StateTriggerEventHandler(IUnitStateMachine stateMachine, List<UnitStates> states, IUnit unit)
        {
            if (unit.IsDestroyed) 
            {
                return;
            }
            foreach (var state in states) 
            {
                stateMachine.SwitchToState(state);
            }
        }

        protected abstract IUnit CreateUnitModel();
        protected abstract IUnitStateMachine CreateStateMachine();
        protected virtual IWeapon CreateWeapon()
        {
            return null;
        }
    }
}