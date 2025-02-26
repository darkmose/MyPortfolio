using Core.LootChests;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Resourses
{
    [CreateAssetMenu(fileName =nameof(LootChestSpriteProvider), menuName = "ScriptableObjects/"+ nameof(LootChestSpriteProvider))]
    public class LootChestSpriteProvider : ScriptableObject
    {
        [SerializeField] private List<LootChestSpriteDescriptor> _spriteDescriptors;
        private Dictionary<LootChestRarity, LootChestSpriteDescriptor> _spritesDictionary;

        private void Prepare()
        {
            if (_spritesDictionary == null) 
            {
                _spritesDictionary = new Dictionary<LootChestRarity, LootChestSpriteDescriptor>();
                foreach (var spriteDescriptor in _spriteDescriptors) 
                {
                    _spritesDictionary.Add(spriteDescriptor.LootChestRarity, spriteDescriptor);
                }
            }
        }

        public LootChestSpriteDescriptor ProvideByRarity(LootChestRarity rarity)
        {
            Prepare();
            if (_spritesDictionary.TryGetValue(rarity, out var spriteDescriptor))
            {
                return spriteDescriptor;
            }
            else
            {
                throw new System.ArgumentException($"Could not find sprite of {rarity} loot chest");
            }
        }
    }

    [System.Serializable]
    public class LootChestSpriteDescriptor
    {
        public LootChestRarity LootChestRarity;
        public Sprite ClosedSprite;
        public Sprite OpenedSprite;
    }
}