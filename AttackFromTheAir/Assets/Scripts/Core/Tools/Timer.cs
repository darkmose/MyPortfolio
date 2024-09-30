using DG.Tweening;

namespace Core.Tools
{
    public static class Timer
    {
        public static Tweener SetTimer(float time, System.Action onComplete)
        {
            var currentTime = time;
            return DOTween.To(() => currentTime, newTime => currentTime = newTime, 0, time).OnComplete(() =>
            {
                onComplete?.Invoke();
            }).SetEase(Ease.Linear);
        }
        
        public static Tweener SetTimer(float time, System.Action onComplete, System.Action<float> onUpdateNormalized)
        {
            var currentTime = time;
            return DOTween.To(() => currentTime, newTime => currentTime = newTime, 0, time)
            .OnUpdate(() => 
            {
                var normalizedValue = 1f - currentTime/time;
                onUpdateNormalized?.Invoke(normalizedValue);
            })
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            })
            .SetEase(Ease.Linear); 
        }
    }
}