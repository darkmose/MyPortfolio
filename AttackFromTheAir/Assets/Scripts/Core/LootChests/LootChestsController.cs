using Configuration;
using Core.MVP;
using Core.Tools;
using Core.UI;
using Core.Utilities;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.LootChests
{
    public interface ILootChestsController : System.IDisposable
    {
        SimpleEvent LootChestsMiniGameOverEvent { get; }
        SimpleEvent LevelRewardLootTakenEvent { get; }
        void SetActiveChestMiniGame(bool isActive);
        void InitChestMiniGame();
        void SetActiveLevelRewardChestPanel(bool isActive);
        void InitLevelRewardChest();
        void SetMiniGameTapAvailability(bool isAvailable);
        void SetLevelRewardChestTapAvailability(bool isAvailable);
    }

    public class LootChestsController : ILootChestsController
    {
        private LootChestMiniGame _lootChestMiniGame;
        private LootChestsConfiguration _configuration;
        private LootChestRewardPanel _chestRewardPanel;
        private IWinScreenPresenter _winScreenPresenter;
        public SimpleEvent LootChestsMiniGameOverEvent { get; } = new SimpleEvent();
        public SimpleEvent LevelRewardLootTakenEvent { get; } = new SimpleEvent();

        public LootChestsController(IWinScreenPresenter winScreenPresenter)
        {
            _configuration = UnityEngine.Resources.Load<LootChestsConfiguration>("ScriptableObjects/" + nameof(LootChestsConfiguration));
            _winScreenPresenter = winScreenPresenter;
        }

        public void Dispose()
        {
            _lootChestMiniGame?.Dispose();
            _chestRewardPanel?.Dispose();
        }

        public void InitChestMiniGame()
        {
            if (_lootChestMiniGame == null)
            {
                _lootChestMiniGame = new LootChestMiniGame(_configuration.MiniGameConfiguration);
            }
            _lootChestMiniGame.BombDiffuseEvent.AddListener(OnBombDiffuse);
            _lootChestMiniGame.BombEvent.AddListener(OnBombExploded);
            _lootChestMiniGame.GotLootEvent.AddListener(OnMiniGameGotLoot);
            _lootChestMiniGame.InitChests();
            _lootChestMiniGame.LinkView(_winScreenPresenter.ProxyView.View.LootChestMiniGameView);
        }

        private void OnMiniGameGotLoot()
        {
            _lootChestMiniGame.SetTapAvailability(false);
            Timer.SetTimer(1f, () => { _lootChestMiniGame.SetTapAvailability(true); });
        }

        private void OnBombExploded()
        {
            LootChestsMiniGameOverEvent.Notify();
        }

        private void OnBombDiffuse()
        {
            //TODO Add Diffuse Sound
        }

        public void SetActiveChestMiniGame(bool isActive)
        {
            _winScreenPresenter.SetActiveLootChestMiniGame(isActive);
        }

        public void SetActiveLevelRewardChestPanel(bool isActive)
        {
            if (isActive)
            {
                _chestRewardPanel.ShowPanel();
            }
            else 
            {
                _chestRewardPanel.HidePanel();
            }
        }

        public void InitLevelRewardChest()
        {
            _chestRewardPanel = new LootChestRewardPanel();
            var rartities = Enum.GetNames(typeof(LootChestRarity));
            var randomChestRarity = (LootChestRarity)Random.Range(0, rartities.Length); //TODO Take chest from level descriptor
            if (randomChestRarity == LootChestRarity.LevelReward)
            {
                randomChestRarity = LootChestRarity.Simple;
            }

            var possibleLoot = _configuration.GetPossibleChestLoot(randomChestRarity);
            var randomLoot = Random.Range(0, possibleLoot.Count);
            var loot = possibleLoot[randomLoot];

            var panelView = _winScreenPresenter.ProxyView.View.LootChestRewardPanelView;
            _chestRewardPanel.InitChest(randomChestRarity, loot);
            _chestRewardPanel.LinkView(panelView);
            _chestRewardPanel.LootTaken.AddListener(OnLevelRewardLootTaken);
        }

        private void OnLevelRewardLootTaken()
        {
            LevelRewardLootTakenEvent.Notify();
        }

        public void SetMiniGameTapAvailability(bool isAvailable)
        {
            _lootChestMiniGame.SetTapAvailability(isAvailable);
        }

        public void SetLevelRewardChestTapAvailability(bool isAvailable)
        {
            _chestRewardPanel.SetTapAvailability(isAvailable);
        }
    }
}