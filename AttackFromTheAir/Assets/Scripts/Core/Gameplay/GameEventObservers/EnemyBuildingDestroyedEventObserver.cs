using Core.Events;
using Core.Level;

namespace Core.GameLogic
{
    public class EnemyBuildingDestroyedEventObserver : BaseGameEventObserver
    {
        public override GameEventType GameEventType => GameEventType.EnemyBuildingDestroyed;

        public override void StartObserve()
        {
            EventAggregator.Subscribe<BuildingDestroyedEvent>(OnBuildingDestroyed);
        }

        private void OnBuildingDestroyed(object sender, BuildingDestroyedEvent data)
        {
            if (data.Building.UnitFraction == Units.UnitFraction.Enemy)
            {
                RaiseGameEvent();
            }
        }

        public override void StopObserve()
        {
            EventAggregator.Unsubscribe<BuildingDestroyedEvent>(OnBuildingDestroyed);
        }
    }
}