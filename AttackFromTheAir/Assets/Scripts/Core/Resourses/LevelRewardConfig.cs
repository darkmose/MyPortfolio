using UnityEngine;

namespace Core.Resourses
{
    [CreateAssetMenu(fileName = "LevelRewardConfig", menuName = "Game/Level Reward Config")]
    public class LevelRewardConfig : ScriptableObject
    {
        [Header("Base Reward Values")]
        [SerializeField] private int baseCoins = 50;
        [SerializeField] private int baseGears = 10;

        [Header("Scaling Factors")]
        [Tooltip("Increase in reward per level (1.1 = 10% increase per level)")]
        [SerializeField] private float scalingFactor = 1.1f;

        [Header("Random Range for Variability")]
        [Range(0.8f, 1.2f)][SerializeField] private float minMultiplier = 0.8f;
        [Range(0.8f, 1.2f)][SerializeField] private float maxMultiplier = 1.2f;

        private System.Random random = new System.Random();

        public int GenerateGrearsRewards(int levelNumber)
        {
            int gears = Mathf.RoundToInt(baseGears * Mathf.Pow(scalingFactor, levelNumber - 1));

            gears = random.Next(
                Mathf.RoundToInt(gears * minMultiplier),
                Mathf.RoundToInt(gears * maxMultiplier)
            );

            return gears;
        }

        public int GenerateCoinsRewards(int levelNumber)
        {
            int coins = Mathf.RoundToInt(baseCoins * Mathf.Pow(scalingFactor, levelNumber - 1));
           
            coins = random.Next(
                Mathf.RoundToInt(coins * minMultiplier),
                Mathf.RoundToInt(coins * maxMultiplier)
            );
            return coins;
        }

    }
}

