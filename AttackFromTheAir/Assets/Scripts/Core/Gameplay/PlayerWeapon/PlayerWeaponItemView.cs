using Core.Utilities;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.GameLogic
{
    public class PlayerWeaponItemView : MonoBehaviour
    {
        [SerializeField] private Image _weaponIcon;
        [SerializeField] private GameObject _lockObject;
        [SerializeField] private GameObject _selection;
        [SerializeField] private Button _itemButton;
        [SerializeField] private TextMeshProUGUI _requiredLevel;
        [SerializeField] private Image _panelBG;
        [SerializeField] private Color _warningColor;
        private Sequence _colorSequence;
        private Color _startColor;
        public PlayerWeaponType PlayerWeaponType { get; set; }
        public PlayerExtraWeaponType PlayerExtraWeaponType { get; set; }
        public SimpleEvent<PlayerWeaponItemView> ItemClicked { get; } = new SimpleEvent<PlayerWeaponItemView>();

        private void Awake()
        {
            _startColor = _panelBG.color;
            _itemButton.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            ItemClicked.Notify(this);
        }

        public void SetWeaponIcon(Sprite icon)
        {
            _weaponIcon.sprite = icon;
        }

        public void AnimateWarning()
        {
            _colorSequence?.Kill();
            var warningColor = _panelBG.DOColor(_warningColor, 0.3f);
            var colorBack = _panelBG.DOColor(_startColor, 0.3f);
            _colorSequence = DOTween.Sequence().Append(warningColor).Append(colorBack);
        }

        public void SetRequiredLevel(int level)
        {
            _requiredLevel.text = level.ToString("LVL 0");
        }

        public void SetLockedStatus(bool locked)
        {
            _lockObject.SetActive(locked);
            _itemButton.interactable = !locked;
        }

        public void Select()
        {
            _selection.SetActive(true);
        }

        public void Unselect()
        {
            _selection.SetActive(false);
        }

        private void OnDestroy()
        {            
            _itemButton.onClick.RemoveAllListeners();
        }
    }
}