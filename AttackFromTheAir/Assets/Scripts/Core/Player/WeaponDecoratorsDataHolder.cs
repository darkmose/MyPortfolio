using Core.GameLogic;
using Core.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

namespace Core.PlayerModule
{
    public interface IWeaponDecoratorsHolder : IStoragableDictionary
    {
        void SetPlayerWeaponLevel(PlayerWeaponType playerWeaponType, int level);
        void SetPlayerExtraWeaponLevel(PlayerExtraWeaponType playerExtraWeaponType, int level);
        int GetPlayerWeaponLevel(PlayerWeaponType playerWeaponType);
        int GetPlayerExtraWeaponLevel(PlayerExtraWeaponType playerExtraWeaponType);
    }

    public class WeaponDecoratorsDataHolder : IWeaponDecoratorsHolder
    {
        private const string WEAPON_TYPE_KEY = "WeaponType";
        private const string WEAPON_RANK_KEY = "WeaponRank";
        private const string WEAPONS_KEY = "Weapons";
        private const string EXTRA_WEAPONS_KEY = "ExtraWeapons";
        private Dictionary<PlayerWeaponType, int> _playerWeaponLevels;
        private Dictionary<PlayerExtraWeaponType, int> _playerExtraWeaponLevels;

        public WeaponDecoratorsDataHolder()
        {
            _playerWeaponLevels = new Dictionary<PlayerWeaponType, int>();
            _playerExtraWeaponLevels = new Dictionary<PlayerExtraWeaponType, int>();
        }

        public int GetPlayerExtraWeaponLevel(PlayerExtraWeaponType playerExtraWeaponType)
        {
            if (_playerExtraWeaponLevels.TryGetValue(playerExtraWeaponType, out var level))
            {
                return level;
            }
            else
            {
                return 1;
            }
        }

        public int GetPlayerWeaponLevel(PlayerWeaponType playerWeaponType)
        {
            if (_playerWeaponLevels.TryGetValue(playerWeaponType, out var level))
            {
                return level;
            }
            else
            {
                return 1;
            }
        }

        public void Load(Dictionary<string, object> data)
        {
            if (data.TryGetValue(nameof(WeaponDecoratorsDataHolder), out var storageData))
            {
                var convertedData = (JObject)storageData;
                var playerWeaponLevels = (JObject)convertedData[WEAPONS_KEY];
                var playerExtraWeaponLevels = (JObject)convertedData[EXTRA_WEAPONS_KEY];

                _playerWeaponLevels.Clear();

                for (int i = 0; i < playerWeaponLevels.Count; i++)
                {
                    var weaponLevel = (JObject)playerWeaponLevels[$"Weapon_{i}"];
                    var type = (PlayerWeaponType)Enum.Parse(typeof(PlayerWeaponType), (string)weaponLevel[WEAPON_TYPE_KEY]);
                    var rank = (int)weaponLevel[WEAPON_RANK_KEY];

                    _playerWeaponLevels.Add(type, rank);
                }

                _playerExtraWeaponLevels.Clear();

                for (int i = 0; i < playerExtraWeaponLevels.Count; i++)
                {
                    var extraWeaponLevel = (JObject)playerExtraWeaponLevels[$"ExtraWeapon_{i}"];
                    var type = (PlayerExtraWeaponType)Enum.Parse(typeof(PlayerExtraWeaponType), (string)extraWeaponLevel[WEAPON_TYPE_KEY]);
                    var rank = (int)extraWeaponLevel[WEAPON_RANK_KEY];

                    _playerExtraWeaponLevels.Add(type, rank);
                }
            }
        }

        public void Init()
        {
            _playerWeaponLevels.Clear();
            var weaponNames = Enum.GetNames(typeof(PlayerWeaponType));
            for (int i = 0; i < weaponNames.Length; i++)
            {
                var weapon = (PlayerWeaponType)i;
                _playerWeaponLevels.Add(weapon, 1);
            }

            _playerExtraWeaponLevels.Clear();
            var extraWeaponNames = Enum.GetNames(typeof(PlayerExtraWeaponType));
            for (int i = 0; i < extraWeaponNames.Length; i++)
            {
                var extraWeapon = (PlayerExtraWeaponType)i;
                _playerExtraWeaponLevels.Add(extraWeapon, 1);
            }
        }

        public void Save(Dictionary<string, object> data)
        {
            //var storageData = new Dictionary<string, object>()
            //{
            //    [PLAYER_WEAPONS_LEVELS_KEY] = _playerWeaponLevels,
            //    [PLAYER_EXTRA_WEAPONS_LEVELS_KEY] = _playerExtraWeaponLevels
            //};

            var storageData = new Dictionary<string, object>();

            var weaponsLevels = new Dictionary<string, object>();
            var extraWeaponsLevels = new Dictionary<string, object>();
            int index = 0;
            foreach (var item in _playerWeaponLevels)
            {
                var weaponLevel = new Dictionary<string, object>()
                {
                    [WEAPON_TYPE_KEY] = item.Key.ToString(),
                    [WEAPON_RANK_KEY] = item.Value
                };

                weaponsLevels.Add($"Weapon_{index}", weaponLevel);
                index++;
            }
            index = 0;
            foreach (var item in _playerExtraWeaponLevels)
            {
                var weaponLevel = new Dictionary<string, object>()
                {
                    [WEAPON_TYPE_KEY] = item.Key.ToString(),
                    [WEAPON_RANK_KEY] = item.Value
                };

                extraWeaponsLevels.Add($"ExtraWeapon_{index}", weaponLevel);
                index++;
            }
            storageData.Add(WEAPONS_KEY, weaponsLevels);
            storageData.Add(EXTRA_WEAPONS_KEY, extraWeaponsLevels);
            data.Add(nameof(WeaponDecoratorsDataHolder), storageData);
        }

        public void SetPlayerExtraWeaponLevel(PlayerExtraWeaponType playerExtraWeaponType, int level)
        {
            if (_playerExtraWeaponLevels.ContainsKey(playerExtraWeaponType))
            {
                _playerExtraWeaponLevels[playerExtraWeaponType] = level;
            }
        }

        public void SetPlayerWeaponLevel(PlayerWeaponType playerWeaponType, int level)
        {
            if (_playerWeaponLevels.ContainsKey(playerWeaponType))
            {
                _playerWeaponLevels[playerWeaponType] = level;
            }
        }
    }
}