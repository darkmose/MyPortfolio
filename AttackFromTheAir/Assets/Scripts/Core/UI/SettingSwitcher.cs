using Core.Utilities;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MVP
{
    public class SettingSwitcher : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _inactiveIcon;
        [SerializeField] private Sprite _activeIcon;

        [SerializeField] private RectTransform _state0_point;
        [SerializeField] private RectTransform _state1_point;
        [SerializeField] private RectTransform _handle;

        [SerializeField] private Image _switcherBG;
        [SerializeField] private Color _inactiveColor;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Button _toggleBtn;
        private bool _state;

        public SimpleEvent<bool> StateChangeEvent { get; } = new SimpleEvent<bool>();

        private void OnEnable()
        {
            _toggleBtn.onClick.AddListener(OnToggleClick);
        }

        private void OnToggleClick()
        {
            _state = !_state;
            SetState(_state);
            StateChangeEvent.Notify(_state);
        }

        private void OnDisable()
        {
            _toggleBtn.onClick.RemoveListener(OnToggleClick);
        }

        public void SetState(bool state)
        {
            _switcherBG.color = state ? _activeColor : _inactiveColor;
            _icon.sprite = state ? _activeIcon : _inactiveIcon;
            _handle.transform.localPosition = state ? _state1_point.localPosition : _state0_point.localPosition; 
        }
    }
}