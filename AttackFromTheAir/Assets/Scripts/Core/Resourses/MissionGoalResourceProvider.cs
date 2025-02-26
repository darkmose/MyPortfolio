using Core.Level;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Resourses
{
    [CreateAssetMenu(fileName =nameof(MissionGoalResourceProvider), menuName ="ScriptableObjects/"+nameof(MissionGoalResourceProvider))]
    public class MissionGoalResourceProvider
    {
        [SerializeField] private List<MissionGoalResourceDescriptor> _missionGoalResources;
        private Dictionary<GameEventType, MissionGoalResourceDescriptor> _resourcesDictionary;

        private void Prepare()
        {
            if (_resourcesDictionary== null)
            {
                _resourcesDictionary = new Dictionary<GameEventType, MissionGoalResourceDescriptor>();
                foreach (var resource in _missionGoalResources)
                {
                    _resourcesDictionary.Add(resource.GoalType, resource);
                }
            }
        }

        public MissionGoalResourceDescriptor ProvideMissionGoalResource(GameEventType gameEventType)
        {
            if (_resourcesDictionary.TryGetValue(gameEventType, out var resourceDescriptor))
            {
                return resourceDescriptor;
            }
            else
            {
                throw new System.Exception($"Could not find resources for game event {gameEventType}");
            }
        }

    }

    [System.Serializable]
    public class MissionGoalResourceDescriptor
    {
        public GameEventType GoalType;
        public Sprite GoalIcon;
        public string StatusFormat;
    }
}