using Core.GameLogic;
using System;
using Zenject;

namespace Core.MVP
{
    public class LobbyScreenExtraPagePresenter : BasePagePresenterModelUseCases<LobbyScreenExtraPageView, LobbyScreenExtraPageModel, LobbyScreenExtraPageUseCases>
    {
        private PlayerExtraWeaponUpgradeSystem _playerExtraWeaponUpgradeSystem;

        public LobbyScreenExtraPagePresenter(DiContainer diContainer) : base(diContainer)
        {
            _playerExtraWeaponUpgradeSystem = diContainer.Resolve<PlayerExtraWeaponUpgradeSystem>();
        }

        protected override void InitInner(LobbyScreenExtraPageView view, LobbyScreenExtraPageModel model, LobbyScreenExtraPageUseCases useCases)
        {
            _playerExtraWeaponUpgradeSystem.InitWeaponUpgradeSystemView(view.PlayerExtraWeaponUpgradeSystem);
        }
    }
}