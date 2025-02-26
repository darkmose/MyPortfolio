using Configuration;
using Core.Tools;
using Core.Utilities;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

namespace Core.LootChests
{
    public class LootChestMiniGame : IDisposable
    {
        private LootChestMiniGameConfiguration _lootChestMiniGameConfiguration;
        private List<LootChest> _lootChests = new List<LootChest>();
        private Queue<ChestLootType> _chestLootQueue;
        private LootChestMiniGameView _view;
        private int _bombDiffuseAmount = 0;
        private bool _canTap;
        public List<LootChest> LootChests => _lootChests;
        public SimpleEvent GotLootEvent { get; } = new SimpleEvent();
        public SimpleEvent BombEvent { get; } = new SimpleEvent();
        public SimpleEvent BombDiffuseEvent { get; } = new SimpleEvent();
        public LootChestMiniGame(LootChestMiniGameConfiguration lootChestMiniGameConfiguration)
        {
            _lootChestMiniGameConfiguration = lootChestMiniGameConfiguration;
        }

        public void LinkView(LootChestMiniGameView view)
        {
            _view = view;
            view.LinkModel(this);
        }

        public void InitChests()
        {
            PrepareLootQueue();
            var lootAmount = _chestLootQueue.Count;
            for (int i = 0; i < lootAmount; i++)
            {
                var lootChest = new LootChest();
                var randomLoot = GetLoot();
                lootChest.InitRarity(LootChestRarity.LevelReward);
                lootChest.InitLoot(randomLoot);
                lootChest.OpenRequestEvent.AddListener(OnLootChestOpenRequest);

                _lootChests.Add(lootChest);
            }
        }

        private void OnLootChestOpenRequest(ILootChest chest)
        {
            if (chest.IsOpened.Value || !_canTap)
            {
                return;
            }
            else 
            {
                chest.Open();
                if (chest.Loot.LootType == ChestLootType.BombDefuser)
                {
                    _bombDiffuseAmount++;
                }
                if (chest.Loot.LootType == ChestLootType.Bomb)
                {
                    if (_bombDiffuseAmount > 0)
                    {
                        _bombDiffuseAmount--;
                        BombDiffuseEvent.Notify();
                    }
                    else
                    {
                        BombEvent.Notify();
                    }
                }

                GotLootEvent.Notify();
            }
        }

        private ChestLootType GetLoot()
        {
            if (_chestLootQueue.TryDequeue(out var lootType))
            {
                return lootType;
            }
            else 
            {
                return 0;
            }
        }

        private void PrepareLootQueue()
        {
            var lootTypes = new List<ChestLootType>(_lootChestMiniGameConfiguration.Loot);

            var bombsAmount = _lootChestMiniGameConfiguration.BombsAmount;
            var bombDefusersAmount = _lootChestMiniGameConfiguration.BombDefusersAmount;
            
            for (int i = 0; i < bombsAmount; i++)
            {
                lootTypes.Add(ChestLootType.Bomb);
            }

            for (int i = 0; i < bombDefusersAmount; i++)
            {
                lootTypes.Add(ChestLootType.BombDefuser);
            }

            lootTypes = lootTypes.Shuffle();

            _chestLootQueue = new Queue<ChestLootType>(lootTypes);
        }

        public void SetTapAvailability(bool isAvailable)
        {
            _canTap = isAvailable;
        }

        public void Dispose()
        {
            foreach (var chest in _lootChests)
            {
                chest.Dispose();
            }
            _lootChests.Clear();
            _chestLootQueue.Clear();
            _view.Dispose();

            _bombDiffuseAmount = 0;
            BombEvent.RemoveAllListeners();
            BombDiffuseEvent.RemoveAllListeners();
            GotLootEvent.RemoveAllListeners();
        }
    }
}