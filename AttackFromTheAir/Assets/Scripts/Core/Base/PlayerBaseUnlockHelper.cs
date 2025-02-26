using Core.MVP;
using Core.UI;
using Core.Utilities;
using Sirenix.Utilities;
using System;
using System.Linq;
using UnityEngine;

namespace Core.LobbyBase
{
    public class PlayerBaseUnlockHelper
    {
        private const int MULTIPLIER_PER_BASE_NUMBER = 1;
        private FloatProperty _unlockProgress = new FloatProperty();
        private CustomProperty<string> _baseName = new CustomProperty<string>(string.Empty);
        private PlayerBaseSwitcher _playerBaseSwitcher;
        private PlayerBasesManager _playerBasesManager;
        private IInfoPopupPresenter _infoPopupPresenter;
        private IPlayerBaseProgression _playerBaseProgression;
        private ILobbyScreenPresenter _lobbyScreenPresenter;
        private BaseObjectSelector _baseObjectSelector;
        private Base _currentBase;
        private int _currentObjectsLevelTarget;
        public IPropertyReadOnly<float> UnlockProgress => _unlockProgress;
        public IPropertyReadOnly<string> BaseName => _baseName;

        public PlayerBaseUnlockHelper(PlayerBaseSwitcher playerBaseSwitcher, PlayerBasesManager playerBasesManager, IInfoPopupPresenter infoPopupPresenter, IPlayerBaseProgression playerBaseProgression, ILobbyScreenPresenter lobbyScreenPresenter, BaseObjectSelector baseObjectSelector)
        {
            _playerBaseSwitcher = playerBaseSwitcher;
            _playerBasesManager = playerBasesManager;
            _infoPopupPresenter = infoPopupPresenter;
            _lobbyScreenPresenter = lobbyScreenPresenter;
            _playerBaseProgression = playerBaseProgression;
            _playerBasesManager.CurrentBaseIndexChanged.AddListener(OnBaseIndexChanged);
            _baseObjectSelector = baseObjectSelector;
            OnBaseIndexChanged(_playerBasesManager.CurrentBaseIndex);
        }

        private void OnBaseIndexChanged(int index)
        {
            if (_currentBase != null)
            {
                UnsubscribeUnlockProgress();
            }

            var currentBase = _playerBasesManager.CurrentBase;
            _baseName.SetValue(currentBase.BaseName, false);
            if (!currentBase.IsLocked && _playerBasesManager.HasNextBase)
            {
                var nextBase = _playerBasesManager.NextBase;
                if (nextBase.IsLocked)
                {
                    SetActiveBaseDetailsPanel(false);
                    SubscribeUnlockProgress();
                    _currentObjectsLevelTarget = (index + 1) * MULTIPLIER_PER_BASE_NUMBER;
                    MakeUnlockProgress();
                }
                else
                {
                    SetActiveBaseDetailsPanel(true);
                    SetBaseDetails("UNLOCK COMPLETE");
                }
            }
            else if (!currentBase.IsLocked && !_playerBasesManager.HasNextBase)
            {
                SetActiveBaseDetailsPanel(false);
                SubscribeUnlockProgress();
                _currentObjectsLevelTarget = (index + 1) * MULTIPLIER_PER_BASE_NUMBER;
                MakeUnlockProgress();
            }
            else if (currentBase.IsLocked)
            {
                SetActiveBaseDetailsPanel(true);
                SetBaseDetails("LOCKED");
            }
        }

        private void SetActiveBaseDetailsPanel(bool isActive)
        {
            var basePageView = _lobbyScreenPresenter.BasePagePresenter.View;
            basePageView.SetActiveBaseDetailsPanel(isActive);
        }

        private void SetBaseDetails(string details)
        {
            var basePageView = _lobbyScreenPresenter.BasePagePresenter.View;
            basePageView.SetBaseDetails(details);
        }

        private void SubscribeUnlockProgress()
        {
            _currentBase = _playerBasesManager.CurrentBase;
            var objects = _currentBase.BaseObjectsDict;

            foreach (var item in objects)
            {
                foreach (var baseObject in item.Value)
                {
                    baseObject.UpgradableBuilding.Level.RegisterValueChangeListener(OnBuildingLevelChanged);
                }
            }
        }

        private void OnBuildingLevelChanged(int level)
        {
            MakeUnlockProgress();
        }

        private void UnsubscribeUnlockProgress()
        {
            var objects = _currentBase.BaseObjectsDict;

            foreach (var item in objects)
            {
                foreach (var baseObject in item.Value)
                {
                    baseObject.UpgradableBuilding.Level.UnregisterValueChangeListener(OnBuildingLevelChanged);
                }
            }
        }

        private void MakeUnlockProgress()
        {
            var objects = _playerBasesManager.CurrentBase.BaseObjectsDict;
            int count = 0;
            int summaryLevel = 0;
            foreach (var item in objects) 
            {
                foreach (var baseObject in item.Value)
                {
                    summaryLevel += baseObject.UpgradableBuilding.Level.Value;
                    count++;
                }    
            }

            float averageLevel = (float)summaryLevel / count;
            float progress = averageLevel / (float)_currentObjectsLevelTarget;

            if (progress >= 1f)
            {
                _unlockProgress.SetValue(1f, false);
                BaseUnlockedHandler();
            }
            else
            {
                _unlockProgress.SetValue(progress, false);
            }
        }

        private void BaseUnlockedHandler()
        {
            if (_playerBasesManager.HasNextBase)
            {
                var baseIndex = _playerBasesManager.CurrentBaseIndex + 1;
                _playerBaseProgression.SetCurrentBaseIndex(baseIndex);
                _playerBasesManager.NextBase.IsLocked = false;
                _playerBasesManager.NextBase.SetVisibilityMode(true);

                _infoPopupPresenter.Init();
                _infoPopupPresenter.Show();
                _baseObjectSelector.ClearViewSelection();
                _infoPopupPresenter.SetInfo("You unlocked a\nNew Base!");
                _infoPopupPresenter.CloseEvent.AddListener(OnInfoPopupClosed);
            }
        }

        private void OnInfoPopupClosed()
        {
            _infoPopupPresenter.CloseEvent.RemoveListener(OnInfoPopupClosed);
            _playerBaseSwitcher.SwitchNext();
            var firstBaseObject = _playerBasesManager.CurrentBase.BaseObjectsDict.First().Value.First();
            _baseObjectSelector.SelectBaseObject(firstBaseObject);
        }
    }
}