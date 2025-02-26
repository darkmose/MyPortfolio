using Core.Buildings;
using Core.LobbyBase;
using Core.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{

    public class BaseBuildingTab : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _buildingName;
        [SerializeField] private Image _selectedForeground;
        [SerializeField] private Toggle _toggle;
        private BuildingType _buildingType;
        public bool IsOn => _toggle.isOn;
        public SimpleEvent<BuildingType, bool> TabStateChangeEvent { get; } = new SimpleEvent<BuildingType, bool>();
        public BuildingType BuildingType => _buildingType;

        private void OnEnable()
        {
            _toggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(bool value)
        {
            TabStateChangeEvent.Notify(_buildingType, value);
        }

        public void InitToggleGroup(ToggleGroup toggleGroup)
        {
            _toggle.group = toggleGroup;
        }

        public void ToggleOn()
        {
            _toggle.isOn = true;
        }

        public void InitBuildingType(BuildingType buildingType)
        {
            _buildingType = buildingType;
            _buildingName.text = buildingType.ToString();
        }

        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(OnValueChanged);
        }
    }
}