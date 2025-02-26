using Core.Tools;
using Core.Utilities;
using System;

namespace Core.LootChests
{
    public class LootChestRewardPanel : IDisposable
    {
        private LootChest _lootChest;
        private LootChestRewardPanelView _view;
        private bool _canTap;
        public LootChest LootChest => _lootChest;
        public SimpleEvent LootTaken { get; } = new SimpleEvent();

        public void InitChest(LootChestRarity rarity, ChestLootType lootType)
        {
            _lootChest = new LootChest();
            _lootChest.InitRarity(rarity);
            _lootChest.InitLoot(lootType);
            _lootChest.OpenRequestEvent.AddListener(OnChestOpenRequest);
        }

        private void OnChestOpenRequest(ILootChest chest)
        {
            if (!chest.IsOpened.Value && _canTap)
            {
                chest.LootApearedEvent.AddListener(OnLootApeared);
                chest.Open();
            }
        }

        private void OnLootApeared(ILootChest chest)
        {
            chest.LootApearedEvent.RemoveListener(OnLootApeared);
            Timer.SetTimer(1f, ()=> 
            {
                LootTaken.Notify();
                HidePanel();
            });
        }

        public void LinkView(LootChestRewardPanelView view)
        {
            _view = view;
            _view.LinkModel(this);
        }

        public void SetTapAvailability(bool isAvailable)
        {
            _canTap = isAvailable;
        }

        public void ShowPanel()
        {
            _view.gameObject.SetActive(true);
        }

        public void HidePanel()
        {
            _view.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            LootTaken.RemoveAllListeners();
            _lootChest.Dispose();
            _view.Dispose();
        }
    }
}