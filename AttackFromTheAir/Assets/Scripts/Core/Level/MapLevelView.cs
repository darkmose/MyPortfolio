using Core.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Level
{
    public class MapLevelView : MonoBehaviour
    {
        [SerializeField] private Button _mapLevelButton;
        [SerializeField] private GameObject _finishedMark;
        [SerializeField] private Color _availableColor;
        [SerializeField] private Color _nonAvailableColor;
        [SerializeField] private Color _finishedColor;
        [SerializeField] private Color _lockedColor;
        [SerializeField] private GameObject _selection;
        [SerializeField] private TextMeshProUGUI _levelNumber;
        [SerializeField] private Image _bg;
        [SerializeField] private GameObject _lockPanel;
        public SimpleEvent ClickEvent { get; } = new SimpleEvent();

        private void OnEnable()
        {
            _mapLevelButton.onClick.AddListener(() => ClickEvent.Notify());
        }

        private void OnDisable()
        {
            _mapLevelButton.onClick.RemoveAllListeners();
        }

        public void SetFinished(bool isFinished)
        {
            if (isFinished)
            {
                _bg.color = _finishedColor;
            }
            _finishedMark.SetActive(isFinished);
        }

        public void SetLockedStatus()
        {
            _lockPanel.SetActive(true);
            _bg.color = _lockedColor;
        }

        public void SetAvailable(bool isAvailable)
        {
            _bg.color = isAvailable ? _availableColor : _nonAvailableColor;
            _mapLevelButton.interactable = isAvailable;
        }

        public void SetSelected(bool isSelected)
        {
            _selection.SetActive(isSelected);
        }

        public void SetLevelNumber(int number)
        {
            _levelNumber.text = number.ToString();
        }
    }
}