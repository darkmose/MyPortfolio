using Core.Events;
using Core.Level;
using System;
using Zenject;

namespace Core.GameLogic
{
    public class PrisonerReleaseEventObserver : BaseGameEventObserver
    {
        public override GameEventType GameEventType => GameEventType.PrisonerReleased;

        private void OnUnitArrivedToSafePoint(object arg1, UnitArrivedToSafePointEvent @event)
        {
            var unit = @event.Unit;
            if (unit.UnitTag == Units.UnitTag.Prisoner)
            {
                if (unit.UnitFraction == Units.UnitFraction.Ally)
                {
                    RaiseGameEvent();
                }
            }
        }

        public override void StartObserve()
        {
            EventAggregator.Subscribe<UnitArrivedToSafePointEvent>(OnUnitArrivedToSafePoint);
        }

        public override void StopObserve()
        {
            EventAggregator.Unsubscribe<UnitArrivedToSafePointEvent>(OnUnitArrivedToSafePoint);
        }
    }
}