using Core.Level;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Resourses
{
    [CreateAssetMenu(fileName = "ResourceHolder", menuName = "ScriptableObjects/Resource Holder")]
    public class ResourceHolder : ScriptableObject
    {
        public List<LevelDescriptor> Levels => _levels;
        public ToastMessage ToastMessagePrefab => _toastMessagePrefab;
        public ToastMessage ToastImageMoneyPrefab => _toastImageMoneyPrefab;

        [SerializeField] private List<LevelDescriptor> _levels = new List<LevelDescriptor>();
        [SerializeField] private ToastMessage _toastMessagePrefab;
        [SerializeField] private ToastMessage _toastImageMoneyPrefab;

        public LevelDescriptor GetLevel(int levelNumber)
        {
            var result = _levels.Find(levelData => levelData.LevelNumber == levelNumber);

            return result;
        }

        public LevelDescriptor GetLevelRepeatly(int levelNumber) 
        {
            LevelDescriptor result;
            if (levelNumber <= Levels.Count)
            {
                result = GetLevel(levelNumber);
            }
            else
            {
                int repeatCount = levelNumber / Levels.Count;
                int levelOffset = levelNumber - (repeatCount * Levels.Count);
                if (levelOffset == 0)
                {
                    result = GetLevel(Levels.Count);
                }
                else
                {
                    result = GetLevel(levelOffset);
                }
            }

            return result;
        }
    }
}