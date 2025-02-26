using Core.UI;
using Core.Utilities;
using System;
using UnityEngine;

namespace Core.LootChests
{
    public interface ILootChest
    {
        LootChestRarity Rarity { get; }
        ChestLoot Loot { get; }
        IPropertyReadOnly<bool> IsOpened { get; }
        SimpleEvent<ILootChest> OpenRequestEvent { get; }
        SimpleEvent<ILootChest> LootApearedEvent { get; }
        void Open();
    }

    public interface ILootChestInitor
    {
        void InitRarity(LootChestRarity rarity);
        void InitLoot(ChestLootType lootType);
    }

    public class LootChest : ILootChest, ILootChestInitor, ILootChestViewLink, IDisposable
    {
        private BoolProperty _isOpened = new BoolProperty(false);
        private SimpleEvent<ILootChest> _openRequestEvent = new SimpleEvent<ILootChest>();
        private SimpleEvent<ILootChest> _lootApearedEvent = new SimpleEvent<ILootChest>();
        public LootChestRarity Rarity { get; set; }
        public ChestLoot Loot { get; set; }
        public IPropertyReadOnly<bool> IsOpened => _isOpened;
        public LootChestView Link { get; set; }
        public SimpleEvent<ILootChest> OpenRequestEvent => _openRequestEvent;
        public SimpleEvent<ILootChest> LootApearedEvent => _lootApearedEvent;

        public void InitLoot(ChestLootType lootType)
        {
            Loot = new ChestLoot(lootType);
        }

        public void InitRarity(LootChestRarity rarity)
        {
            Rarity = rarity;
        }

        public void LinkView(LootChestView link)
        {
            Link = link;
            link.ChestViewClickRequestEvent.AddListener(OnViewClick);
            link.LinkModel(this);
        }

        private void OnViewClick()
        {
            _openRequestEvent.Notify(this);
        }

        public void Open()
        {
            _isOpened.SetValue(true);
            Link.Open();
            Loot.Appear(()=> LootApearedEvent.Notify(this));
        }

        public void Dispose()
        {
            _isOpened.RemoveAllListeners();
            _openRequestEvent.RemoveAllListeners();
            _lootApearedEvent.RemoveAllListeners();
            Link.ChestViewClickRequestEvent.RemoveAllListeners();
        }
    }
}