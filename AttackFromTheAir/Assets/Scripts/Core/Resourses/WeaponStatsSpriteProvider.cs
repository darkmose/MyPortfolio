using System.Collections.Generic;
using UnityEngine;

namespace Core.Resourses
{
    public enum PlayerWeaponStats
    {
        ProjectileSpeed,
        ProjectilesCount,
        ReloadSpeed,
        Damage,
        ContinuousFirerate,
        ContinuousFiresBeforeReload
    }

    [CreateAssetMenu(fileName ="WeaponStatsSpriteProvider", menuName ="ScriptableObjects/WeaponStatsSpriteProvider")]
    public class WeaponStatsSpriteProvider : ScriptableObject
    {
        [SerializeField] private List<WeaponStatSpriteDescriptor> _weaponStatSpriteDescriptors;
        private Dictionary<PlayerWeaponStats, Sprite> _spriteDictionary;

        private void Prepare()
        {
            if (_spriteDictionary == null)
            {
                _spriteDictionary = new Dictionary<PlayerWeaponStats, Sprite>();

                foreach (var descr in _weaponStatSpriteDescriptors)
                {
                    _spriteDictionary.Add(descr.WeaponStat, descr.Sprite);
                }
            }
        }

        public Sprite ProvideSprite(PlayerWeaponStats weaponStat)
        {
            Prepare();
            if (_spriteDictionary.TryGetValue(weaponStat, out var sprite))
            {
                return sprite;
            }
            else
            {
                throw new System.Exception($"Provider could not find sprite for weapon stat : {weaponStat}");
            }
        }
    }

    [System.Serializable]
    public class WeaponStatSpriteDescriptor
    {
        public PlayerWeaponStats WeaponStat;
        public Sprite Sprite;
    }
}