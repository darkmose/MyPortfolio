using Core.GameLogic;
using Core.PlayerModule;
using Core.Utilities;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MVP
{
    public class LobbyScreenArmyPageView : BaseView, IPageView
    {
        [SerializeField] private PlayerWeaponSelectorView _playerWeaponSelectorView;
        [Header("Upgrade")]
        [Space(10)]
        [SerializeField] private GameObject _upgradePanel;
        [SerializeField] private Button _upgradePanelButton;
        [SerializeField] private Button _upgradePanelCloseButton;
        [SerializeField] private Button _speedSoftUpgradeButton;
        [SerializeField] private Button _reloadSoftUpgradeButton;
        [SerializeField] private Button _armorSoftUpgradeButton;
        [SerializeField] private Button _speedHardUpgradeButton;
        [SerializeField] private Button _reloadHardUpgradeButton;
        [SerializeField] private Button _armorHardUpgradeButton;
        [SerializeField] private TextMeshProUGUI _droneName;
        [SerializeField] private TextMeshProUGUI _droneLevel;
        [SerializeField] private TextMeshProUGUI _dronePower;
        [SerializeField] private TextMeshProUGUI _speedValue;
        [SerializeField] private TextMeshProUGUI _reloadValue;
        [SerializeField] private TextMeshProUGUI _armorValue;
        [SerializeField] private TextMeshProUGUI _speedSoftCost;
        [SerializeField] private TextMeshProUGUI _speedHardCost;
        [SerializeField] private TextMeshProUGUI _reloadSoftCost;
        [SerializeField] private TextMeshProUGUI _reloadHardCost;
        [SerializeField] private TextMeshProUGUI _armorSoftCost;
        [SerializeField] private TextMeshProUGUI _armorHardCost;
        [SerializeField] private Button _nextDroneButton;
        [SerializeField] private Button _previousDroneButton;
        [SerializeField] private GameObject _droneUnlockedPanel;
        [SerializeField] private GameObject _droneLockedPanel;
        [SerializeField] private TextMeshProUGUI _playerLevelRequiredValue;

        private string _speedSoftCurrencyFormat;
        private string _reloadSoftCurrencyFormat;
        private string _armorSoftCurrencyFormat;
        private string _speedHardCurrencyFormat;
        private string _reloadHardCurrencyFormat;
        private string _armorHardCurrencyFormat;
        private CultureInfo _cultureInfo;
        public PlayerWeaponSelectorView PlayerWeaponSelectorView => _playerWeaponSelectorView;
        public override ViewType View => ViewType.Panel;

        public void InitPresenter(IPagePresenter presenter)
        {
            if (presenter is LobbyScreenArmyPagePresenter lobbyScreenArmyPagePresenter)
            {
                _speedSoftUpgradeButton.onClick.AddListener(lobbyScreenArmyPagePresenter.UseCases.OnSpeedSoftUpgradeButtonClick);
                _speedHardUpgradeButton.onClick.AddListener(lobbyScreenArmyPagePresenter.UseCases.OnSpeedHardUpgradeButtonClick);
                _reloadSoftUpgradeButton.onClick.AddListener(lobbyScreenArmyPagePresenter.UseCases.OnReloadSoftUpgradeButtonClick);
                _reloadHardUpgradeButton.onClick.AddListener(lobbyScreenArmyPagePresenter.UseCases.OnReloadHardUpgradeButtonClick);
                _armorSoftUpgradeButton.onClick.AddListener(lobbyScreenArmyPagePresenter.UseCases.OnArmorSoftUpgradeButtonClick);
                _armorHardUpgradeButton.onClick.AddListener(lobbyScreenArmyPagePresenter.UseCases.OnArmorHardUpgradeButtonClick);
                _nextDroneButton.onClick.AddListener(lobbyScreenArmyPagePresenter.UseCases.OnNextDroneButtonClick);
                _previousDroneButton.onClick.AddListener(lobbyScreenArmyPagePresenter.UseCases.OnPreviousDroneButtonClick);
                _upgradePanelButton.onClick.AddListener(lobbyScreenArmyPagePresenter.OnDroneUpgradeButtonClick);
                _upgradePanelCloseButton.onClick.AddListener(lobbyScreenArmyPagePresenter.OnDroneUpgradePanelCloseButtonClick);
            }
            _cultureInfo = CultureInfo.GetCultureInfo("en-US");
        }

        public void SetDroneName(string droneName)
        {
            _droneName.text = droneName;
        }

        public void SetDroneLevel(int droneLevel)
        {
            _droneLevel.text = droneLevel.ToString("LVL: 0");
        }

        public void SetDronePower(float dronePower)
        {
            _dronePower.text = dronePower.ToString("POWER: 0.0", _cultureInfo);
        }

        public void SetVisibleAvailableDronePanel(bool isVisible)
        {
            _droneUnlockedPanel.SetActive(isVisible);
            _droneLockedPanel.SetActive(!isVisible);
            if (!isVisible)
            {
                SetUpgradePanelVisible(false);
            }
        }

        public void SetDroneRequiredLevel(int droneRequiredLevel)
        {
            _playerLevelRequiredValue.text = droneRequiredLevel.ToString("<U>DRONE IS LOCKED</U>\r\n\r\nPLAYER LEVEL <color=red>0</COLOR> REQUIRED");
        }

        public void SetUpgradePanelVisible(bool isVisible)
        {
            _upgradePanel.SetActive(isVisible);
            _upgradePanelButton.gameObject.SetActive(!isVisible);
        }

        public void SetSpeedValue(float value)
        {
            _speedValue.text = value.ToString("SPEED: +0.0", _cultureInfo);
        }

        public void SetReloadValue(float value)
        {
            _reloadValue.text = value.ToString("RELOAD: 0.0ms", _cultureInfo);
        }

        public void SetArmorValue(float value)
        {
            _armorValue.text = value.ToString("ARMOR: +0.0", _cultureInfo);
        }

        public void SetSpeedSoftCost(int value)
        {
            _speedSoftCost.text = value.ToString(_speedSoftCurrencyFormat);
        }

        public void SetSpeedHardCost(int value)
        {
            _speedHardCost.text = value.ToString(_speedHardCurrencyFormat);
        }

        public void SetReloadSpeedSoftCost(int value)
        {
            _reloadSoftCost.text = value.ToString(_reloadSoftCurrencyFormat);
        }

        public void SetReloadSpeedHardCost(int value)
        {
            _reloadHardCost.text = value.ToString(_reloadHardCurrencyFormat);
        }

        public void SetArmorSoftCost(int value)
        {
            _armorSoftCost.text = value.ToString(_armorSoftCurrencyFormat); 
        }

        public void SetArmorHardCost(int value)
        {
            _armorHardCost.text = value.ToString(_armorHardCurrencyFormat);
        }

        public void SetSpeedSoftCurrency(MoneyType moneyType)
        {
            _speedSoftCurrencyFormat = CurrencyFormats.ProvideCurrencyFormat(moneyType);
        }

        public void SetSpeedHardCurrency(MoneyType moneyType)
        {
            _speedHardCurrencyFormat = CurrencyFormats.ProvideCurrencyFormat(moneyType);
        }

        public void SetReloadSoftCurrency(MoneyType moneyType)
        {
            _reloadSoftCurrencyFormat = CurrencyFormats.ProvideCurrencyFormat(moneyType);
        }

        public void SetReloadHardCurrency(MoneyType moneyType)
        {
            _reloadHardCurrencyFormat = CurrencyFormats.ProvideCurrencyFormat(moneyType);
        }

        public void SetArmorSoftCurrency(MoneyType moneyType)
        {
            _armorSoftCurrencyFormat = CurrencyFormats.ProvideCurrencyFormat(moneyType);
        }

        public void SetArmorHardCurrency(MoneyType moneyType)
        {
            _armorHardCurrencyFormat = CurrencyFormats.ProvideCurrencyFormat(moneyType);
        }

        public void SetInteractableSpeedSoftUpgradeButton(bool interactable)
        {
            _speedSoftUpgradeButton.interactable = interactable;
        }

        public void SetInteractableSpeedHardUpgradeButton(bool interactable)
        {
            _speedHardUpgradeButton.interactable = interactable;
        }

        public void SetInteractableReloadSoftUpgradeButton(bool interactable)
        {
            _reloadSoftUpgradeButton.interactable = interactable;
        }

        public void SetInteractableReloadHardUpgradeButton(bool interactable)
        {
            _reloadHardUpgradeButton.interactable = interactable;
        }

        public void SetInteractableArmorSoftUpgradeButton(bool interactable)
        {
            _armorSoftUpgradeButton.interactable = interactable;
        }

        public void SetInteractableArmorHardUpgradeButton(bool interactable)
        {
            _armorHardUpgradeButton.interactable = interactable;
        }
    }
}