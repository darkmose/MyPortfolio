using Core.Events;
using Core.Level;

namespace Core.GameLogic
{
    public class EnemyInfantryKilledEventObserver : BaseGameEventObserver
    {
        public override GameEventType GameEventType => GameEventType.EnemyInfantryKilled;

        public override void StartObserve()
        {
            EventAggregator.Subscribe<UnitDeadEvent>(OnUnitDead);
        }

        private void OnUnitDead(object sender, UnitDeadEvent data)
        {
            if (data.Unit.UnitCategory == Units.UnitCategory.Infantry && data.Unit.UnitFraction == Units.UnitFraction.Enemy)
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