using Core.Level;

namespace Core.GameLogic
{
    public class DummyGameEventObserver : BaseGameEventObserver
    {
        public override GameEventType GameEventType => 0;

        public override void StartObserve()
        {   
        }

        public override void StopObserve()
        {
        }
    }
}