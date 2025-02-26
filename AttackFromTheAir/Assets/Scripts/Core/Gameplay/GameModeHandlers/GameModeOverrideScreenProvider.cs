using Core.Level;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameLogic
{
    [CreateAssetMenu(fileName =nameof(GameModeOverrideScreenProvider), menuName ="ScriptableObjects/"+nameof(GameModeOverrideScreenProvider))]
    public class GameModeOverrideScreenProvider : SerializedScriptableObject
    {
        [SerializeField] private List<AttackScreenOverrideDescriptor> _attackScreenOverrideDescriptors;
        [SerializeField] private List<DefenceScreenOverrideDescriptor> _defenceScreenOverrideDescriptors;
        [SerializeField] private List<SurvivalScreenOverrideDescriptor> _survivalScreenOverrideDescriptors;
        private Dictionary<AttackGameMode, GameModeScreenOverride> _attackModeScreenOverrides;
        private Dictionary<DefenceGameMode, GameModeScreenOverride> _defenceModeScreenOverrides;
        private Dictionary<SurvivalGameMode, GameModeScreenOverride> _survivalModeScreenOverrides;

        private void PrepareAttackModeScreenOverrides()
        {
            if (_attackModeScreenOverrides == null)
            {
                _attackModeScreenOverrides = new Dictionary<AttackGameMode, GameModeScreenOverride>();
                foreach (var item in _attackScreenOverrideDescriptors)
                {
                    _attackModeScreenOverrides.Add(item.AttackGameMode, item.GameModeScreenOverride);
                }
            }
        }
        private void PrepareDefenceModeScreenOverrides()
        {
            if (_defenceModeScreenOverrides == null)
            {
                _defenceModeScreenOverrides = new Dictionary<DefenceGameMode, GameModeScreenOverride>();
                foreach (var item in _defenceScreenOverrideDescriptors)
                {
                    _defenceModeScreenOverrides.Add(item.DefenceGameMode, item.GameModeScreenOverride);
                }
            }
        }
        private void PrepareSurvivalModeScreenOverrides()
        {
            if (_survivalModeScreenOverrides == null)
            {
                _survivalModeScreenOverrides = new Dictionary<SurvivalGameMode, GameModeScreenOverride>();
                foreach (var item in _survivalScreenOverrideDescriptors)
                {
                    _survivalModeScreenOverrides.Add(item.SurvivalGameMode, item.GameModeScreenOverride);
                }
            }
        }


        public GameModeScreenOverride ProvideAttackModeScreenOverride(AttackGameMode gameMode)
        {
            PrepareAttackModeScreenOverrides();
            if (_attackModeScreenOverrides.TryGetValue(gameMode,  out var gameModeScreenOverride))
            {
                return gameModeScreenOverride;
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }
        

        public GameModeScreenOverride ProvideDefenceModeScreenOverride(DefenceGameMode gameMode)
        {
            PrepareDefenceModeScreenOverrides();
            if (_defenceModeScreenOverrides.TryGetValue(gameMode,  out var gameModeScreenOverride))
            {
                return gameModeScreenOverride;
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }
        

        public GameModeScreenOverride ProvideSurvivalModeScreenOverride(SurvivalGameMode gameMode)
        {
            PrepareSurvivalModeScreenOverrides();
            if (_survivalModeScreenOverrides.TryGetValue(gameMode,  out var gameModeScreenOverride))
            {
                return gameModeScreenOverride;
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }        
    }

    [ShowOdinSerializedPropertiesInInspector]
    public class AttackScreenOverrideDescriptor
    {
        public AttackGameMode AttackGameMode;
        public GameModeScreenOverride GameModeScreenOverride;
    }

    [ShowOdinSerializedPropertiesInInspector]
    public class DefenceScreenOverrideDescriptor
    {
        public DefenceGameMode DefenceGameMode;
        public GameModeScreenOverride GameModeScreenOverride;
    }
    
    [ShowOdinSerializedPropertiesInInspector]
    public class SurvivalScreenOverrideDescriptor
    {
        public SurvivalGameMode SurvivalGameMode;
        public GameModeScreenOverride GameModeScreenOverride;
    }

    
}