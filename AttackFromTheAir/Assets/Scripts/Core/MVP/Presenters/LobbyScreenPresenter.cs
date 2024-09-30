using Core.DISimple;
using Core.GameLogic;
using Core.Level;
using Core.Utilities;
using Zenject;

namespace Core.MVP
{
    public interface ILobbyScreenPresenter : IPresenter<LobbyScreenModel, LobbyScreenProxyView, LobbyScreenView>
    {
        SimpleEvent PlayButtonTapEvent { get; }
        SimpleEvent LeftLevelSelectorArrowEvent{ get; }
        SimpleEvent RightLevelSelectorArrowEvent{ get; }
    }

    public class LobbyScreenPresenter : ILobbyScreenPresenter
    {
        private PlayerWeaponUpgradeSystem _playerWeaponUpgradeSystem;
        private SimpleEvent _playButtonTapEvent = new SimpleEvent();
        private SimpleEvent _leftLevelSelectorArrowEvent = new SimpleEvent();
        private SimpleEvent _rightLevelSelectorArrowEvent = new SimpleEvent();
        private LevelSelector _levelSelector;
        public LobbyScreenModel Model { get; }
        public LobbyScreenProxyView ProxyView { get; }
        public LobbyScreenUseCases UseCases { get; }
        public SimpleEvent PlayButtonTapEvent => _playButtonTapEvent;
        public SimpleEvent LeftLevelSelectorArrowEvent => _leftLevelSelectorArrowEvent;
        public SimpleEvent RightLevelSelectorArrowEvent => _rightLevelSelectorArrowEvent;

        public LobbyScreenPresenter(LobbyScreenModel model, LobbyScreenUseCases useCases, LobbyScreenProxyView proxyView, 
        PlayerWeaponUpgradeSystem playerWeaponUpgradeSystem)
        {
            Model = model;
            UseCases = useCases;
            ProxyView = proxyView;
            _playerWeaponUpgradeSystem = playerWeaponUpgradeSystem;
        }

        public void OnStartButtonClickHandler()
        {
            PlayButtonTapEvent.Notify();
        }

        public void Destroy()
        {
            if (!ProxyView.IsPrepared)
            {
                ProxyView.View.Destroy();
            }
        }

        public void Hide()
        {
            if (!ProxyView.IsPrepared)
            {
                return;
            }
            ProxyView.View.Hide();
        }

        public void Init()
        {
            if (!ProxyView.IsPrepared)
            {
                ProxyView.Prepare();
                ProxyView.View.InitPresenter(this);
                var diContainer = ServiceLocator.Resolve<DiContainer>();
                _levelSelector = diContainer.Resolve<LevelSelector>();
                Model.InitLevelSelector(_levelSelector);
                SubscribeDataChange();
                _playerWeaponUpgradeSystem.InitWeaponUpgradeSystemView(ProxyView.View.WeaponUpgradeSystemView);
            }
        }

        public void Show()
        {
            if (!ProxyView.IsPrepared)
            {
                return;
            }
            ProxyView.View.Show();
            RefreshData();
        }

        private void RefreshData()
        {
            OnCurrentLevelChanged(_levelSelector.CurrentLevel.Value);
            ProxyView.View.SetPlayerLevel(Model.PlayerLevel.Value);
            ProxyView.View.SetBaseLevel(Model.BaseLevel.Value);
            ProxyView.View.SetPlayerExpPercent(Model.PlayerExpirience.Value);
            ProxyView.View.SetBaseExpPercent(Model.BaseExpirience.Value);
            ProxyView.View.SetDollarsAmount(Model.DollarsAmount.Value);
            ProxyView.View.SetHammersAmount(Model.HammersAmount.Value);
            ProxyView.View.SetGemsAmount(Model.GemsAmount.Value);

            ProxyView.View.GamemodePanelSelector.SelectGamemode(Model.CurrentMissionGamemode.Value);
            ProxyView.View.SetMissionPreview(Model.CurrentMissionPreview.Value);
            //ProxyView.View.SetMissionDescription(Model.CurrentMissionDescription.Value);
        }

        private void SubscribeDataChange()
        {

            _levelSelector.CurrentLevel.RegisterValueChangeListener(OnCurrentLevelChanged);

            Model.PlayerLevel.RegisterValueChangeListener(ProxyView.View.SetPlayerLevel);
            Model.PlayerExpirience.RegisterValueChangeListener(ProxyView.View.SetPlayerExpPercent);
            Model.BaseLevel.RegisterValueChangeListener(ProxyView.View.SetBaseLevel);
            Model.BaseExpirience.RegisterValueChangeListener(ProxyView.View.SetBaseExpPercent);
            Model.DollarsAmount.RegisterValueChangeListener(ProxyView.View.SetDollarsAmount);
            Model.HammersAmount.RegisterValueChangeListener(ProxyView.View.SetHammersAmount);
            Model.GemsAmount.RegisterValueChangeListener(ProxyView.View.SetGemsAmount);

            Model.CurrentMissionGamemode.RegisterValueChangeListener(ProxyView.View.GamemodePanelSelector.SelectGamemode);
            //Model.CurrentMissionDescription.RegisterValueChangeListener(ProxyView.View.SetMissionDescription);
            Model.CurrentMissionPreview.RegisterValueChangeListener(ProxyView.View.SetMissionPreview);
        }

        private void OnCurrentLevelChanged(int level)
        {
            ProxyView.View.SetCurrentLevel(level);
        }

        public void OnLevelSelectorLeftArrowClick()
        {
            _leftLevelSelectorArrowEvent.Notify();
        }

        public void OnLevelSelectorRightArrowClick()
        {
            _rightLevelSelectorArrowEvent.Notify();
        }
    }
}
