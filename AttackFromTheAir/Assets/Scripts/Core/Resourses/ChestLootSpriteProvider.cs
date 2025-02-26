using Core.LootChests;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Resourses
{
    [CreateAssetMenu(fileName = nameof(ChestLootSpriteProvider), menuName = "ScriptableObjects/" + nameof(ChestLootSpriteProvider))]
    public class ChestLootSpriteProvider : ScriptableObject
    {
        [SerializeField] private List<ChestLootSpriteDescriptor> _spriteDescriptors;
        private Dictionary<ChestLootType, ChestLootSpriteDescriptor> _spritesDictionary;

        private void Prepare()
        {
            if (_spritesDictionary == null)
            {
                _spritesDictionary = new Dictionary<ChestLootType, ChestLootSpriteDescriptor>();
                foreach (var spriteDescriptor in _spriteDescriptors)
                {
                    _spritesDictionary.Add(spriteDescriptor.LootType, spriteDescriptor);
                }
            }
        }

        public Sprite ProvideByLootType(ChestLootType lootType)
        {
            Prepare();
            if (_spritesDictionary.TryGetValue(lootType, out var spriteDescriptor))
            {
                return spriteDescriptor.LootSprite;
            }
            else
            {
                throw new System.ArgumentException($"Could not find sprite of {lootType} loot");
            }
        }
    }
    
    [System.Serializable]
    public class ChestLootSpriteDescriptor
    {
        public ChestLootType LootType;
        public Sprite LootSprite;
    }
}