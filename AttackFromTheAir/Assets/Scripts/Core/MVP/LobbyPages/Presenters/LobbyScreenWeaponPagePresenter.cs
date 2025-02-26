using Core.GameLogic;
using Core.UI;
using System;
using Zenject;

namespace Core.MVP
{
    public class LobbyScreenWeaponPagePresenter : BasePagePresenterModelUseCases<LobbyScreenWeaponPageView, LobbyScreenWeaponPageModel, LobbyScreenWeaponPageUseCases>
    {
        private PlayerWeaponUpgradeSystem _playerWeaponUpgradeSystem;

        public LobbyScreenWeaponPagePresenter(DiContainer diContainer) : base(diContainer)
        {
            _playerWeaponUpgradeSystem = diContainer.Resolve<PlayerWeaponUpgradeSystem>();  
        }

        protected override void InitInner(LobbyScreenWeaponPageView view, LobbyScreenWeaponPageModel model, LobbyScreenWeaponPageUseCases useCases)
        {
            _playerWeaponUpgradeSystem.InitWeaponUpgradeSystemView(view.PlayerWeaponUpgradeSystemView);
        }
    }
}