using Core.GameLogic;
using Core.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Core.MVP
{
    public class LobbyScreenLevelInfoPageView : BaseView, IPageView
    {
        [SerializeField] private BeforeLevelDroneSelectorView _beforeLevelDroneSelectorView;
        [SerializeField] private GamemodePanelSelector _gamemodePanelSelector;
        [SerializeField] private MapLevelSelectorView _mapLevelSelectorView;
        public MapLevelSelectorView MapLevelSelectorView => _mapLevelSelectorView;
        public BeforeLevelDroneSelectorView BeforeLevelDroneSelectorView => _beforeLevelDroneSelectorView;
        public GamemodePanelSelector GamemodePanelSelector => _gamemodePanelSelector;
        public override ViewType View => ViewType.Panel;
        public void InitPresenter(IPagePresenter presenter)
        {
        }
    }
}