using Core.Events;
using Core.Level;

namespace Core.GameLogic
{
    public class PrisonerKilledEventObserver : BaseGameEventObserver
    {
        private const string PRISONER_TAG = "Prisoner";

        public override GameEventType GameEventType => GameEventType.PrisonerKilled;

        public override void StartObserve()
        {
            EventAggregator.Subscribe<UnitDeadEvent>(OnUnitDead);
        }

        private void OnUnitDead(object sender, UnitDeadEvent data)
        {
            if (data.Unit.UnitType == Units.UnitType.People && data.Unit.UnitFraction == Units.UnitFraction.Ally && data.Unit.UnitView.gameObject.CompareTag(PRISONER_TAG))
            {
                RaiseGameEvent();
            }
        }

        public override void StopObserve()
        {
            EventAggregator.Unsubscribe<UnitDeadEvent>(OnUnitDead);
        }
    }
}