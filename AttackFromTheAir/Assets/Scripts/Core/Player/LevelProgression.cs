using Core.Storage;
using Core.UI;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Core.PlayerModule
{
    public interface ILevelProgression: IStoragableDictionary
    {
        IPropertyReadOnly<int> CurrentMaxLevel { get; }
        void SetLevelNumber(int levelNumber);
    }

    public class LevelProgression : ILevelProgression
    {
        private const string StorageKey = nameof(LevelProgression);
        private IntProperty _currentMaxLevel = new IntProperty(1);
        public IPropertyReadOnly<int> CurrentMaxLevel => _currentMaxLevel;

        public void SetLevelNumber(int levelNumber)
        {
            _currentMaxLevel.SetValue(levelNumber, true);
        }

        public void Save(Dictionary<string, object> data)
        {
            data[nameof(LevelProgression)] = new Dictionary<string, object>()
            {
                [nameof(CurrentMaxLevel)] = CurrentMaxLevel.Value,
            };
        }

        public void Load(Dictionary<string, object> data)
        {
            if (data.ContainsKey(nameof(LevelProgression)))
            {
                var storageData = (JObject)data[nameof(LevelProgression)];
                var level = (int)storageData[nameof(CurrentMaxLevel)];
                SetLevelNumber(level);
            }
        }

        public void Init()
        {
        }
    }
}