using Core.MVP;
using Core.UI;
using System.Linq;

namespace Core.LobbyBase
{
    public class PlayerBaseSwitcher
    {
        private PlayerBasesManager _playerBasesManager;
        private BaseObjectSelector _baseObjectSelector;
        private IPlayerBaseProgression _playerBasesProgressionSaver;
        private ILobbyScreenPresenter _lobbyScreenPresenter;
        private BoolProperty _canSwitchNext = new BoolProperty();
        private BoolProperty _canSwitchPrevious = new BoolProperty();
        private BoolProperty _baseObserveMode = new BoolProperty();
        private bool _isSwitching;
        private bool _isComingSoonPanelEnabled;
        public IPropertyReadOnly<bool> CanSwitchNext => _canSwitchNext;
        public IPropertyReadOnly<bool> CanSwitchPrevious => _canSwitchPrevious;
        public IPropertyReadOnly<bool> BasesObserveMode => _baseObserveMode;

        public PlayerBaseSwitcher(PlayerBasesManager playerBasesManager, IPlayerBaseProgression playerBasesProgressionSaver, BaseObjectSelector baseObjectSelector, ILobbyScreenPresenter lobbyScreenPresenter)
        {
            _playerBasesManager = playerBasesManager;
            _playerBasesProgressionSaver = playerBasesProgressionSaver;
            _baseObjectSelector = baseObjectSelector;
            _lobbyScreenPresenter = lobbyScreenPresenter;
            SetActiveBase(_playerBasesManager.CurrentBaseIndex);
            CheckCanSwitch();
        }

        public void SwitchBaseObserveMode()
        {
            if (_isSwitching)
            {
                return;
            }
            _isSwitching = true;
            var mode = !_baseObserveMode.Value;
            if (!mode)
            {
                if (_isComingSoonPanelEnabled)
                {
                    DisableComingSoonPanel();
                }
                int currentBaseIndex = 0;
                if (_playerBasesManager.CurrentBase.IsLocked)
                {
                    currentBaseIndex = _playerBasesProgressionSaver.CurrentBaseIndex;
                }
                else
                {
                    currentBaseIndex = _playerBasesManager.CurrentBaseIndex;
                }
                _playerBasesManager.SetCurrentBaseIndex(currentBaseIndex);
                SetActiveBase(currentBaseIndex);
                _playerBasesManager.DisableCameraObserveMode(() => 
                {
                    _baseObserveMode.SetValue(mode);
                    _lobbyScreenPresenter.BasePagePresenter.PlayerBaseControlPanel.SetEnabled(true);
                    _baseObjectSelector.SelectBaseObject(_playerBasesManager.CurrentBase.BaseObjectsDict.First().Value.First());
                    _isSwitching = false;
                });
            }
            else
            {
                _baseObserveMode.SetValue(mode);
                _baseObjectSelector.ClearViewSelection();
                _playerBasesManager.EnableCameraObserveMode();
                _lobbyScreenPresenter.BasePagePresenter.PlayerBaseControlPanel.SetEnabled(false);
                _isSwitching = false;
            }
            CheckCanSwitch();
        }

        public void SetActiveBase(int baseIndex)
        {
            var bases = _playerBasesManager.Bases;
            for (int i = 0; i < bases.Count; i++)
            {
                var @base = bases[i];
                if (i == baseIndex)
                {
                    @base.ShowBase();
                }
                else
                {
                    @base.HideBase();
                }
            }

            if (baseIndex < _playerBasesManager.Bases.Count && !_playerBasesManager.Bases[baseIndex].IsLocked)
            {
                _playerBasesProgressionSaver.SetSelectedBaseIndex(baseIndex);
            }
        }

        public void SwitchPrev()
        {
            if (_isSwitching)
            {
                return;
            }

            _lobbyScreenPresenter.BasePagePresenter.View.SetActiveBaseDetailsPanel(false);

            if (_isComingSoonPanelEnabled)
            {
                DisableComingSoonPanel();
                SetActiveBase(_playerBasesManager.CurrentBaseIndex);
            }
            else
            {
                var prevBase = _playerBasesManager.CurrentBaseIndex - 1;
                _playerBasesManager.SetCurrentBaseIndex(prevBase);
                SetActiveBase(prevBase);
            }

            CheckCanSwitch();
        }

        public void SwitchNext()
        {
            if (_isSwitching)
            {
                return;
            }
            var nextBase = _playerBasesManager.CurrentBaseIndex + 1;
            if (nextBase >= _playerBasesManager.BaseDescriptorsCount)
            {
                EnableComingSoonPanel();
                _lobbyScreenPresenter.BasePagePresenter.View.SetActiveBaseDetailsPanel(true);
                _lobbyScreenPresenter.BasePagePresenter.View.SetBaseDetails("COMING SOON");
                SetActiveBase(nextBase);
            }
            else
            {
                _lobbyScreenPresenter.BasePagePresenter.View.SetActiveBaseDetailsPanel(false);
                _playerBasesManager.SetCurrentBaseIndex(nextBase);
                SetActiveBase(nextBase);
            }

            CheckCanSwitch();
        }

        private void EnableComingSoonPanel()
        {
            _isComingSoonPanelEnabled = true;
            _lobbyScreenPresenter.BasePagePresenter.View.SetActiveComingSoonPanel(true);
        }

        private void DisableComingSoonPanel()
        {
            _isComingSoonPanelEnabled = false;
            _lobbyScreenPresenter.BasePagePresenter.View.SetActiveComingSoonPanel(false);
        }

        private void CheckCanSwitch()
        {
            var canSwitchNext = CheckCanSwitchNext();
            var canSwitchPrev = _playerBasesManager.CurrentBaseIndex > 0;
            _canSwitchNext.SetValue(canSwitchNext);
            _canSwitchPrevious.SetValue(canSwitchPrev);
        }

        private bool CheckCanSwitchNext()
        {
            return _playerBasesManager.CurrentBaseIndex < (_playerBasesManager.Bases.Count - 1)
                        ||
                   (!_playerBasesManager.CurrentBase.IsLocked && (_playerBasesManager.CurrentBaseIndex == _playerBasesManager.BaseDescriptorsCount - 1));
        }
    }
}