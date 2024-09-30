using Core.PlayerModule;
using Core.Resourses;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.GameLogic
{
    public class WeaponUpgradePanelView : MonoBehaviour
    {
        [SerializeField] private Image _weaponIcon;
        [SerializeField] private Button _softCurrencyBuyButton;
        [SerializeField] private Button _hardCurrencyBuyButton;
        [SerializeField] private TextMeshProUGUI _softCurrencyCost;
        [SerializeField] private TextMeshProUGUI _hardCurrencyCost;
        [SerializeField] private TextMeshProUGUI _projectileSpeedStat;
        [SerializeField] private TextMeshProUGUI _reloadSpeedStat;
        [SerializeField] private TextMeshProUGUI _damageSpeedStat;
        [SerializeField] private TextMeshProUGUI _projectilesCountStat;
        [SerializeField] private TextMeshProUGUI _weaponRank;
        [SerializeField] private TextMeshProUGUI _weaponRequiredLevel;

        [SerializeField] private Image _damageStatIcon;
        [SerializeField] private Image _projectileSpeedStatIcon;
        [SerializeField] private Image _projectileCountStatIcon;
        [SerializeField] private Image _reloadSpeedStatIcon;

        private string _hardCurrencyFormat = "#";
        private string _softCurrencyFormat = "#";

        public void InitCallback(UnityAction onSoftCurrencyBuyAction, UnityAction onHardCurrencyBuyAction)
        {
            _softCurrencyBuyButton.onClick.AddListener(onSoftCurrencyBuyAction);
            _hardCurrencyBuyButton.onClick.AddListener(onHardCurrencyBuyAction);
        }

        public void SetDamageStatIcon(Sprite icon) 
        {
            _damageStatIcon.sprite = icon;
        }

        public void SetProjectileSpeedStatIcon(Sprite icon) 
        {
            _projectileSpeedStatIcon.sprite = icon;
        }

        public void SetProjectileCountStatIcon(Sprite icon) 
        {
            _projectileCountStatIcon.sprite = icon;
        }

        public void SetReloadSpeedStatIcon(Sprite icon)
        {
            _reloadSpeedStatIcon.sprite = icon;
        }

        public void InitWeaponSprite(Sprite sprite)
        {
            _weaponIcon.sprite = sprite;
        }

        public void SetSoftCurrencyCost(int cost)
        {
            if (cost == -1)
            {
                _softCurrencyCost.text = "---";
                return;
            }
            _softCurrencyCost.text = cost.ToString(_softCurrencyFormat);
        }

        public void SetHardCurrencyCost(int cost)
        {
            if (cost == -1)
            {
                _hardCurrencyCost.text = "---";
                return;
            }
            _hardCurrencyCost.text = cost.ToString(_hardCurrencyFormat);
        }

        public void SetWeaponRank(int rank) 
        {
            _weaponRank.text = rank.ToString("RANK 0");
        }

        public void SetWeaponRequiredLevel(int requiredLevel) 
        {
            _weaponRequiredLevel.text = requiredLevel.ToString("LEVEL 0");
        }

        public void SetProjectileSpeedStat(float speed)
        {
            _projectileSpeedStat.text = speed.ToString();
        }

        public void SetReloadSpeedStat(float speed)
        {
            _reloadSpeedStat.text = speed.ToString();
        }

        public void SetDamageStat(int damage)
        {
            _damageSpeedStat.text = damage.ToString();
        }

        public void SetProjectileCountStat(int count)
        {
            _projectilesCountStat.text = count.ToString();
        }

        public void SetSoftCurrencyAvailable(bool isAvailable)
        {
            _softCurrencyBuyButton.interactable = isAvailable;
        }

        public void SetHardCurrencyAvailable(bool isAvailable)
        {
            _hardCurrencyBuyButton.interactable = isAvailable;
        }

        public void SetSoftCurrency(MoneyType moneyType)
        {
            _softCurrencyFormat = GetStringFormatForMoney(moneyType);
        }

        public void SetHardCurrency(MoneyType moneyType)
        {
            _hardCurrencyFormat = GetStringFormatForMoney(moneyType);
        }

        private string GetStringFormatForMoney(MoneyType moneyType) 
        {
            switch (moneyType)
            {
                case MoneyType.Coins:
                    return "$#";
                case MoneyType.Gems:
                    return "#";
                case MoneyType.Hammers:
                    return "#";
                case MoneyType.Cards:
                    return "#";
                default:
                    return "#";
            }
        }
    }
}