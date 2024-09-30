using Core.Events;
using Core.Level;
using Core.UI;
using Core.Utilities;
using System;
using UnityEngine;
using Zenject;

namespace Core.MVP
{
    public interface IGameScreenPresenter : IPresenter<GameScreenModel, GameScreenProxyView, GameScreenView>
    {
        SimpleEvent<float> ZoomNormalizedValueChangedEvent { get; }
        SimpleEvent ZoomStartChangingEvent { get; }
        void OnWeaponFireButtonClick();
        void OnExtraWeaponFireButtonClick();
        void OnWeaponSwitchButtonClick();
        void OnSwitchNightVisionButtonClick();
        void SetFireButtonState(FireButtonAnimationHelper.FireButtonState fireButtonState);
    }

    public class GameScreenPresenter : IGameScreenPresenter
    {
        private SimpleEvent<float> _zoomNormalizedValueChangedEvent = new SimpleEvent<float>();
        private SimpleEvent _zoomStartChangingEvent = new SimpleEvent();
        private ILevelController _levelController;
        private DiContainer _diContainer;
        public GameScreenModel Model { get; }
        public GameScreenProxyView ProxyView { get; }
        public GameScreenUseCases UseCases { get; }
        public SimpleEvent<float> ZoomNormalizedValueChangedEvent => _zoomNormalizedValueChangedEvent;
        public SimpleEvent ZoomStartChangingEvent => _zoomStartChangingEvent;

        public GameScreenPresenter(GameScreenModel model, GameScreenProxyView proxyView, GameScreenUseCases useCases, DiContainer diContainer)
        {
            Model = model;
            ProxyView = proxyView;
            UseCases = useCases;
            _diContainer = diContainer;
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
                _levelController = _diContainer.Resolve<ILevelController>();
                ProxyView.Prepare();
                ProxyView.View.InitPresenter(this);
                SubscribeDataChange();
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
            var defaultValue = Model.SetDefaultZoom();
            _zoomNormalizedValueChangedEvent.Notify(defaultValue);
            ProxyView.View.ZoomScalePanelView.SetHandlePosNormalized(defaultValue);
            ProxyView.View.FireButtonAnimationHelper.ReloadBar.SetValue(Model.WeaponReloadValue.Value);
            ProxyView.View.FireButtonAnimationHelper.ContinuousFireBar.SetValue(Model.ContinuousFireProgress.Value);
            OnFireButtonReloadStatusChange(Model.IsWeaponReloading.Value);
            ProxyView.View.SetCurrentWeaponIcon(Model.CurrentWeaponIcon.Value);
            ProxyView.View.SetSecondWeaponIcon(Model.SecondWeaponIcon.Value);
            ProxyView.View.SetExtraWeaponIcon(Model.ExtraWeaponIcon.Value);
            OnContinuousFireProgressVisibilityChanged(Model.ShowContinuousFireProgress.Value);
        }

        private void SubscribeDataChange()
        {
            _levelController.PlayerData.WeaponContainer.IsContinuousFire.RegisterValueChangeListener(OnContinuousFireMode);
            ProxyView.View.ZoomScalePanelView.ValueChangedEvent.AddListener(OnNormalizedZoomChanged);
            ProxyView.View.ZoomScalePanelView.PointerDownEvent.AddListener(OnZoomPointerDown);
            Model.ZoomScaleValue.RegisterValueChangeListener(ProxyView.View.ZoomScalePanelView.SetValue);
            Model.WeaponReloadValue.RegisterValueChangeListener(ProxyView.View.FireButtonAnimationHelper.ReloadBar.SetValue);
            Model.CurrentWeaponIcon.RegisterValueChangeListener(ProxyView.View.SetCurrentWeaponIcon);
            Model.SecondWeaponIcon.RegisterValueChangeListener(ProxyView.View.SetSecondWeaponIcon);
            Model.ExtraWeaponIcon.RegisterValueChangeListener(ProxyView.View.SetExtraWeaponIcon);
            Model.IsWeaponReloading.RegisterValueChangeListener(OnFireButtonReloadStatusChange);
            Model.ContinuousFireProgress.RegisterValueChangeListener(ProxyView.View.FireButtonAnimationHelper.ContinuousFireBar.SetValue);
            Model.ShowContinuousFireProgress.RegisterValueChangeListener(OnContinuousFireProgressVisibilityChanged);
        }

        private void OnContinuousFireProgressVisibilityChanged(bool isVisible)
        {
            ProxyView.View.FireButtonAnimationHelper.ContinuousFireBar.gameObject.SetActive(isVisible);
        }

        private void OnContinuousFireMode(bool isContinuousFire)
        {
            ProxyView.View.FireButton.IsCalculateVectorMode = isContinuousFire;
            
            if (isContinuousFire)
            {
                ProxyView.View.FireButton.JoystickMoveEvent.AddListener(_levelController.PlayerData.MovementSystem.SetMoveVector);
                _levelController.PlayerData.MovementSystem.PointerDownHandler();
            }
            else
            {
                ProxyView.View.FireButton.JoystickMoveEvent.RemoveListener(_levelController.PlayerData.MovementSystem.SetMoveVector);
                _levelController.PlayerData.MovementSystem.PointerUpHandler();
            }
        }

        private void OnZoomPointerDown()
        {
            _zoomStartChangingEvent.Notify();
        }

        private void OnFireButtonReloadStatusChange(bool isReloading)
        {
            if (isReloading) 
            {
                ProxyView.View.FireButtonAnimationHelper.SetState(FireButtonAnimationHelper.FireButtonState.Reloading);
            }
            else
            {
                ProxyView.View.FireButtonAnimationHelper.SetState(FireButtonAnimationHelper.FireButtonState.Active);
            }
        }

        private void OnNormalizedZoomChanged(float zoomNormalized)
        {
            Model.SetNormalizedZoom(zoomNormalized);
            _zoomNormalizedValueChangedEvent.Notify(zoomNormalized);
        }

        public void SetFireButtonState(FireButtonAnimationHelper.FireButtonState fireButtonState)
        {
            if (ProxyView.IsPrepared)
            {
                ProxyView.View.FireButtonAnimationHelper.SetState(fireButtonState);
            }
        }

        public void OnWeaponFireButtonClick()
        {
            UseCases.MainWeaponFireButtonHandler();
        }

        public void OnExtraWeaponFireButtonClick()
        {
            UseCases.ExtraWeaponFireButtonHandler();
        }

        public void OnWeaponSwitchButtonClick()
        {
            UseCases.SwitchWeaponButtonHandler();
        }

        public void OnSwitchNightVisionButtonClick()
        {
            UseCases.SwitchNightVision();
        }

        public void OnSettingsButtonClick()
        {
            UseCases.OnSettingsButtonClick();
        }
    }
    
}