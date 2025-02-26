using Core.Level;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Resourses
{
    [CreateAssetMenu(fileName =nameof(GamemodeSpriteProvider), menuName = "ScriptableObjects/"+nameof(GamemodeSpriteProvider))]
    public class GamemodeSpriteProvider : SerializedScriptableObject
    {
        [SerializeField] private List<GamemodeSpriteDescriptor> _gamemodeSpriteDescriptors;
        [SerializeField] private Sprite _lockedLevelGamemodeIcon;
        private Dictionary<AttackGameMode, Sprite> _attackGamemodeSprites;
        private Dictionary<DefenceGameMode, Sprite> _defenceGamemodeSprites;
        private Dictionary<SurvivalGameMode, Sprite> _survivalGamemodeSprites;
        public Sprite LockedLevelGamemodeIcon => _lockedLevelGamemodeIcon;

        private void Prepare()
        {
            bool attackGamemodeSpritesEmpty = false;
            bool defenceGamemodeSpritesEmpty = false;
            bool survivalGamemodeSpritesEmpty = false;


            if (_attackGamemodeSprites == null)
            {
                _attackGamemodeSprites = new Dictionary<AttackGameMode, Sprite>();
                attackGamemodeSpritesEmpty = true;
            }
            if (_defenceGamemodeSprites == null)
            {
                _defenceGamemodeSprites = new Dictionary<DefenceGameMode, Sprite>();
                defenceGamemodeSpritesEmpty = true;
            }
            if (_survivalGamemodeSprites == null)
            {
                _survivalGamemodeSprites = new Dictionary<SurvivalGameMode, Sprite>();
                survivalGamemodeSpritesEmpty = true;
            }

            if (!attackGamemodeSpritesEmpty && !defenceGamemodeSpritesEmpty && !survivalGamemodeSpritesEmpty)
            {
                return;
            }

            foreach (var descr in _gamemodeSpriteDescriptors)
            {
                var gamemode = descr.GameMode;
                var sprite = descr.Sprite;

                if (gamemode == GameMode.Attack && attackGamemodeSpritesEmpty)
                {
                    var attackGamemodeDescr = descr as AttackGamemodeSpriteDescriptor;
                    _attackGamemodeSprites.Add(attackGamemodeDescr.AttackGameMode, sprite);
                }
                if (gamemode == GameMode.Defence && defenceGamemodeSpritesEmpty)
                {
                    var defenceGamemodeDescr = descr as DefenceGamemodeSpriteDescriptor;
                    _defenceGamemodeSprites.Add(defenceGamemodeDescr.DefenceGameMode, sprite);
                }
                if (gamemode == GameMode.Survival && survivalGamemodeSpritesEmpty)
                {
                    var survivalGamemodeDescr = descr as SurvivalGamemodeSpriteDescriptor;
                    _survivalGamemodeSprites.Add(survivalGamemodeDescr.SurvivalGameMode, sprite);
                }
            }
        }

        public Sprite ProvideAttackGamemodeSprite(AttackGameMode attackGameMode)
        {
            Prepare();
            if (_attackGamemodeSprites.TryGetValue(attackGameMode, out var sprite))
            {
                return sprite;
            }
            else
            {
                throw default;
            }
        }

        public Sprite ProvideDefenceGamemodeSprite(DefenceGameMode defenceGamemode)
        {
            Prepare();
            if (_defenceGamemodeSprites.TryGetValue(defenceGamemode, out var sprite))
            {
                return sprite;
            }
            else
            {
                throw default;
            }
        }

        public Sprite ProvideSurvivalGamemodeSprite(SurvivalGameMode survivalGamemode)
        {
            Prepare();
            if (_survivalGamemodeSprites.TryGetValue(survivalGamemode, out var sprite))
            {
                return sprite;
            }
            else
            {
                throw default;
            }
        }
    }

    [ShowOdinSerializedPropertiesInInspector]
    public abstract class GamemodeSpriteDescriptor
    {
        public abstract GameMode GameMode { get; }
        public Sprite Sprite;
    }

    [System.Serializable]
    public class AttackGamemodeSpriteDescriptor : GamemodeSpriteDescriptor
    {
        public override GameMode GameMode => GameMode.Attack;
        public AttackGameMode AttackGameMode;
    }

    [System.Serializable]
    public class DefenceGamemodeSpriteDescriptor : GamemodeSpriteDescriptor
    {
        public override GameMode GameMode => GameMode.Defence;
        public DefenceGameMode DefenceGameMode;
    }

    [System.Serializable]
    public class SurvivalGamemodeSpriteDescriptor : GamemodeSpriteDescriptor
    {
        public override GameMode GameMode => GameMode.Survival;
        public SurvivalGameMode SurvivalGameMode;
    }
}


