using Core.Level;
using Core.Tools;
using DG.Tweening;

namespace Core.GameLogic
{
    public class TestTimeDelayEventObserver : BaseGameEventObserver
    {
        private Tweener _timerTweener;
        public override GameEventType GameEventType => GameEventType.TestTimeDelay;

        public override void StartObserve()
        {
            _timerTweener = Timer.SetTimer(1f, OnTimerOver);
        }

        private void OnTimerOver()
        {
            RaiseGameEvent();
            _timerTweener?.Kill();
            _timerTweener = Timer.SetTimer(1f, OnTimerOver);
        }

        public override void StopObserve()
        {
            _timerTweener?.Kill();
        }
    }
}