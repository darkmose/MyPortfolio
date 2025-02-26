using Core.GameLogic;
using System;
using UnityEngine;

namespace Core.MVP
{
    public class LobbyScreenWeaponPageView : BaseView, IPageView
    {
        [SerializeField] private PlayerWeaponUpgradeSystemView _playerWeaponUpgradeSystemView;
        public PlayerWeaponUpgradeSystemView PlayerWeaponUpgradeSystemView => _playerWeaponUpgradeSystemView;
        public override ViewType View => ViewType.Panel;

        public void InitPresenter(IPagePresenter presenter)
        {
        }
    }
}