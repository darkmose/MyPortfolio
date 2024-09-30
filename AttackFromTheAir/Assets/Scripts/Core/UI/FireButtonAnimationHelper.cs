using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class FireButtonAnimationHelper : MonoBehaviour 
    {
        public enum FireButtonState 
        {
            Active,
            Reloading
        }

        [SerializeField] private ProgressBarView _reloadBar;
        [SerializeField] private ProgressBarView _continuousFireBar;
        [SerializeField] private Image _weaponIcon;
        [SerializeField] private Image _fireButtonBackground;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _inactiveColor;
        [SerializeField] private Color _reloadColor;
        private Sequence _animationSequence;
        public ProgressBarView ReloadBar => _reloadBar;
        public ProgressBarView ContinuousFireBar => _continuousFireBar;

        private void SetReloadState()
        {
            _reloadBar.gameObject.SetActive(true);
            _fireButtonBackground.color = _inactiveColor;
            var bgColorToReload = _fireButtonBackground.DOColor(_reloadColor, 0.5f);
            var bgColorToInactive = _fireButtonBackground.DOColor(_inactiveColor, 0.5f);
            var weaponIconFadeOut = _weaponIcon.DOFade(0f, 0.5f);
            var weaponIconFadeIn = _weaponIcon.DOFade(1f, 0.5f);

            _animationSequence = DOTween.Sequence().Append(bgColorToReload).Join(weaponIconFadeOut).Append(bgColorToInactive).Join(weaponIconFadeIn).SetLoops(-1, LoopType.Restart);
        }

        private void SetActiveState()
        {
            _animationSequence?.Kill();
            _reloadBar.gameObject.SetActive(false);
            _fireButtonBackground.color = _activeColor;
            _weaponIcon.color = Color.white;
        }

        public void SetState(FireButtonState fireButtonState)
        {
            switch (fireButtonState)
            {
                case FireButtonState.Active:
                    SetActiveState();
                    break;
                case FireButtonState.Reloading:
                    SetReloadState();
                    break;
                default:
                    break;
            }
        }
    }
}