using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Resourses
{

    public enum ChestType
    {
        Common,      // ќбычный сундук
        Golden,      // «олотой сундук
        Epic         // Ёпичный сундук
    }


    [CreateAssetMenu(fileName = "ChestRewardConfig", menuName = "Game/Chest Reward Config")]
    public class ChestRewardConfig : ScriptableObject
    {
        [SerializeField] private List<RewardRange> rewardRanges = new List<RewardRange>();

        private System.Random random = new System.Random();

        [SerializeField] private List<BigChestReward> _bigChestRewards = new List<BigChestReward>();

        public List<ResourceType> GenerateChestRewards(int boxSize)
        {
            List<ResourceType> resources = new List<ResourceType>();

            foreach (var rewardRange in rewardRanges)
            {
                for (int i = 0; i < rewardRange.CountValue; i++)
                {
                    resources.Add(rewardRange.ResourceType);
                }
            }

            resources = resources.OrderBy(x => random.Next()).ToList();

            if (resources.Count > boxSize)
            {
                resources = resources.Take(boxSize).ToList();
            }

            return resources;
        }


        public List<ResourceType> GenerateRewardsForChestType(ChestType chestType)
        {
            // Ќаходим подход€щий BigChestReward дл€ заданного типа сундука
            BigChestReward chestReward = _bigChestRewards.FirstOrDefault(reward => reward.TypeChester == chestType);

            if (chestReward == null || chestReward.ChestReward.Count < 2)
            {
                Debug.LogWarning($"Ќе удалось найти награды дл€ сундука типа {chestType} или недостаточно наград.");
                return new List<ResourceType>();
            }

            // ќпредел€ем случайное количество наград от 2 до максимально доступного количества
            int rewardCount = random.Next(2, chestReward.ChestReward.Count + 1);

            // ¬ыбираем случайное подмножество наград
            List<ResourceType> selectedRewards = chestReward.ChestReward
                .OrderBy(x => random.Next())
                .Take(rewardCount)
                .ToList();

            return selectedRewards;
        }


    }

    [System.Serializable]
    public class RewardRange
    {
        public ResourceType ResourceType;
        public int CountValue;
    }


    [System.Serializable]
    public class BigChestReward
    {
        public ChestType TypeChester;
        public List<ResourceType> ChestReward;
    }

}


