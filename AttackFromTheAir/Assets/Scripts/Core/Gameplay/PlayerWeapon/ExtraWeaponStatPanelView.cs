using Core.PlayerModule;
using Core.Tools;
using Core.Utilities;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.GameLogic
{
    public class ExtraWeaponStatPanelView : MonoBehaviour, IDisposable
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _statName;
        [SerializeField] private TextMeshProUGUI _currentLevelValue;
        [SerializeField] private TextMeshProUGUI _nextLevelValue;
        [SerializeField] private TextMeshProUGUI _softCurrencyCost;
        [SerializeField] private TextMeshProUGUI _hardCurrencyCost;
        [SerializeField] private Button _softCurrencyButton;
        [SerializeField] private Button _hardCurrencyButton;
        [SerializeField] private Button _advertisementButton;

        private string _softCostCurrencyFormat = string.Empty;
        private string _hardCostCurrencyFormat = string.Empty;

        public void SetSoftCostCurrency(MoneyType moneyType)
        {
            _softCostCurrencyFormat = CurrencyFormats.ProvideCurrencyFormat(moneyType);
        }

        public void SetHardCostCurrency(MoneyType moneyType)
        {
            _hardCostCurrencyFormat = CurrencyFormats.ProvideCurrencyFormat(moneyType);
        }

        public void InitSoftCurrencyButtonCallback(UnityAction callback)
        {
            _softCurrencyButton.onClick.AddListener(callback);
        }

        public void InitHardCurrencyButtonCallback(UnityAction callback)
        {
            _hardCurrencyButton.onClick.AddListener(callback);
        }

        public void InitAdvertisementButtonCallback(UnityAction callback)
        {
            _advertisementButton.onClick.AddListener(callback);
        }

        public void SetActiveAdvertisementButton(bool isActive)
        {
            _advertisementButton.gameObject.SetActive(isActive);
        }

        public void SetWeaponStatIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }

        public void SetSoftCurrencyCost(int cost)
        {
            _softCurrencyCost.text = cost.ToString() + _softCostCurrencyFormat;
        }

        public void SetHardCurrencyCost(int cost)
        {
            _hardCurrencyCost.text = cost.ToString() + _hardCostCurrencyFormat;
        }

        public void SetCurrentLevelValue(float value)
        {
            _currentLevelValue.text = value.ToString("0 >>");
        }

        public void SetNextLevelValue(float value)
        {
            _nextLevelValue.text = value.ToString(" 0");
        }

        public void SetWeaponStatName(string name)
        {
            _statName.text = name;
        }

        public void SetSoftCurrencyButtonInteractable(bool isInteractable)
        {
            _softCurrencyButton.interactable = isInteractable;
        }

        public void SetHardCurrencyButtonInteractable(bool isInteractable)
        {
            _hardCurrencyButton.interactable = isInteractable;
        }

        public void Dispose()
        {
            _softCurrencyButton.onClick.RemoveAllListeners();
            _hardCurrencyButton.onClick.RemoveAllListeners();
            _advertisementButton.onClick.RemoveAllListeners();
        }
    }
}