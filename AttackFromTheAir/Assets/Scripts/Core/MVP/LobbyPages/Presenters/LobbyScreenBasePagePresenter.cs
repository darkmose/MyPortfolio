using Core.LobbyBase;
using Core.UI;
using System;
using UnityEngine;
using Zenject;

namespace Core.MVP
{
    public class LobbyScreenBasePagePresenter : BasePagePresenterModelUseCases<LobbyScreenBasePageView, LobbyScreenBasePageModel, LobbyScreenBasePageUseCases>
    {
        private readonly IPlayerBaseProgression _playerBaseProgression;
        private readonly PlayerBasesManager _playerBasesManager;
        private readonly BaseObjectSelector _baseObjectSelector;
        private PlayerBaseControlPanel _playerBaseControlPanel;
        public PlayerBaseControlPanel PlayerBaseControlPanel => _playerBaseControlPanel;

        public LobbyScreenBasePagePresenter(DiContainer diContainer) : base(diContainer)
        {
            _playerBaseProgression = diContainer.Resolve<IPlayerBaseProgression>();
            _playerBasesManager = diContainer.Resolve<PlayerBasesManager>();
            _baseObjectSelector = diContainer.Resolve<BaseObjectSelector>();
        }

        protected override void InitInner(LobbyScreenBasePageView view, LobbyScreenBasePageModel model, LobbyScreenBasePageUseCases useCases)
        {
            _baseObjectSelector.LinkView(View.BaseObjectSelectorView);
            _playerBaseControlPanel = view.PlayerBaseControlPanel;
            _playerBaseControlPanel.OnPointerClickEvent.AddListener(OnPointerClick);
            _playerBaseControlPanel.OnDragEvent.AddListener(OnDrag);
            _playerBaseControlPanel.OnDragBeginEvent.AddListener(OnDragBegin);
            _playerBaseControlPanel.OnDragEndEvent.AddListener(OnDragEnd);
            _playerBasesManager.View.InitCameraRenderImage(view.BaseRenderImage);
            SubscribeDataChange();
        }

        private void OnDragEnd(Vector2 vector)
        {
            var nearestBuilding = _playerBasesManager.GetNearestBuilding();
            if (nearestBuilding != null)
            {
                _baseObjectSelector.SelectBaseObject(nearestBuilding);
            }
            else
            {
                _baseObjectSelector.NextSelectedObject();
            }
        }

        private void OnDragBegin(Vector2 vector)
        {
            _baseObjectSelector.ClearViewSelection();
        }

        private void OnDrag(Vector2 vector)
        {
            _playerBasesManager.MoveCamera(vector);
        }

        private void OnPointerClick(Vector2 vector)
        {
            var tapBuilding = _playerBasesManager.SelectBaseObjectByScreenTap(vector);
            if (tapBuilding != null)
            {
                _baseObjectSelector.SelectBaseObject(tapBuilding);
            }
        }

        private void OnBaseIndexChanged(int currentBaseIndex)
        {
            RefreshBaseBuildingTabs();
        }

        private void RefreshBaseBuildingTabs()
        {
            View.TabGenerator.ClearTabs();

            var currentBase = _playerBasesManager.CurrentBase;
            foreach (var keyValuePair in currentBase.BaseObjectsDict)
            {
                var buildingType = keyValuePair.Key;
                View.TabGenerator.AddTab(buildingType, UseCases.OnBaseBuildingTabStateChanged);
            }

            View.TabGenerator.ToggleOnFirstTab();
            View.TabGenerator.ResetRootPosition();
        }

        protected override void OnHidePage()
        {
            View.TabGenerator.ClearTabs();
            _playerBasesManager.CurrentBaseIndexChanged.RemoveListener(OnBaseIndexChanged);
            _playerBasesManager.SetCurrentBaseIndex(_playerBaseProgression.SelectedBaseIndex);   
        }

        protected override void OnShowPage()
        {
            RefreshBaseBuildingTabs();
            RefreshData();
            _playerBasesManager.CurrentBaseIndexChanged.AddListener(OnBaseIndexChanged);
        }

        private void RefreshData()
        {
            View.SetCurrentResourceAmount(Model.CurrentObjectResources.Value);
            View.SetResourceGoal(Model.GoalObjectResources.Value);
            View.SetResourceProgress(Model.UpgradeProgress.Value);
            View.SetUnlockedStatus(Model.IsObjectUnlocked.Value);
            View.SetObjectLevel(Model.ObjectLevel.Value);
            View.SetSameObjectsAmount(Model.SameObjectsCount.Value);

            View.SetLvlUpSoftCurrencyCost(Model.SoftCurrencyToUpgrade.Value);
            View.SetLvlUpHardCurrencyCost(Model.HardCurrencyToUpgrade.Value);
            View.SetSoftCurrencyLvlUpButtonVisible(Model.IsSoftCurrencyLevelUpButtonShown.Value);
            View.SetHardCurrencyLvlUpButtonVisible(Model.IsHardCurrencyLevelUpButtonShown.Value);
            View.SetSoftCurrencyLvlUpButtonIntaractable(Model.IsEnoughSoftCurrency.Value);
            View.SetHardCurrencyLvlUpButtonIntaractable(Model.IsEnoughHardCurrency.Value);

            View.SetBuildingIcon(Model.BuildingIcon.Value);

            View.SetBaseUnlockProgress(Model.BaseUnlockProgress.Value);
            View.SetBaseName(Model.BaseName.Value);

            View.SetInteractableObserveNextBaseButton(Model.CanSwitchNextBase.Value);
            View.SetInteractableObservePreviousBaseButton(Model.CanSwitchPreviousBase.Value);
            View.SetActiveBaseInfoPanel(Model.BaseObserveMode.Value);
        }

        private void SubscribeDataChange()
        {
            Model.CurrentObjectResources.RegisterValueChangeListener(View.SetCurrentResourceAmount);
            Model.GoalObjectResources.RegisterValueChangeListener(View.SetResourceGoal);
            Model.UpgradeProgress.RegisterValueChangeListener(View.SetResourceProgress);
            Model.IsObjectUnlocked.RegisterValueChangeListener(View.SetUnlockedStatus);
            Model.ObjectLevel.RegisterValueChangeListener(View.SetObjectLevel);
            Model.SameObjectsCount.RegisterValueChangeListener(View.SetSameObjectsAmount);

            Model.SoftCurrencyToUpgrade.RegisterValueChangeListener(View.SetLvlUpSoftCurrencyCost);
            Model.HardCurrencyToUpgrade.RegisterValueChangeListener(View.SetLvlUpHardCurrencyCost);
            Model.IsSoftCurrencyLevelUpButtonShown.RegisterValueChangeListener(View.SetSoftCurrencyLvlUpButtonVisible);
            Model.IsHardCurrencyLevelUpButtonShown.RegisterValueChangeListener(View.SetHardCurrencyLvlUpButtonVisible);
            Model.IsEnoughSoftCurrency.RegisterValueChangeListener(View.SetSoftCurrencyLvlUpButtonIntaractable);
            Model.IsEnoughHardCurrency.RegisterValueChangeListener(View.SetHardCurrencyLvlUpButtonIntaractable);

            Model.BuildingIcon.RegisterValueChangeListener(View.SetBuildingIcon);

            Model.BaseUnlockProgress.RegisterValueChangeListener(View.SetBaseUnlockProgress);
            Model.BaseName.RegisterValueChangeListener(View.SetBaseName);

            Model.CanSwitchNextBase.RegisterValueChangeListener(View.SetInteractableObserveNextBaseButton);
            Model.CanSwitchPreviousBase.RegisterValueChangeListener(View.SetInteractableObservePreviousBaseButton);
            Model.BaseObserveMode.RegisterValueChangeListener(View.SetActiveBaseInfoPanel);
        }

        public void OnResourceSendButtonDown()
        {
            UseCases.OnSendResourceButtonDown();
        }

        public void OnResourceSendButtonUp()
        {
            UseCases.OnSendResourceButtonUp();
        }

        public void OnAdvLevelUpButtonClick()
        {
            UseCases.OnAdvLevelUpButtonClick();
        }

        public void OnSoftCurrencyLevelUpButtonClick()
        {
            UseCases.OnSoftCurrencyLevelUpButtonClick();
        }

        public void OnHardCurrencyLevelUpButtonClick()
        {
            UseCases.OnHardCurrencyLevelUpButtonClick();
        }

        public void OnPreviousObjectButtonClick()
        {
            UseCases.OnPreviousObjectButtonClick();
        }

        public void OnNextObjectButtonClick()
        {
            UseCases.OnNextObjectButtonClick();
        }

        public void OnSwitchBaseObserveModeButtonClick()
        {
            UseCases.OnSwitchBaseObserveModeButtonClick();
        }

        public void OnSwitchBaseToNextButtonClick()
        {
            UseCases.OnSwitchBaseToNext();
        }

        public void OnSwitchBaseToPreviousButtonClick()
        {
            UseCases.OnSwitchBaseToPrevious();
        }
    }
}