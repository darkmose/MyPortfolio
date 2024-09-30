using Core.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Tools
{
    [CreateAssetMenu(fileName = "TriggerToUnitStateConfiguration", menuName = "ScriptableObjects/TriggerConfigurations/TriggerToUnitStateConfiguration")]
    public class TriggerToUnitStateConfiguration : ScriptableObject
    {
        [SerializeField] private List<TriggerToUnitStateDescriptor> _triggerToStateDescriptors;
        
        public List<UnitStates> GetUnitStatesByTrigger(GameTriggerType gameTriggerType, int triggerIndex)
        {
            var descr = _triggerToStateDescriptors.Find(pred => pred.TriggerType == gameTriggerType && pred.TriggerIndex == triggerIndex);
            if (descr != null)
            {
                return descr.UnitStates;
            }
            else 
            {
                return null;
            }
        }
    }

    [System.Serializable]
    public class TriggerToUnitStateDescriptor
    {
        public GameTriggerType TriggerType;
        public int TriggerIndex;
        [Tooltip("States will be switched sequentially")]
        public List<UnitStates> UnitStates;
    }
}