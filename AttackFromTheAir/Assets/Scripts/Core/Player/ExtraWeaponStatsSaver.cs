using Core.GameLogic;
using Core.Resourses;
using Core.Storage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.PlayerModule
{
    public interface IExtraWeaponStatsSaver : IStoragableDictionary
    {
        void SetStatLevel(PlayerExtraWeaponType playerExtraWeaponType, PlayerWeaponStats stat, int level);
        int GetStatLevel(PlayerExtraWeaponType playerExtraWeaponType, PlayerWeaponStats stat);
    }

    public class ExtraWeaponStatsSaver : IExtraWeaponStatsSaver
    {
        private const string EXTRA_WEAPON_KEY = "ExtraWeapon_";
        private const string STAT_DATA_TYPE_KEY = "StatDataType";
        private const string STAT_DATA_KEY = "StatData";
        private const string STAT_KEY = "Stat_";
        private const string EXTRA_WEAPON_TYPE_KEY = "ExtraWeaponType";
        private const string EXTRA_WEAPON_STATS_KEY = "ExtraWeaponStats";
        
        private readonly Dictionary<PlayerExtraWeaponType, Dictionary<PlayerWeaponStats, int>> _statsDictionary;

        public ExtraWeaponStatsSaver()
        {
            _statsDictionary = new Dictionary<PlayerExtraWeaponType, Dictionary<PlayerWeaponStats, int>>();
            var extraWeapons = Enum.GetNames(typeof(PlayerExtraWeaponType));
            for (int i = 0; i < extraWeapons.Length - 1; i++)
            {
                var extraWeapon = (PlayerExtraWeaponType)i;
                _statsDictionary.Add(extraWeapon, new Dictionary<PlayerWeaponStats, int>());
            }
        }

        public void SetStatLevel(PlayerExtraWeaponType playerExtraWeaponType, PlayerWeaponStats stat, int level)
        {
            if (_statsDictionary.TryGetValue(playerExtraWeaponType, out var stats))
            {
                if (stats.ContainsKey(stat))
                {
                    stats[stat] = level;
                }
                else
                {
                    stats.Add(stat, level);
                }
            }
        }

        public int GetStatLevel(PlayerExtraWeaponType playerExtraWeaponType, PlayerWeaponStats stat)
        {
            if (_statsDictionary.TryGetValue(playerExtraWeaponType, out var stats))
            {
                if (stats.TryGetValue(stat, out var value))
                {
                    return value;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public void Init()
        {
        }

        public void Load(Dictionary<string, object> data)
        {
            if (data.TryGetValue(nameof(ExtraWeaponStatsSaver), out var rawData))
            {
                var storageData = (JObject)rawData;

                for (int i = 0; i < storageData.Count; i++)
                {
                    var extraWeaponData = (JObject)storageData[EXTRA_WEAPON_KEY + i.ToString()];
                    var extraWeaponType = (PlayerExtraWeaponType)Enum.Parse(typeof(PlayerExtraWeaponType), (string)extraWeaponData[EXTRA_WEAPON_TYPE_KEY]);
                    var extraWeaponStats = (JObject)extraWeaponData[EXTRA_WEAPON_STATS_KEY];

                    for (int k = 0; k < extraWeaponStats.Count; k++)
                    {
                        var stat = (JObject)extraWeaponStats[STAT_KEY + k.ToString()];
                        var statType = (PlayerWeaponStats)Enum.Parse(typeof(PlayerWeaponStats), (string)stat[STAT_DATA_TYPE_KEY]);
                        var statValue = (int)stat[STAT_DATA_KEY];
                        SetStatLevel(extraWeaponType, statType, statValue);
                    }
                }
            }
        }

        public void Save(Dictionary<string, object> data)
        {
            var storageData = new Dictionary<string, object>();

            int weaponIndex = 0;
            foreach (var item in _statsDictionary)
            {
                int statIndex = 0;
                var extraWeaponType = item.Key.ToString();
                var extraWeaponStatsData = new Dictionary<string, object>();

                foreach (var stat in item.Value)
                {
                    var statData = new Dictionary<string, object>()
                    {
                        [STAT_DATA_TYPE_KEY] = stat.Key.ToString(),
                        [STAT_DATA_KEY] = stat.Value
                    };
                    extraWeaponStatsData.Add(STAT_KEY + statIndex.ToString(), statData);
                    statIndex++;
                }

                var extraWeaponData = new Dictionary<string, object>()
                {
                    [EXTRA_WEAPON_TYPE_KEY] = extraWeaponType,
                    [EXTRA_WEAPON_STATS_KEY] = extraWeaponStatsData
                };

                storageData.Add(EXTRA_WEAPON_KEY+ weaponIndex.ToString(), extraWeaponData);
                weaponIndex++;
            }

            data.Add(nameof(ExtraWeaponStatsSaver), storageData);
        }
    }
}