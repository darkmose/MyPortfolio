using Core.GameLogic;
using Core.Level;
using Core.UI;
using System;
using Zenject;

namespace Core.MVP
{
    public class LobbyScreenLevelInfoPagePresenter : BasePagePresenterModelUseCases<LobbyScreenLevelInfoPageView, LobbyScreenLevelInfoPageModel, LobbyScreenLevelInfoPageUseCases>
    {
        private LevelSelector _levelSelector;
        private BeforeLevelDroneSelector _beforeLevelDroneSelector;
        private MapLevelSelector _mapLevelSelector;

        public LobbyScreenLevelInfoPagePresenter(DiContainer diContainer) : base(diContainer)
        {
            _levelSelector = diContainer.Resolve<LevelSelector>();
            _beforeLevelDroneSelector = diContainer.Resolve<BeforeLevelDroneSelector>();
            _mapLevelSelector = diContainer.Resolve<MapLevelSelector>();
        }

        protected override void InitInner(LobbyScreenLevelInfoPageView view, LobbyScreenLevelInfoPageModel model, LobbyScreenLevelInfoPageUseCases useCases)
        {
            Model.InitLevelSelector(_levelSelector);
            //Model.CurrentMissionGamemode.RegisterValueChangeListener(view.GamemodePanelSelector.SelectGamemode);
            _beforeLevelDroneSelector.LinkView(view.BeforeLevelDroneSelectorView);
            _mapLevelSelector.LinkView(view.MapLevelSelectorView);
        }

        protected override void OnShowPage()
        {
            base.OnShowPage();
            //View.GamemodePanelSelector.SelectGamemode(Model.CurrentMissionGamemode.Value);
        }
    }
}