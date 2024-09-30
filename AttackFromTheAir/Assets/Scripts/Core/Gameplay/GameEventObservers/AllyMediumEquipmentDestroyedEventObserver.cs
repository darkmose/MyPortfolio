using Core.Events;
using Core.Level;

namespace Core.GameLogic
{
    public class AllyMediumEquipmentDestroyedEventObserver : BaseGameEventObserver
    {
        public override GameEventType GameEventType => GameEventType.AllyMediumEquipmentDestroyed;

        public override void StartObserve()
        {
            EventAggregator.Subscribe<UnitDeadEvent>(OnUnitDead);
        }

        private void OnUnitDead(object sender, UnitDeadEvent data)
        {
            if (data.Unit.UnitCategory == Units.UnitCategory.MediumEquipment && data.Unit.UnitFraction == Units.UnitFraction.Ally)
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