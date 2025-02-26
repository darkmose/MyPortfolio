using Core.PlayerModule;
using Core.Resourses;
using Core.UI;
using Core.Utilities;
using System;

namespace Core.GameLogic
{
    public class PlayerExtraWeaponStatUpgradeDescriptor : IDisposable
    {
        private IntProperty _softCost = new IntProperty(0);
        private IntProperty _hardCost = new IntProperty(0);
        private IntProperty _statLevel = new IntProperty(0);
        private BoolProperty _softCurrencyButtonAvailable = new BoolProperty(false);
        private BoolProperty _hardCurrencyButtonAvailable = new BoolProperty(false);
        private BoolProperty _advertisementAvailable = new BoolProperty(false);
        private CustomProperty<MoneyType> _softCurrency = new CustomProperty<MoneyType>(MoneyType.Coins);
        private CustomProperty<MoneyType> _hardCurrency = new CustomProperty<MoneyType>(MoneyType.Cards);
        private SimpleEvent<PlayerExtraWeaponStatUpgradeDescriptor> _onSoftCurrencyButtonEvent = new SimpleEvent<PlayerExtraWeaponStatUpgradeDescriptor>();
        private SimpleEvent<PlayerExtraWeaponStatUpgradeDescriptor> _onHardCurrencyButtonEvent = new SimpleEvent<PlayerExtraWeaponStatUpgradeDescriptor>();
        private SimpleEvent<PlayerExtraWeaponStatUpgradeDescriptor> _onAdvertisementButtonEvent = new SimpleEvent<PlayerExtraWeaponStatUpgradeDescriptor>();
        private PlayerWeaponStats _playerWeaponStat;
        private CustomProperty<object> _value = new CustomProperty<object>(null);
        private CustomProperty<object> _nextLevelValue = new CustomProperty<object>(null);
        private PlayerExtraWeaponType _extraWeaponType;
        public PlayerWeaponStats PlayerWeaponStats => _playerWeaponStat;
        public IPropertyReadOnly<MoneyType> SoftCurrency => _softCurrency;
        public IPropertyReadOnly<MoneyType> HardCurrency => _hardCurrency;
        public IPropertyReadOnly<bool> AdvertisementAvailable => _advertisementAvailable;
        public IPropertyReadOnly<int> SoftCost => _softCost;
        public IPropertyReadOnly<int> HardCost => _hardCost;
        public IPropertyReadOnly<bool> SoftCurrencyButtonAvailable => _softCurrencyButtonAvailable;
        public IPropertyReadOnly<bool> HardCurrencyButtonAvailable => _hardCurrencyButtonAvailable;
        public SimpleEvent<PlayerExtraWeaponStatUpgradeDescriptor> OnSoftCurrencyButtonEvent => _onSoftCurrencyButtonEvent;
        public SimpleEvent<PlayerExtraWeaponStatUpgradeDescriptor> OnHardCurrencyButtonEvent => _onHardCurrencyButtonEvent;
        public SimpleEvent<PlayerExtraWeaponStatUpgradeDescriptor> OnAdvertisementButtonEvent => _onAdvertisementButtonEvent;
        public IPropertyReadOnly<object> Value => _value;
        public IPropertyReadOnly<object> NextLevelValue => _nextLevelValue;
        public IPropertyReadOnly<int> StatLevel => _statLevel;
        public PlayerExtraWeaponType ExtraWeaponType => _extraWeaponType;

        public PlayerExtraWeaponStatUpgradeDescriptor(PlayerWeaponStats playerWeaponStat, PlayerExtraWeaponType extraWeaponType)
        {
            _playerWeaponStat = playerWeaponStat;
            _extraWeaponType = extraWeaponType;
        }

        public void SetStatLevel(int level)
        {
            _statLevel.SetValue(level, false);
        }

        public void SetValue(object value)
        {
            _value.SetValue(value, false);
        }

        public void SetNextLevelValue(object value)
        {
            _nextLevelValue.SetValue(value, false);
        }
        
        public void SetSoftCurrency(MoneyType softCurrency)
        {
            _softCurrency.SetValue(softCurrency, false);
        }

        public void SetHardCurrency(MoneyType hardCurrency)
        {
            _hardCurrency.SetValue(hardCurrency, false);
        }

        public void SetSoftCost(int cost)
        {
            _softCost.SetValue(cost, false);
        }

        public void SetHardCost(int cost)
        {
            _hardCost.SetValue(cost, false);
        }

        public void SetSoftCurrencyAvailable(bool isAvailable)
        {
            _softCurrencyButtonAvailable.SetValue(isAvailable);
        }

        public void SetHardCurrencyAvailable(bool isAvailable)
        {
            _hardCurrencyButtonAvailable.SetValue(isAvailable);
        }

        public void SetAdvertisementAvailable(bool isAvailable)
        {
            _advertisementAvailable.SetValue(isAvailable);
        }

        public void OnSoftCurrencyBuyButtonClick()
        {
            OnSoftCurrencyButtonEvent.Notify(this);
        }

        public void OnHardCurrencyBuyButtonClick()
        {
            OnHardCurrencyButtonEvent.Notify(this);
        }

        public void OnAdvertisementBuyButtonClick()
        {
            OnAdvertisementButtonEvent.Notify(this);
        }

        public void Dispose()
        {
            _onSoftCurrencyButtonEvent.RemoveAllListeners();
            _onHardCurrencyButtonEvent.RemoveAllListeners();
            _onAdvertisementButtonEvent.RemoveAllListeners();

            _value.RemoveAllListeners();
            _softCost.RemoveAllListeners();
            _hardCost.RemoveAllListeners();
            _softCurrencyButtonAvailable.RemoveAllListeners();
            _hardCurrencyButtonAvailable.RemoveAllListeners();
            _advertisementAvailable.RemoveAllListeners();
            _softCurrency.RemoveAllListeners();
            _hardCurrency.RemoveAllListeners();
        }
    }
}