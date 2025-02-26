using Core.PlayerModule;
using Core.Resourses;
using Sirenix.Utilities;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GameLogic
{
    public class ExtraWeaponPanelView : MonoBehaviour, IDisposable
    {
        [SerializeField] private RectTransform _statsRoot;
        [SerializeField] private ExtraWeaponStatPanelView _statPanelPrefab;
        [SerializeField] private WeaponStatsSpriteProvider _spriteProvider;
        [SerializeField] private PlayerWeaponSpriteProvider _weaponSpriteProvider;
        [SerializeField] private Image _weaponIcon;
        [SerializeField] private TextMeshProUGUI _weaponName;
        [SerializeField] private TextMeshProUGUI _weaponRank;
        private ExtraWeaponStatPanelView _damagePanel;
        private ExtraWeaponStatPanelView _ammoPanel;
        private ExtraWeaponStatPanelView _reloadSpeedPanel;
        private ExtraWeaponStatPanelView _projectileSpeedPanel;

        public void InitStats(PlayerExtraWeaponUpgradePanelDescriptor currentLevelUpgradePanelDescriptor)
        {
            currentLevelUpgradePanelDescriptor.WeaponRank.RegisterValueChangeListener(OnWeaponRankChanged);
            OnWeaponRankChanged(currentLevelUpgradePanelDescriptor.WeaponRank.Value);

            var damage = currentLevelUpgradePanelDescriptor.DamageStat;
            var ammo = currentLevelUpgradePanelDescriptor.AmmoStat;
            var reloadSpeed = currentLevelUpgradePanelDescriptor.ReloadSpeedStat;
            var projectileSpeed = currentLevelUpgradePanelDescriptor.ProjectileSpeedStat;

            var damageIcon = _spriteProvider.ProvideSprite(PlayerWeaponStats.Damage);
            var ammoIcon = _spriteProvider.ProvideSprite(PlayerWeaponStats.ProjectilesCount);
            var reloadSpeedIcon = _spriteProvider.ProvideSprite(PlayerWeaponStats.ReloadSpeed);
            var projectileSpeedIcon = _spriteProvider.ProvideSprite(PlayerWeaponStats.ProjectileSpeed);

            _weaponName.text = currentLevelUpgradePanelDescriptor.WeaponType.ToString().SplitPascalCase();
            _weaponIcon.sprite = _weaponSpriteProvider.ProvideByType(currentLevelUpgradePanelDescriptor.WeaponType);
            _weaponRank.text = currentLevelUpgradePanelDescriptor.WeaponRank.Value.ToString("RANK 0");

            _damagePanel = Instantiate(_statPanelPrefab, _statsRoot);
            _damagePanel.SetWeaponStatIcon(damageIcon);
            _damagePanel.SetWeaponStatName("Damage:");
            _damagePanel.InitAdvertisementButtonCallback(currentLevelUpgradePanelDescriptor.DamageStat.OnAdvertisementBuyButtonClick);
            _damagePanel.InitSoftCurrencyButtonCallback(currentLevelUpgradePanelDescriptor.DamageStat.OnSoftCurrencyBuyButtonClick);
            _damagePanel.InitHardCurrencyButtonCallback(currentLevelUpgradePanelDescriptor.DamageStat.OnHardCurrencyBuyButtonClick);

            _ammoPanel = Instantiate(_statPanelPrefab, _statsRoot);
            _ammoPanel.SetWeaponStatIcon(ammoIcon);
            _ammoPanel.SetWeaponStatName("Ammo:");
            _ammoPanel.InitAdvertisementButtonCallback(currentLevelUpgradePanelDescriptor.AmmoStat.OnAdvertisementBuyButtonClick);
            _ammoPanel.InitSoftCurrencyButtonCallback(currentLevelUpgradePanelDescriptor.AmmoStat.OnSoftCurrencyBuyButtonClick);
            _ammoPanel.InitHardCurrencyButtonCallback(currentLevelUpgradePanelDescriptor.AmmoStat.OnHardCurrencyBuyButtonClick);

            _reloadSpeedPanel = Instantiate(_statPanelPrefab, _statsRoot);
            _reloadSpeedPanel.SetWeaponStatIcon(reloadSpeedIcon);
            _reloadSpeedPanel.SetWeaponStatName("Reload Speed:");
            _reloadSpeedPanel.InitAdvertisementButtonCallback(currentLevelUpgradePanelDescriptor.ReloadSpeedStat.OnAdvertisementBuyButtonClick);
            _reloadSpeedPanel.InitSoftCurrencyButtonCallback(currentLevelUpgradePanelDescriptor.ReloadSpeedStat.OnSoftCurrencyBuyButtonClick);
            _reloadSpeedPanel.InitHardCurrencyButtonCallback(currentLevelUpgradePanelDescriptor.ReloadSpeedStat.OnHardCurrencyBuyButtonClick);

            _projectileSpeedPanel = Instantiate(_statPanelPrefab, _statsRoot);
            _projectileSpeedPanel.SetWeaponStatIcon(reloadSpeedIcon);
            _projectileSpeedPanel.SetWeaponStatName("Projectile Speed:");
            _projectileSpeedPanel.InitAdvertisementButtonCallback(currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.OnAdvertisementBuyButtonClick);
            _projectileSpeedPanel.InitSoftCurrencyButtonCallback(currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.OnSoftCurrencyBuyButtonClick);
            _projectileSpeedPanel.InitHardCurrencyButtonCallback(currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.OnHardCurrencyBuyButtonClick);

            currentLevelUpgradePanelDescriptor.DamageStat.Value.RegisterValueChangeListener(OnDamageChanged);
            currentLevelUpgradePanelDescriptor.AmmoStat.Value.RegisterValueChangeListener(OnAmmoChanged);
            currentLevelUpgradePanelDescriptor.ReloadSpeedStat.Value.RegisterValueChangeListener(OnReloadSpeedChanged);
            currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.Value.RegisterValueChangeListener(OnProjectileSpeedChanged);

            currentLevelUpgradePanelDescriptor.DamageStat.NextLevelValue.RegisterValueChangeListener(OnNextDamageChanged);
            currentLevelUpgradePanelDescriptor.AmmoStat.NextLevelValue.RegisterValueChangeListener(OnNextAmmoChanged);
            currentLevelUpgradePanelDescriptor.ReloadSpeedStat.NextLevelValue.RegisterValueChangeListener(OnNextReloadSpeedChanged);
            currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.NextLevelValue.RegisterValueChangeListener(OnNextProjectileSpeedChanged);

            OnDamageChanged(currentLevelUpgradePanelDescriptor.DamageStat.Value.Value);
            OnAmmoChanged(currentLevelUpgradePanelDescriptor.AmmoStat.Value.Value);
            OnReloadSpeedChanged(currentLevelUpgradePanelDescriptor.ReloadSpeedStat.Value.Value);
            OnProjectileSpeedChanged(currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.Value.Value);

            OnNextDamageChanged(currentLevelUpgradePanelDescriptor.DamageStat.NextLevelValue.Value);
            OnNextAmmoChanged(currentLevelUpgradePanelDescriptor.AmmoStat.NextLevelValue.Value);
            OnNextProjectileSpeedChanged(currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.NextLevelValue.Value);
            OnNextReloadSpeedChanged(currentLevelUpgradePanelDescriptor.ReloadSpeedStat.NextLevelValue.Value);


            currentLevelUpgradePanelDescriptor.DamageStat.AdvertisementAvailable.RegisterValueChangeListener(OnDamageAdvButtonStatusChanged);
            currentLevelUpgradePanelDescriptor.DamageStat.SoftCurrencyButtonAvailable.RegisterValueChangeListener(OnDamageSoftButtonStatusChanged);
            currentLevelUpgradePanelDescriptor.DamageStat.HardCurrencyButtonAvailable.RegisterValueChangeListener(OnDamageHardButtonStatusChanged);
            currentLevelUpgradePanelDescriptor.DamageStat.SoftCost.RegisterValueChangeListener(OnDamageSoftCostChanged);
            currentLevelUpgradePanelDescriptor.DamageStat.HardCost.RegisterValueChangeListener(OnDamageHardCostChanged);
            currentLevelUpgradePanelDescriptor.DamageStat.SoftCurrency.RegisterValueChangeListener(OnDamageSoftCurrencyChanged);
            currentLevelUpgradePanelDescriptor.DamageStat.HardCurrency.RegisterValueChangeListener(OnDamageHardCurrencyChanged);
            OnDamageAdvButtonStatusChanged(currentLevelUpgradePanelDescriptor.DamageStat.AdvertisementAvailable.Value);
            OnDamageSoftButtonStatusChanged(currentLevelUpgradePanelDescriptor.DamageStat.SoftCurrencyButtonAvailable.Value);
            OnDamageHardButtonStatusChanged(currentLevelUpgradePanelDescriptor.DamageStat.HardCurrencyButtonAvailable.Value);
            OnDamageSoftCurrencyChanged(currentLevelUpgradePanelDescriptor.DamageStat.SoftCurrency.Value);
            OnDamageHardCurrencyChanged(currentLevelUpgradePanelDescriptor.DamageStat.HardCurrency.Value); 
            OnDamageSoftCostChanged(currentLevelUpgradePanelDescriptor.DamageStat.SoftCost.Value);
            OnDamageHardCostChanged(currentLevelUpgradePanelDescriptor.DamageStat.HardCost.Value);

            currentLevelUpgradePanelDescriptor.AmmoStat.AdvertisementAvailable.RegisterValueChangeListener(OnAmmoAdvButtonStatusChanged);
            currentLevelUpgradePanelDescriptor.AmmoStat.SoftCurrencyButtonAvailable.RegisterValueChangeListener(OnAmmoSoftButtonStatusChanged);
            currentLevelUpgradePanelDescriptor.AmmoStat.HardCurrencyButtonAvailable.RegisterValueChangeListener(OnAmmoHardButtonStatusChanged);
            currentLevelUpgradePanelDescriptor.AmmoStat.SoftCost.RegisterValueChangeListener(OnAmmoSoftCostChanged);
            currentLevelUpgradePanelDescriptor.AmmoStat.HardCost.RegisterValueChangeListener(OnAmmoHardCostChanged);
            currentLevelUpgradePanelDescriptor.AmmoStat.SoftCurrency.RegisterValueChangeListener(OnAmmoSoftCurrencyChanged);
            currentLevelUpgradePanelDescriptor.AmmoStat.HardCurrency.RegisterValueChangeListener(OnAmmoHardCurrencyChanged);
            OnAmmoAdvButtonStatusChanged(currentLevelUpgradePanelDescriptor.AmmoStat.AdvertisementAvailable.Value);
            OnAmmoSoftButtonStatusChanged(currentLevelUpgradePanelDescriptor.AmmoStat.SoftCurrencyButtonAvailable.Value);
            OnAmmoHardButtonStatusChanged(currentLevelUpgradePanelDescriptor.AmmoStat.HardCurrencyButtonAvailable.Value);
            OnAmmoSoftCurrencyChanged(currentLevelUpgradePanelDescriptor.AmmoStat.SoftCurrency.Value);
            OnAmmoHardCurrencyChanged(currentLevelUpgradePanelDescriptor.AmmoStat.HardCurrency.Value); 
            OnAmmoSoftCostChanged(currentLevelUpgradePanelDescriptor.AmmoStat.SoftCost.Value);
            OnAmmoHardCostChanged(currentLevelUpgradePanelDescriptor.AmmoStat.HardCost.Value);

            currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.AdvertisementAvailable.RegisterValueChangeListener(OnProjectileSpeedAdvButtonStatusChanged);
            currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.SoftCurrencyButtonAvailable.RegisterValueChangeListener(OnProjectileSpeedSoftButtonStatusChanged);
            currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.HardCurrencyButtonAvailable.RegisterValueChangeListener(OnProjectileSpeedHardButtonStatusChanged);
            currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.SoftCost.RegisterValueChangeListener(OnProjectileSpeedSoftCostChanged);
            currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.HardCost.RegisterValueChangeListener(OnProjectileSpeedHardCostChanged);
            currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.SoftCurrency.RegisterValueChangeListener(OnProjectileSpeedSoftCurrencyChanged);
            currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.HardCurrency.RegisterValueChangeListener(OnProjectileSpeedHardCurrencyChanged);
            OnProjectileSpeedAdvButtonStatusChanged(currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.AdvertisementAvailable.Value);
            OnProjectileSpeedSoftButtonStatusChanged(currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.SoftCurrencyButtonAvailable.Value);
            OnProjectileSpeedHardButtonStatusChanged(currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.HardCurrencyButtonAvailable.Value);
            OnProjectileSpeedSoftCurrencyChanged(currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.SoftCurrency.Value);
            OnProjectileSpeedHardCurrencyChanged(currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.HardCurrency.Value); 
            OnProjectileSpeedSoftCostChanged(currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.SoftCost.Value);
            OnProjectileSpeedHardCostChanged(currentLevelUpgradePanelDescriptor.ProjectileSpeedStat.HardCost.Value);

            currentLevelUpgradePanelDescriptor.ReloadSpeedStat.AdvertisementAvailable.RegisterValueChangeListener(OnReloadSpeedAdvButtonStatusChanged);
            currentLevelUpgradePanelDescriptor.ReloadSpeedStat.SoftCurrencyButtonAvailable.RegisterValueChangeListener(OnReloadSpeedSoftButtonStatusChanged);
            currentLevelUpgradePanelDescriptor.ReloadSpeedStat.HardCurrencyButtonAvailable.RegisterValueChangeListener(OnReloadSpeedHardButtonStatusChanged);
            currentLevelUpgradePanelDescriptor.ReloadSpeedStat.SoftCost.RegisterValueChangeListener(OnReloadSpeedSoftCostChanged);
            currentLevelUpgradePanelDescriptor.ReloadSpeedStat.HardCost.RegisterValueChangeListener(OnReloadSpeedHardCostChanged);
            currentLevelUpgradePanelDescriptor.ReloadSpeedStat.SoftCurrency.RegisterValueChangeListener(OnReloadSpeedSoftCurrencyChanged);
            currentLevelUpgradePanelDescriptor.ReloadSpeedStat.HardCurrency.RegisterValueChangeListener(OnReloadSpeedHardCurrencyChanged);
            OnReloadSpeedAdvButtonStatusChanged(currentLevelUpgradePanelDescriptor.ReloadSpeedStat.AdvertisementAvailable.Value);
            OnReloadSpeedSoftButtonStatusChanged(currentLevelUpgradePanelDescriptor.ReloadSpeedStat.SoftCurrencyButtonAvailable.Value);
            OnReloadSpeedHardButtonStatusChanged(currentLevelUpgradePanelDescriptor.ReloadSpeedStat.HardCurrencyButtonAvailable.Value);
            OnReloadSpeedSoftCurrencyChanged(currentLevelUpgradePanelDescriptor.ReloadSpeedStat.SoftCurrency.Value);
            OnReloadSpeedHardCurrencyChanged(currentLevelUpgradePanelDescriptor.ReloadSpeedStat.HardCurrency.Value); 
            OnReloadSpeedSoftCostChanged(currentLevelUpgradePanelDescriptor.ReloadSpeedStat.SoftCost.Value);
            OnReloadSpeedHardCostChanged(currentLevelUpgradePanelDescriptor.ReloadSpeedStat.HardCost.Value);
        }

        private void OnWeaponRankChanged(int rank)
        {
            _weaponRank.text = rank.ToString($"RANK: {rank}");
        }

        #region DAMAGE CALLBACKS

        private void OnDamageHardCurrencyChanged(MoneyType type)
        {
            _damagePanel.SetHardCostCurrency(type);
        }

        private void OnDamageSoftCurrencyChanged(MoneyType type)
        {
            _damagePanel.SetSoftCostCurrency(type);
        }

        private void OnDamageHardCostChanged(int cost)
        {
            _damagePanel.SetHardCurrencyCost(cost);
        }

        private void OnDamageSoftCostChanged(int cost)
        {
            _damagePanel.SetSoftCurrencyCost(cost);
        }

        private void OnDamageHardButtonStatusChanged(bool status)
        {
            _damagePanel.SetHardCurrencyButtonInteractable(status);
        }

        private void OnDamageSoftButtonStatusChanged(bool status)
        {
            _damagePanel.SetSoftCurrencyButtonInteractable(status);
        }

        private void OnDamageAdvButtonStatusChanged(bool status)
        {
            _damagePanel.SetActiveAdvertisementButton(status);
        }

#endregion
        
#region AMMO CALLBACKS

        private void OnAmmoHardCurrencyChanged(MoneyType type)
        {
            _ammoPanel.SetHardCostCurrency(type);
        }

        private void OnAmmoSoftCurrencyChanged(MoneyType type)
        {
            _ammoPanel.SetSoftCostCurrency(type);
        }

        private void OnAmmoHardCostChanged(int cost)
        {
            _ammoPanel.SetHardCurrencyCost(cost);
        }

        private void OnAmmoSoftCostChanged(int cost)
        {
            _ammoPanel.SetSoftCurrencyCost(cost);
        }

        private void OnAmmoHardButtonStatusChanged(bool status)
        {
            _ammoPanel.SetHardCurrencyButtonInteractable(status);
        }

        private void OnAmmoSoftButtonStatusChanged(bool status)
        {
            _ammoPanel.SetSoftCurrencyButtonInteractable(status);
        }

        private void OnAmmoAdvButtonStatusChanged(bool status)
        {
            _ammoPanel.SetActiveAdvertisementButton(status);
        }

#endregion        

#region ProjectileSpeed CALLBACKS

        private void OnProjectileSpeedHardCurrencyChanged(MoneyType type)
        {
            _projectileSpeedPanel.SetHardCostCurrency(type);
        }

        private void OnProjectileSpeedSoftCurrencyChanged(MoneyType type)
        {
            _projectileSpeedPanel.SetSoftCostCurrency(type);
        }

        private void OnProjectileSpeedHardCostChanged(int cost)
        {
            _projectileSpeedPanel.SetHardCurrencyCost(cost);
        }

        private void OnProjectileSpeedSoftCostChanged(int cost)
        {
            _projectileSpeedPanel.SetSoftCurrencyCost(cost);
        }

        private void OnProjectileSpeedHardButtonStatusChanged(bool status)
        {
            _projectileSpeedPanel.SetHardCurrencyButtonInteractable(status);
        }

        private void OnProjectileSpeedSoftButtonStatusChanged(bool status)
        {
            _projectileSpeedPanel.SetSoftCurrencyButtonInteractable(status);
        }

        private void OnProjectileSpeedAdvButtonStatusChanged(bool status)
        {
            _projectileSpeedPanel.SetActiveAdvertisementButton(status);
        }

#endregion
        
#region ReloadSpeed CALLBACKS

        private void OnReloadSpeedHardCurrencyChanged(MoneyType type)
        {
            _reloadSpeedPanel.SetHardCostCurrency(type);
        }

        private void OnReloadSpeedSoftCurrencyChanged(MoneyType type)
        {
            _reloadSpeedPanel.SetSoftCostCurrency(type);
        }

        private void OnReloadSpeedHardCostChanged(int cost)
        {
            _reloadSpeedPanel.SetHardCurrencyCost(cost);
        }

        private void OnReloadSpeedSoftCostChanged(int cost)
        {
            _reloadSpeedPanel.SetSoftCurrencyCost(cost);
        }

        private void OnReloadSpeedHardButtonStatusChanged(bool status)
        {
            _reloadSpeedPanel.SetHardCurrencyButtonInteractable(status);
        }

        private void OnReloadSpeedSoftButtonStatusChanged(bool status)
        {
            _reloadSpeedPanel.SetSoftCurrencyButtonInteractable(status);
        }

        private void OnReloadSpeedAdvButtonStatusChanged(bool status)
        {
            _reloadSpeedPanel.SetActiveAdvertisementButton(status);
        }

#endregion

        private void OnProjectileSpeedChanged(object speed)
        {
            _projectileSpeedPanel.SetCurrentLevelValue((float)speed);
        }

        private void OnReloadSpeedChanged(object reloadSpeed)
        {
            _reloadSpeedPanel.SetCurrentLevelValue((float)reloadSpeed);
        }

        private void OnAmmoChanged(object ammo)
        {
            _ammoPanel.SetCurrentLevelValue((int)ammo);
        }

        private void OnDamageChanged(object damage)
        {
            _damagePanel.SetCurrentLevelValue((int)damage);
        }

        private void OnNextProjectileSpeedChanged(object speed)
        {
            _projectileSpeedPanel.SetNextLevelValue((float)speed);
        }

        private void OnNextReloadSpeedChanged(object reloadSpeed)
        {
            _reloadSpeedPanel.SetNextLevelValue((float)reloadSpeed);
        }

        private void OnNextAmmoChanged(object ammo)
        {
            _ammoPanel.SetNextLevelValue((float)ammo);
        }

        private void OnNextDamageChanged(object damage)
        {
            _damagePanel.SetNextLevelValue((float)damage);
        }

        public void Dispose()
        {
            _damagePanel?.Dispose();
            _ammoPanel?.Dispose();
            _reloadSpeedPanel?.Dispose();
            _projectileSpeedPanel?.Dispose();
        }
    }
}