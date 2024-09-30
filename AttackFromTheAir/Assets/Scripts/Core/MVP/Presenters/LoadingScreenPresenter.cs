using Core.GameLogic;
using Core.UI;
using Core.Utilities;
using System;

namespace Core.MVP
{
    public interface ILoadingScreenPresenter : IPresenter<LoadingScreenModel, LoadingScreenProxyView, LoadingScreenView>
    {
        SimpleEvent StartLevelEvent { get; }
    }

    public class LoadingScreenPresenter : BasePresenter, ILoadingScreenPresenter
    {
        private DroneContainerView _droneContainerView;
        private SimpleEvent _startLevelEvent = new SimpleEvent();
        private IDroneSelectService _droneSelectService;
        public LoadingScreenProxyView ProxyView { get; }
        public LoadingScreenModel Model { get; }
        public override bool IsOpen { get; }
        public SimpleEvent StartLevelEvent => _startLevelEvent;

        public LoadingScreenPresenter(LoadingScreenProxyView proxyView, LoadingScreenModel model, DroneContainerView droneContainerView, IDroneSelectService droneSelectService)
        {
            ProxyView = proxyView;
            Model = model;
            _droneContainerView = droneContainerView;
            _droneSelectService = droneSelectService;
        }

        public void Destroy()
        {
            if (ProxyView.IsPrepared)
            {
                ProxyView.View.Destroy();
            }
        }

        public override void Hide()
        {
            if (ProxyView.IsPrepared)
            {
                ProxyView.View.Hide();
                _droneContainerView.Disable();
            }
        }

        public override void Init()
        {
            if (!ProxyView.IsPrepared)
            {
                ProxyView.Prepare();
                ProxyView.View.InitPresenter(this);
                SubscribeDataChange();
            }
        }

        private void SubscribeDataChange()
        {
            Model.CurrentLevel.RegisterValueChangeListener(ProxyView.View.SetCurrentLevel);
        }

        private void RefreshData()
        {
            ProxyView.View.SetCurrentLevel(Model.CurrentLevel.Value);
        }

        public override void Show()
        {
            if (ProxyView.IsPrepared)
            {
                ProxyView.View.Show();
                var selectedDrone = _droneSelectService.SelectedDrone.Value;
                _droneContainerView.InitDrone(selectedDrone);
                _droneContainerView.Enable();
                RefreshData();
            }
        }

        public void OnStartLevelButtonClick()
        {
            _startLevelEvent.Notify();
        }
    }
}