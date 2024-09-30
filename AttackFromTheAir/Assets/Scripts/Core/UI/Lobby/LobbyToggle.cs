using Core.Tools;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public enum LobbyTabs
    {
        Base,
        Weapon,
        Army,
        Extra,
        LevelStart
    }

    public class LobbyToggle : MonoBehaviour
    {
        private const float ANIMATION_DURATION = 0.3f;
        [SerializeField] private LobbyTabs lobbyTab;
        [SerializeField] private RectTransform _toggleRect;
        [SerializeField] private Toggle _toggle;
        [SerializeField] private Image _mainIcon;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private LayoutElement _layoutElement;
        [SerializeField] private Color _activeIconColor;
        private Color _normalIconColor = Color.white;
        private Sequence _activateAnimation;
        private float _startLayoutWidth;
        public event Action<LobbyTabs, bool> OnToggledEvent;
        public LobbyTabs LobbyTab => lobbyTab;
        public Toggle Toggle => _toggle;

        public void Init()
        {
            Timer.SetTimer(0.1f, () =>
            {
                _startLayoutWidth = _toggleRect.sizeDelta.x;
                if (_toggle.isOn)
                {
                    ForceActivate();
                }
            });
            _toggle.onValueChanged.AddListener(OnToggleStatusChanged);
        }

        private void OnToggleStatusChanged(bool isToggled)
        {
            if (isToggled)
            {
                StartActivationAnimation();
            }
            else
            {
                ResetAnimation();
            }
            OnToggledEvent?.Invoke(LobbyTab, isToggled);
        }

        private void ResetAnimation()
        {
            _activateAnimation?.Kill();
            _mainIcon.color = _normalIconColor;
            _layoutElement.enabled = false;
        }

        private void StartActivationAnimation()
        {
            _activateAnimation?.Complete();
            _activateAnimation?.Kill();

            var width = _toggleRect.sizeDelta.x;
            var expandedWidth = _startLayoutWidth * 1.7f;
            var extraExpandedWidth = _startLayoutWidth * 1.8f;

            var expand = DOTween.To(() => width, value => width = value, expandedWidth, ANIMATION_DURATION).OnUpdate(() => 
            {
                _layoutElement.preferredWidth = width;
            });

            _activateAnimation = DOTween.Sequence()
            .AppendCallback(() =>
            {
                _layoutElement.enabled = true;
            })
            .AppendCallback(() =>
            {
                _mainIcon.color = _activeIconColor;
            })
            .Append(expand).SetEase(Ease.Linear);
        }

        private void ForceActivate()
        {
            var expandedWidth = _startLayoutWidth * 1.7f;
            _layoutElement.enabled = true;
            _layoutElement.preferredWidth = expandedWidth;
            _mainIcon.color = _activeIconColor;
        }

        private void OnDestroy()
        {
            _toggle.onValueChanged.RemoveListener(OnToggleStatusChanged);
        }
    }
}