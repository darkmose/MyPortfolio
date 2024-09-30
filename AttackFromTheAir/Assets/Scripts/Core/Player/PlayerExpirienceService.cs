using Core.Storage;
using Core.UI;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Core.PlayerModule
{
    public interface IPlayerExpirienceService : IStoragableDictionary
    {
        IPropertyReadOnly<int> PlayerLevel { get; }
        IPropertyReadOnly<int> BaseLevel { get; }
        IPropertyReadOnly<float> PlayerExpirience { get; }
        IPropertyReadOnly<float> BaseExpirience { get; }
    }

    public class PlayerExpirienceService : IPlayerExpirienceService
    {
        private const string PLAYER_EXPIRIENCE_KEY = "PlayerExp";
        private const string BASE_EXPIRIENCE_KEY = "BaseExp";
        private const string PLAYER_LEVEL_KEY = "PlayerLevel";
        private const string BASE_LEVEL_KEY = "BaseLevel";

        private FloatProperty _playerExpirience = new FloatProperty();
        private FloatProperty _baseExpirience = new FloatProperty();
        private IntProperty _playerLevel = new IntProperty(1);
        private IntProperty _baseLevel = new IntProperty(1);

        public IPropertyReadOnly<int> PlayerLevel => _playerLevel;
        public IPropertyReadOnly<int> BaseLevel => _baseLevel;
        public IPropertyReadOnly<float> PlayerExpirience => _playerExpirience;
        public IPropertyReadOnly<float> BaseExpirience => _baseExpirience;

        public void Init()
        {
        }

        public void Load(Dictionary<string, object> data)
        {
            if (data.TryGetValue(nameof(PlayerExpirienceService), out var storageData))
            {
                var convertedData = (JObject)storageData;
                var playerExp = (float)convertedData[PLAYER_EXPIRIENCE_KEY];
                var baseExp = (float)convertedData[BASE_EXPIRIENCE_KEY];
                var playerLevel = (int)convertedData[PLAYER_LEVEL_KEY];
                int baseLevel = (int)convertedData[BASE_LEVEL_KEY];

                _playerExpirience.SetValue(playerExp, true);
                _playerLevel.SetValue(playerLevel, true);
                _baseExpirience.SetValue(baseExp, true);
                _baseLevel.SetValue(baseLevel, true);
            }
        }

        public void Save(Dictionary<string, object> data)
        {
            var storageData = new Dictionary<string, object>()
            {
                [PLAYER_EXPIRIENCE_KEY] = PlayerExpirience.Value,
                [PLAYER_LEVEL_KEY] = PlayerLevel.Value,
                [BASE_EXPIRIENCE_KEY] = BaseExpirience.Value,
                [BASE_LEVEL_KEY] = BaseLevel.Value
            };

            data[nameof(PlayerExpirienceService)] = storageData;
        }
    }
}