using Core.UI;
using System;
using UnityEngine;

namespace Core.LootChests
{
    public class LootChestRewardPanelView : MonoBehaviour, IDisposable
    {
        [SerializeField] private LootChestView _lootPrefab;
        [SerializeField] private Transform _chestRoot;
        private LootChestView _loot;

        public void Dispose()
        {
            _chestRoot.ClearAllChild();
        }

        public void LinkModel(LootChestRewardPanel lootChestRewardPanel)
        {
            _loot = Instantiate<LootChestView>(_lootPrefab, _chestRoot);
            lootChestRewardPanel.LootChest.LinkView(_loot);
        }
    }
}