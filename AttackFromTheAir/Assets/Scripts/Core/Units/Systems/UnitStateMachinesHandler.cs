using Core.Events;
using Core.Level;
using System;
using System.Collections.Generic;

namespace Core.Units
{
    public class UnitStateMachinesHandler
    {
        private readonly ILevelController _levelController;
        private List<IUnit> _units = new List<IUnit>();

        public UnitStateMachinesHandler(ILevelController levelController)
        {
            _levelController = levelController;
            _levelController.LevelLoadedEvent.AddListener(OnLevelLoaded);
            EventAggregator.Subscribe<GameTriggerEvent>(OnGameTrigger);
        }

        private void OnLevelLoaded()
        {
            _units = _levelController.UnitsCollector.GetAllUnits();
        }

        private void OnGameTrigger(object sender, GameTriggerEvent data)
        {
            var gameTriggerType = data.GameTrigger.TriggerType;
            var gameTriggerIndex = data.GameTrigger.ID;

            foreach (var unit in _units)
            {
                var states = unit.StateMachine.TriggerToUnitStateMatrix.GetUnitStatesByTrigger(gameTriggerType, gameTriggerIndex);

                if (states != null)
                {
                    foreach (var state in states)
                    {
                        unit.StateMachine.SwitchToState(state);
                    }
                }
            }
        }

        public void StartStateMachines()
        {
            foreach (var unit in _units) 
            {
                var initState = unit.StateMachine.InitialState;
                if (unit.StateMachine.HasState(initState))
                {
                    unit.StateMachine.SwitchToState(initState);
                }
                else
                {
                    unit.StateMachine.SwitchToState(UnitStates.Idle);
                }
            }
        }
    }
}