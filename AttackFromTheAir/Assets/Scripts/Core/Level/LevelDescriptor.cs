using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Level
{

    [CreateAssetMenu(fileName = "Level 1", menuName = "ScriptableObjects/Level")]
    public class LevelDescriptor : ScriptableObject
    {
        public int LevelNumber;
        public GameMode GameMode;
        public AttackGameMode AttackGameMode;
        public DefenceGameMode DefenceGameMode;
        public GameModeSettings GameModeSettings;
        public string MissionDescription;
        public Sprite MissionPreview;
        public LevelData LevelData;
    }

    [Serializable]
    public class GameModeSettings
    {
        public GameEventsCategory EventsToWin;
        public GameEventsCategory AdditionalGoals;
        public GameEventsCategory EventsToLose;
    }

    [Serializable]
    public class GameEventsCategory
    {
        public List<GameEventGoal> Events;
        public List<LevelRewardDescriptor> Rewards;
    }

    [Serializable]
    public class GameEventGoal
    {
        public GameEventType GameEventType;
        public int Amount;
    }

    [Serializable]
    public class LevelRewardDescriptor
    {
        public LevelRewardType LevelRewardType;
        public int Amount;
    }
}