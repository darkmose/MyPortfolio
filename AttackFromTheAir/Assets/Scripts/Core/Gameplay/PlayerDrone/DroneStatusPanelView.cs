using Core.Resourses;
using Core.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GameLogic
{
    public class DroneStatusPanelView : MonoBehaviour
    {
        [SerializeField] private Button _selectButton;
        [SerializeField] private TextMeshProUGUI _availabilityInfo;
        [SerializeField] private TextMeshProUGUI _availableDroneCount;
        [SerializeField] private Image _droneIcon;
        [SerializeField] private Color _availableButtonColor;
        [SerializeField] private Color _unavailableButtonColor;
        [SerializeField] private Color _unavailableInfoColor;
        [SerializeField] private Color _availableInfoColor;
        [SerializeField] private Image _infoPanel;
        [SerializeField] private Image _buttonPanel;
        [SerializeField] private GameObject _advButtonGroup;
        [SerializeField] private GameObject _noAdvButtonGroup;
        [SerializeField] private DroneProvider _droneProvider;
        [SerializeField] private GameObject _lockPanel;
        [SerializeField] private GameObject _selection;

        public PlayerDroneType DroneType { get; private set; }
        public SimpleEvent<DroneStatusPanelView> DronePanelClickEvent { get; } = new SimpleEvent<DroneStatusPanelView>();

        private void OnEnable()
        {
            _selectButton.onClick.AddListener(()=>DronePanelClickEvent.Notify(this));
        }

        private void OnDisable()
        {
            _selectButton.onClick.RemoveAllListeners();
        }

        private void OnDestroy()
        {
            DronePanelClickEvent.RemoveAllListeners();
        }

        public void SetSelection(bool selected)
        {
            _selection.SetActive(selected);
        }

        public void SetLocked(bool locked)
        {
            _lockPanel.SetActive(locked);
        }

        public void SetDroneType(PlayerDroneType droneType)
        {
            DroneType = droneType;
            var droneConfig = _droneProvider.ProvideByType(droneType);
            _droneIcon.sprite = droneConfig.DroneIcon;
        }

        public void SetIcon(Sprite sprite)
        {
            _droneIcon.sprite = sprite;
        }

        public void SetAvailabilityInfo(string info)
        {
            _availabilityInfo.text = info;
        }

        public void SetAvailableCount(int count)
        {
            _availableDroneCount.text = count.ToString("0 AVAILABLE");
        }

        public void SetDroneAvailability(bool droneAvailability)
        {
            var buttonColor = droneAvailability ? _availableButtonColor : _unavailableButtonColor;
            var infoPanelColor = droneAvailability ? _availableInfoColor : _unavailableInfoColor;
            _infoPanel.color = infoPanelColor;
            _buttonPanel.color = buttonColor;
            _noAdvButtonGroup.SetActive(droneAvailability);
            _advButtonGroup.SetActive(!droneAvailability);
            _availabilityInfo.gameObject.SetActive(!droneAvailability);
            _availableDroneCount.gameObject.SetActive(droneAvailability);
        }
    }
}