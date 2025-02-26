using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GameLogic
{
    public class ConnectionErrorPanel : MonoBehaviour
    {
        private const float ERROR_ICON_ANIMATION_DURATION = 1f;
        private const float CANVAS_GROUP_ANIMATION_DURATION = 2f;
        [SerializeField] private Color _startErrorColor;
        [SerializeField] private Color _endErrorColor;
        [SerializeField] private Image _errorIcon;
        [SerializeField] private CanvasGroup _canvasGroup;
        private Sequence _errorAnimationSequence;

        public void StartErrorAnimation()
        {
            _canvasGroup.gameObject.SetActive(true);
            var iconAnimation = IconAnimation();
            var canvasGroupAnimation = CanvasGroupAnimation();
            _errorAnimationSequence = DOTween.Sequence();
            _errorAnimationSequence.Insert(0, iconAnimation);
            _errorAnimationSequence.Insert(0, canvasGroupAnimation);
            _errorAnimationSequence.SetLoops(int.MaxValue, LoopType.Restart);
            _errorAnimationSequence.Play();
        }

        private Tweener IconAnimation()
        {
            _errorIcon.color = _startErrorColor;
            return _errorIcon.DOColor(_endErrorColor, ERROR_ICON_ANIMATION_DURATION).SetLoops(-1, LoopType.Yoyo);
        }

        private Tweener CanvasGroupAnimation()
        {
            _canvasGroup.alpha = 0f;
            return _canvasGroup.DOFade(1f, CANVAS_GROUP_ANIMATION_DURATION).SetLoops(-1, LoopType.Yoyo);
        }

        public void StopErrorAnimation()
        {
            _errorAnimationSequence?.Kill();
            _errorIcon.color = _startErrorColor;
            _canvasGroup.alpha = 1f;
            _canvasGroup.gameObject.SetActive(false);
        }
    }
}