using Core.Level;
using Core.PlayerModule;
using Core.UI;
using UnityEngine;
using Zenject;

namespace Core.MVP
{
    public class LobbyScreenModel : IModel
    {
        public IPropertyReadOnly<int> PlayerLevel { get; }
        public IPropertyReadOnly<int> BaseLevel { get; }
        public IPropertyReadOnly<int> DollarsAmount { get; }
        public IPropertyReadOnly<int> HammersAmount { get; }
        public IPropertyReadOnly<int> GemsAmount { get; }
        public IPropertyReadOnly<float> PlayerExpirience { get; }
        public IPropertyReadOnly<float> BaseExpirience { get; }
        public IPropertyReadOnly<string> CurrentMissionDescription { get; private set; }
        public IPropertyReadOnly<Sprite> CurrentMissionPreview { get; private set; }
        public IPropertyReadOnly<GameMode> CurrentMissionGamemode { get; private set; }

        public LobbyScreenModel(IPlayerExpirienceService playerExpirienceService, IWallet wallet)
        {
            PlayerLevel = playerExpirienceService.PlayerLevel;
            PlayerExpirience = playerExpirienceService.PlayerExpirience;
            BaseLevel = playerExpirienceService.BaseLevel;
            BaseExpirience = playerExpirienceService.BaseExpirience;
            DollarsAmount = wallet.GetMoneyProperty(MoneyType.Coins);
            HammersAmount = wallet.GetMoneyProperty(MoneyType.Hammers);
            GemsAmount = wallet.GetMoneyProperty(MoneyType.Gems);
        }

        public void InitLevelSelector(LevelSelector levelSelector)
        {
            CurrentMissionDescription = levelSelector.MissionDescription;
            CurrentMissionPreview = levelSelector.MissionPreview;
            CurrentMissionGamemode = levelSelector.GameMode;
        }
    }
}