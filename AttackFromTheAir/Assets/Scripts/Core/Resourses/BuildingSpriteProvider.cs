using Core.Buildings;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Resourses
{
    [CreateAssetMenu(fileName = nameof(BuildingSpriteProvider), menuName ="ScriptableObjects/"+nameof(BuildingSpriteProvider))]
    public class BuildingSpriteProvider : ScriptableObject
    {
        [SerializeField] private List<BuildingSpriteDescriptor> _spriteDescriptors;
        private Dictionary<BuildingType, Sprite> _spriteDictionary;

        private void Prepare()
        {
            if (_spriteDictionary is null)
            {
                _spriteDictionary = new Dictionary<BuildingType, Sprite>();
                foreach (var spriteDescriptor in _spriteDescriptors)
                {
                    _spriteDictionary.Add(spriteDescriptor.BuildingType, spriteDescriptor.Sprite);
                }
            }
        }

        public Sprite ProvideByType(BuildingType type)
        {
            Prepare();
            if (_spriteDictionary.TryGetValue(type, out var sprite))
            {
                return sprite;
            }
            else
            {
                throw new System.Exception($"Could not find sprite of Building {type}");
            }
        }
    }

    [System.Serializable]
    public class BuildingSpriteDescriptor
    {
        public BuildingType BuildingType;
        public Sprite Sprite;
    }
}