using System;
using System.Collections.Generic;
using Core.Buildings;
using Core.Units;
using UnityEngine;

namespace Core.Resourses
{
    public enum ChestColor
    {
        Red,
        Blue,
        Yellow
    }

    public enum ResourceType
    {
        Hammers,       
        Coins,         
        XP,          
        XPBase,
        MultiplyBy2,    
        MultiplyBy3,
        MultiplyBy4,
        MultiplyBy5,
        MultiplyBy6,
        MultiplyBy7,
        MultiplyBy8,
        MultiplyBy9,
        MultiplyBy10,
        Cutters,
        Mine,
        Gems,
        Cards

    }
    public enum EffectType
    {
        Explosive
    }


    [CreateAssetMenu(fileName = "SpriteHolder", menuName = "ScriptableObjects/SpriteHolder", order = 1)]
    public class SpriteHolder : ScriptableObject
    {
        [SerializeField] public List<UnitSpriteDescriptor> UnitSprites;
        [SerializeField] public List<ResourceSpriteData> ResourceSprites;
        [SerializeField] public List<EffectSpriteData> EffectSprites;
        private Dictionary<UnitCategory, Sprite> _unitSpritesDict;

        private void PrepareUnitSpritesDictionary()
        {
            if (_unitSpritesDict == null)
            {
                _unitSpritesDict = new Dictionary<UnitCategory, Sprite>();
                foreach (var descr in UnitSprites)
                {
                    _unitSpritesDict.Add(descr.UnitCategory, descr.UnitSpriteImage);
                }
            }
        }

        public Sprite GetUnitSprite(UnitCategory unitCategory)
        {
            PrepareUnitSpritesDictionary();
            if (_unitSpritesDict.TryGetValue(unitCategory, out var sprite))
            {
                return sprite;
            }
            else
            {
                throw new System.Exception($"Could not find sprite for unit category {unitCategory}");
            }
        }

        public Sprite GetResourceSprite(ResourceType resourceType)
        {
            foreach (var resourceSprite in ResourceSprites)
            {
                if (resourceSprite.ResourceType == resourceType)
                {
                    return resourceSprite.ResourceSpriteImage;
                }
            }

            Debug.LogError($"Sprite for resource type {resourceType} not found!");
            return null;
        }


        public List<Sprite> GetExplosionSprite(EffectType effectType)
        {
            foreach (var resourceSprite in EffectSprites)
            {
                if (resourceSprite.EffectResourType == effectType)
                {
                    return resourceSprite.ResourceSpriteImage;
                }
            }

            Debug.LogError($"Sprites for resource type {effectType} not found!");
            return null;
        }
    }

    [Serializable]
    public class UnitSpriteDescriptor
    {
        public UnitCategory UnitCategory;
        public Sprite UnitSpriteImage;
    }

    [Serializable]
    public class ResourceSpriteData
    {
        public ResourceType ResourceType;
        public Sprite ResourceSpriteImage;
    }

    [Serializable] 
    public class EffectSpriteData
    {
        public EffectType EffectResourType;
        public List<Sprite> ResourceSpriteImage;
    }
}


