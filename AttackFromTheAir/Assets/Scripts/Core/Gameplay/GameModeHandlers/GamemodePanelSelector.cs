using Core.Level;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameLogic
{
    public class GamemodePanelSelector : MonoBehaviour
    {
        [SerializeField] private List<GamemodePanelView> _gamemodesList;

        public void SelectGamemode(GameMode gameMode)
        {
            foreach (var gamemode in _gamemodesList)
            {
                if (gamemode.GameMode == gameMode)
                {
                    gamemode.Select();
                }
                else
                {
                    gamemode.Unselect();
                }
            }
        }
    }
}