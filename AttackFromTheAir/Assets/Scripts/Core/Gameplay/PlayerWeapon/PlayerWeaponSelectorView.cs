using Core.Resourses;
using Core.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Core.GameLogic
{
    public class PlayerWeaponSelectorView : MonoBehaviour
    {
        private PlayerWeaponSelector _model;
        [SerializeField] private PlayerWeaponItemView _weaponItemPrefab;
        [SerializeField] private RectTransform _mainWeaponItemRoot;
        [SerializeField] private RectTransform _secondWeaponItemRoot;
        [SerializeField] private RectTransform _extraWeaponItemRoot;
        [SerializeField] private GameObject _secondWeaponLockPanel;
        [SerializeField] private GameObject _extraWeaponLockPanel;
        [SerializeField] private TextMeshProUGUI _secondWeaponUnlockLevel;
        [SerializeField] private TextMeshProUGUI _extraWeaponUnlockLevel;
        [SerializeField] private PlayerWeaponSpriteProvider _weaponSpriteProvider;
        private Dictionary<PlayerWeaponType, PlayerWeaponItemView> _mainWeaponItems = new Dictionary<PlayerWeaponType, PlayerWeaponItemView>();
        private Dictionary<PlayerWeaponType, PlayerWeaponItemView> _secondWeaponItems = new Dictionary<PlayerWeaponType, PlayerWeaponItemView>();
        private Dictionary<PlayerExtraWeaponType, PlayerWeaponItemView> _extraWeaponItems = new Dictionary<PlayerExtraWeaponType, PlayerWeaponItemView>();
        private PlayerWeaponItemView _selectedMainWeapon;
        private PlayerWeaponItemView _selectedSecondWeapon;
        private PlayerWeaponItemView _selectedExtraWeapon;

        public void LinkModel(PlayerWeaponSelector playerWeaponSelector)
        {
            _model = playerWeaponSelector;
            playerWeaponSelector.SelectedMainWeapon.RegisterValueChangeListener(OnMainWeaponChanged);
            playerWeaponSelector.SelectedSecondWeapon.RegisterValueChangeListener(OnSecondWeaponChanged);
            playerWeaponSelector.SelectedExtraWeapon.RegisterValueChangeListener(OnExtraWeaponChanged);
        }

        public void SetLockedSecondWeaponPanel(bool locked, int unlockLevel = 0)
        {
            _secondWeaponLockPanel.SetActive(locked);
            _secondWeaponUnlockLevel.text = unlockLevel.ToString("Unlocks at level 0");
            _secondWeaponItemRoot.gameObject.SetActive(!locked);
        }

        public void SetLockedExtraWeaponPanel(bool locked, int unlockLevel = 0)
        {
            _extraWeaponLockPanel.SetActive(locked);
            _extraWeaponUnlockLevel.text = unlockLevel.ToString("Unlocks at level 0");
            _extraWeaponItemRoot.gameObject.SetActive(!locked);
        }

        private void OnExtraWeaponChanged(PlayerExtraWeaponType type)
        {
            _selectedExtraWeapon?.Unselect();
            if (type == PlayerExtraWeaponType.None)
            {
                return;
            }
            if (_extraWeaponItems.TryGetValue(type, out var weaponItemView))
            {
                _selectedExtraWeapon = weaponItemView;
                weaponItemView.Select();
            }
            else
            {
                _selectedExtraWeapon = null;
            }
        }

        private void OnSecondWeaponChanged(PlayerWeaponType type)
        {
            _selectedSecondWeapon?.Unselect();
            if (type == PlayerWeaponType.None)
            {
                return;
            }
            if (_secondWeaponItems.TryGetValue(type, out var weaponItemView))
            {
                _selectedSecondWeapon = weaponItemView;
                weaponItemView.Select();
            }
            else
            {
                _selectedSecondWeapon = null;
            }
        }

        public void AnimateMainWeaponWarning(PlayerWeaponType playerWeaponType)
        {
            if (_mainWeaponItems.TryGetValue(playerWeaponType, out var itemView))
            {
                itemView.AnimateWarning();
            }
        }

        public void AnimateSecondWeaponWarning(PlayerWeaponType playerWeaponType)
        {
            if (_secondWeaponItems.TryGetValue(playerWeaponType, out var itemView))
            {
                itemView.AnimateWarning();
            }
        }

        private void OnMainWeaponChanged(PlayerWeaponType type)
        {
            _selectedMainWeapon?.Unselect();
            if (type == PlayerWeaponType.None)
            {
                return;
            }
            if (_mainWeaponItems.TryGetValue(type, out var weaponItemView))
            {
                _selectedMainWeapon = weaponItemView;
                weaponItemView.Select();
            }
            else
            {
                _selectedMainWeapon = null;
            }
        }

        public void InitMainWeapons(List<PlayerWeaponStatusDescriptor> playerWeapons)
        {
            ClearMainWeaponRoot();
            foreach (var weapon in playerWeapons)
            {
                var itemView = Instantiate<PlayerWeaponItemView>(_weaponItemPrefab, _mainWeaponItemRoot, false);
                _mainWeaponItems.Add(weapon.WeaponType, itemView);
                var isLocked = weapon.IsLocked;
                weapon.LockStatusChangedEvent.AddListener(OnMainWeaponLockStatusChanged);
                itemView.PlayerWeaponType = weapon.WeaponType;
                itemView.ItemClicked.AddListener(OnMainWeaponItemClick);
                itemView.SetLockedStatus(isLocked);
                if (!isLocked)
                {
                    var icon = _weaponSpriteProvider.ProvideByType(weapon.WeaponType);
                    itemView.SetWeaponIcon(icon);
                }
                else
                {
                    itemView.SetRequiredLevel(weapon.UnlockLevel);
                }
            }
        }

        private void OnMainWeaponItemClick(PlayerWeaponItemView playerWeaponItemView)
        {
            _model.SelectMainWeapon(playerWeaponItemView.PlayerWeaponType);
        }

        public void InitSecondWeapons(List<PlayerWeaponStatusDescriptor> playerWeapons)
        {
            ClearSecondWeaponRoot();
            foreach (var weapon in playerWeapons)
            {
                var itemView = Instantiate<PlayerWeaponItemView>(_weaponItemPrefab, _secondWeaponItemRoot, false);
                _secondWeaponItems.Add(weapon.WeaponType, itemView);
                weapon.LockStatusChangedEvent.AddListener(OnSecondWeaponLockStatusChanged);
                var isLocked = weapon.IsLocked;
                itemView.PlayerWeaponType = weapon.WeaponType;
                itemView.ItemClicked.AddListener(OnSecondWeaponItemClick);
                itemView.SetLockedStatus(weapon.IsLocked);
                if (!isLocked)
                {
                    var icon = _weaponSpriteProvider.ProvideByType(weapon.WeaponType);
                    itemView.SetWeaponIcon(icon);
                }
                else
                {
                    itemView.SetRequiredLevel(weapon.UnlockLevel);
                }
            }
        }

        private void OnExtraWeaponLockStatusChanged(PlayerExtraWeaponStatusDescriptor descriptor, bool isLocked)
        {
            var itemView = _extraWeaponItems[descriptor.ExtraWeaponType];
            UpdateExtraWeaponLockStatus(itemView, descriptor, isLocked);
        }

        private void OnSecondWeaponLockStatusChanged(PlayerWeaponStatusDescriptor descriptor, bool isLocked)
        {
            var itemView = _secondWeaponItems[descriptor.WeaponType];
            UpdateWeaponLockStatus(itemView, descriptor, isLocked);
        }
        
        private void OnMainWeaponLockStatusChanged(PlayerWeaponStatusDescriptor descriptor, bool isLocked)
        {
            var itemView = _mainWeaponItems[descriptor.WeaponType];
            UpdateWeaponLockStatus(itemView, descriptor, isLocked);
        }

        private void UpdateWeaponLockStatus(PlayerWeaponItemView itemView, PlayerWeaponStatusDescriptor descriptor, bool isLocked)
        {
            itemView.SetLockedStatus(isLocked);

            if (!isLocked)
            {
                var icon = _weaponSpriteProvider.ProvideByType(descriptor.WeaponType);
                itemView.SetWeaponIcon(icon);
            }
            else
            {
                itemView.SetRequiredLevel(descriptor.UnlockLevel);
            }
        }
        
        private void UpdateExtraWeaponLockStatus(PlayerWeaponItemView itemView, PlayerExtraWeaponStatusDescriptor descriptor, bool isLocked)
        {
            itemView.SetLockedStatus(isLocked);

            if (!isLocked)
            {
                var icon = _weaponSpriteProvider.ProvideByType(descriptor.ExtraWeaponType);
                itemView.SetWeaponIcon(icon);
            }
            else
            {
                itemView.SetRequiredLevel(descriptor.UnlockLevel);
            }
        }

        private void OnSecondWeaponItemClick(PlayerWeaponItemView playerWeaponItemView)
        {
            _model.SelectSecondWeapon(playerWeaponItemView.PlayerWeaponType);
        }

        public void InitExtraWeapons(List<PlayerExtraWeaponStatusDescriptor> playerWeapons)
        {
            ClearExtraWeaponRoot();
            foreach (var weapon in playerWeapons)
            {
                var itemView = Instantiate<PlayerWeaponItemView>(_weaponItemPrefab, _extraWeaponItemRoot, false);
                _extraWeaponItems.Add(weapon.ExtraWeaponType, itemView);
                weapon.LockStatusChangedEvent.AddListener(OnExtraWeaponLockStatusChanged);
                var isLocked = weapon.IsLocked;
                itemView.PlayerExtraWeaponType = weapon.ExtraWeaponType;
                itemView.ItemClicked.AddListener(OnExtraWeaponItemClick);
                itemView.SetLockedStatus(weapon.IsLocked);
                if (!isLocked)
                {
                    var icon = _weaponSpriteProvider.ProvideByType(weapon.ExtraWeaponType);
                    itemView.SetWeaponIcon(icon);
                }
                else
                {
                    itemView.SetRequiredLevel(weapon.UnlockLevel);
                }
            }
        }

        private void OnExtraWeaponItemClick(PlayerWeaponItemView playerExtraWeaponItemView)
        {
            _model.SelectExtraWeapon(playerExtraWeaponItemView.PlayerExtraWeaponType);
        }

        private void ClearMainWeaponRoot()
        {
            foreach (var item in _mainWeaponItems.Values)
            {
                item.ItemClicked.RemoveAllListeners();
            }
            _selectedMainWeapon?.Unselect();
            _selectedMainWeapon = null;
            _mainWeaponItems.Clear();
            _mainWeaponItemRoot.ClearAllChild();
        }

        private void ClearSecondWeaponRoot()
        {
            foreach (var item in _secondWeaponItems.Values)
            {
                item.ItemClicked.RemoveAllListeners();
            }
            _selectedSecondWeapon?.Unselect();
            _selectedSecondWeapon = null;
            _secondWeaponItems.Clear();
            _secondWeaponItemRoot.ClearAllChild();
        }

        private void ClearExtraWeaponRoot()
        {
            foreach (var item in _extraWeaponItems.Values)
            {
                item.ItemClicked.RemoveAllListeners();
            }
            _selectedExtraWeapon?.Unselect();
            _selectedExtraWeapon = null;
            _extraWeaponItems.Clear();
            _extraWeaponItemRoot.ClearAllChild();
        }
    }
}