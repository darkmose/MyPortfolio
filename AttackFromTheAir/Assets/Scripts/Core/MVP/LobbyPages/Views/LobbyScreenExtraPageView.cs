using Core.GameLogic;
using System;
using UnityEngine;

namespace Core.MVP
{
    public class LobbyScreenExtraPageView : BaseView, IPageView
    {
        [SerializeField] private PlayerExtraWeaponUpgradeSystemView _playerExtraWeaponUpgradeSystem;
        public PlayerExtraWeaponUpgradeSystemView PlayerExtraWeaponUpgradeSystem => _playerExtraWeaponUpgradeSystem;
        public override ViewType View => ViewType.Panel;

        public void InitPresenter(IPagePresenter presenter)
        {
        }
    }
}