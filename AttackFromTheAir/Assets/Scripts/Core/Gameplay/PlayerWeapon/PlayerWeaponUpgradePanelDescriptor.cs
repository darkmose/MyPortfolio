using Core.PlayerModule;
using Core.UI;
using Core.Utilities;

namespace Core.GameLogic
{
    public class PlayerWeaponUpgradePanelDescriptor
    {
        private PlayerWeaponType _weaponType;
        private PlayerExtraWeaponType _extraWeaponType;
        private IntProperty _damageStat = new IntProperty(0);
        private IntProperty _projectilesCountStat = new IntProperty(1);
        private FloatProperty _projectilesSpeedStat = new FloatProperty();
        private FloatProperty _reloadSpeedStat = new FloatProperty();
        private CustomProperty<MoneyType> _softCurrency = new CustomProperty<MoneyType>(MoneyType.Coins);
        private CustomProperty<MoneyType> _hardCurrency = new CustomProperty<MoneyType>(MoneyType.Cards);
        private IntProperty _softCost = new IntProperty(0);
        private IntProperty _hardCost = new IntProperty(0);
        private IntProperty _weaponRank = new IntProperty(1);
        private IntProperty _weaponRequiredLevel = new IntProperty(1);
        private BoolProperty _softCurrencyButtonAvailable = new BoolProperty(false);
        private BoolProperty _hardCurrencyButtonAvailable = new BoolProperty(false);
        private SimpleEvent<PlayerWeaponUpgradePanelDescriptor> _onSoftCurrencyButtonEvent = new SimpleEvent<PlayerWeaponUpgradePanelDescriptor>();
        private SimpleEvent<PlayerWeaponUpgradePanelDescriptor> _onHardCurrencyButtonEvent = new SimpleEvent<PlayerWeaponUpgradePanelDescriptor>();
        public PlayerWeaponType WeaponType => _weaponType;
        public PlayerExtraWeaponType ExtraWeaponType => _extraWeaponType;
        public IPropertyReadOnly<int> DamageStat => _damageStat;
        public IPropertyReadOnly<int> ProjectilesCountStat => _projectilesCountStat;
        public IPropertyReadOnly<float> ProjectileSpeedStat => _projectilesSpeedStat;
        public IPropertyReadOnly<float> ReloadSpeedStat => _reloadSpeedStat;
        public IPropertyReadOnly<MoneyType> SoftCurrency => _softCurrency;
        public IPropertyReadOnly<MoneyType> HardCurrency => _hardCurrency;
        public IPropertyReadOnly<int> SoftCost => _softCost;
        public IPropertyReadOnly<int> HardCost => _hardCost;
        public IPropertyReadOnly<bool> SoftCurrencyButtonAvailable => _softCurrencyButtonAvailable;
        public IPropertyReadOnly<bool> HardCurrencyButtonAvailable => _hardCurrencyButtonAvailable;
        public IPropertyReadOnly<int> WeaponRank => _weaponRank;
        public IPropertyReadOnly<int> WeaponRequiredLevel => _weaponRequiredLevel;

        public bool IsExtraWeapon { get; set; }
        public SimpleEvent<PlayerWeaponUpgradePanelDescriptor> OnSoftCurrencyButtonEvent => _onSoftCurrencyButtonEvent;
        public SimpleEvent<PlayerWeaponUpgradePanelDescriptor> OnHardCurrencyButtonEvent => _onHardCurrencyButtonEvent;

        public PlayerWeaponUpgradePanelDescriptor(PlayerWeaponType playerWeaponType, PlayerWeaponConfig initWeaponConfig = default)
        {
            IsExtraWeapon = false;
            _weaponType = playerWeaponType;
            SetInitialStats(initWeaponConfig);
        }

        private void SetInitialStats(PlayerWeaponConfig weaponConfig)
        {
            _damageStat.SetValue(weaponConfig.Damage, true);
            _projectilesCountStat.SetValue(weaponConfig.ProjectileCount, true);
            _projectilesSpeedStat.SetValue(weaponConfig.Speed, true);
            _reloadSpeedStat.SetValue(weaponConfig.ReloadSpeed, true);
        }

        public PlayerWeaponUpgradePanelDescriptor(PlayerExtraWeaponType playerExtraWeaponType, PlayerWeaponConfig initWeaponConfig = default)
        {
            IsExtraWeapon = true;
            _extraWeaponType = playerExtraWeaponType;
            SetInitialStats(initWeaponConfig);
        }

        public void SetWeaponRank(int weaponRank) 
        {
            _weaponRank.SetValue(weaponRank, false);
        }

        public void SetWeaponRequiredLevel(int requiredLevel)
        {
            _weaponRequiredLevel.SetValue(requiredLevel, false);
        }

        public void OnSoftCurrencyBuyButtonClick()
        {
            OnSoftCurrencyButtonEvent.Notify(this);
        }

        public void OnHardCurrencyBuyButtonClick()
        {
            OnHardCurrencyButtonEvent.Notify(this);
        }

        public void SetDamage(int damage)
        {
            _damageStat.SetValue(damage, false);
        }

        public void SetProjectilesCount(int projectilesCount) 
        {
            _projectilesCountStat.SetValue(projectilesCount, false);
        }

        public void SetProjectilesSpeed(float speed) 
        {
            _projectilesSpeedStat.SetValue(speed, false);
        }

        public void SetReloadSpeed(float reloadSpeed) 
        {
            _reloadSpeedStat.SetValue(reloadSpeed, false);
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
    }
}