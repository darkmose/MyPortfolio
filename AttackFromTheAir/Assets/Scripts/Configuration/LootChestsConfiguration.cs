using Core.LootChests;
using System.Collections.Generic;
using UnityEngine;

namespace Configuration
{
    [CreateAssetMenu(fileName =nameof(LootChestsConfiguration), menuName ="ScriptableObjects/" + nameof(LootChestsConfiguration))]
    public class LootChestsConfiguration : ScriptableObject
    {
        [Header("Main Settings")]
        [SerializeField] private List<LootAmountDescriptor> _lootAmounts;
        [Header("Config for Loot Chest MiniGame")]
        [Space(10)]
        public LootChestMiniGameConfiguration MiniGameConfiguration;
        [Header("Reward Loot Chest Configuration")]
        [SerializeField] private List<LootChestRewardDescriptor> _lootChestRewardDescriptors;

        public List<ChestLootType> GetPossibleChestLoot(LootChestRarity chestRarity)
        {
            var lootDescriptor = _lootChestRewardDescriptors.Find(pred=>pred.ChestRarity == chestRarity);
            if (lootDescriptor != null)
            {
                return lootDescriptor.Loot;
            }
            else
            {
                throw new System.Exception($"Could not find possible loot for {chestRarity} chest");
            }
        }

        public int GetLootAmount(ChestLootType lootType)
        {
            var amountDescriptor = _lootAmounts.Find(pred=>pred.Type == lootType);
            if (amountDescriptor != null)
            {
                if (amountDescriptor.FromRange)
                {
                    var randValue = Random.Range(amountDescriptor.Min, amountDescriptor.Max + 1);
                    return randValue;
                }
                else
                {
                    return amountDescriptor.LootAmount;
                }
            }
            else
            {
                return 1;
            }
        }
    }

    [System.Serializable]
    public class LootChestMiniGameConfiguration
    {
        public List<ChestLootType> Loot;
        public int BombsAmount = 5;
        public int BombDefusersAmount = 1;
    }

    [System.Serializable]
    public class LootAmountDescriptor
    {
        public ChestLootType Type;
        public int LootAmount;

        public bool FromRange;
        public int Min = 1;
        public int Max = 5;
    }

    [System.Serializable]
    public class LootChestRewardDescriptor
    {
        public LootChestRarity ChestRarity;
        public List<ChestLootType> Loot;
    }
}