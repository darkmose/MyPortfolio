using Core.Level;
using Core.UI;
using UnityEngine;
using Zenject;

namespace Core.MVP
{
    public class LobbyScreenLevelInfoPageModel : IPageModel
    {
        public IPropertyReadOnly<string> CurrentMissionDescription { get; private set; }
        public IPropertyReadOnly<GameMode> CurrentMissionGamemode { get; private set; }

        public void Init(DiContainer diContainer)
        {
        }

        public void InitLevelSelector(LevelSelector levelSelector)
        {
            CurrentMissionDescription = levelSelector.MissionDescription;
            CurrentMissionGamemode = levelSelector.GameMode;
        }
    }
}