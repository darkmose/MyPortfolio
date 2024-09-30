using Core.Events;
using Core.Level;

namespace Core.GameLogic
{
    public class EnemyHeavyEquipmentDestroyedEventObserver : BaseGameEventObserver
    {
        public override GameEventType GameEventType => GameEventType.EnemyHeavyEquipmentDestroyed;

        public override void StartObserve()
        {
            EventAggregator.Subscribe<UnitDeadEvent>(OnUnitDead);
        }

        private void OnUnitDead(object sender, UnitDeadEvent data)
        {
            if (data.Unit.UnitCategory == Units.UnitCategory.HeavyEquipment && data.Unit.UnitFraction == Units.UnitFraction.Enemy)
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