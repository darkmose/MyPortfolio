using Core.Level;
using System.Collections.Generic;
using Zenject;

namespace Core.GameLogic
{
    public class GameModeHelpersFactory
    {
        private static GameModeHelpersFactory instance;
        private static DiContainer _diContainer;
        private static GameModeHelper _lastHelper;
        private Dictionary<AttackGameMode, GameModeHelper> _attackGameModeHelpers;
        private Dictionary<DefenceGameMode, GameModeHelper> _defenceGameModeHelpers;
        private Dictionary<SurvivalGameMode, GameModeHelper> _survivalGameModeHelpers;
        public static GameModeHelper LastHelper => _lastHelper;

        public GameModeHelpersFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
            instance = this;
            _attackGameModeHelpers = new Dictionary<AttackGameMode, GameModeHelper>();
            _defenceGameModeHelpers = new Dictionary<DefenceGameMode, GameModeHelper>();
            _survivalGameModeHelpers = new Dictionary<SurvivalGameMode, GameModeHelper>();
        }

        public static GameModeHelper ProvideAttackGameModeHelper(AttackGameMode attackGameMode)
        {
            if (instance._attackGameModeHelpers.TryGetValue(attackGameMode, out var helper))
            {
                _lastHelper = helper;
                return helper;
            }
            else
            {
                GameModeHelper gameModeHelper = null;
                switch (attackGameMode)
                {
                    case AttackGameMode.PrisonerRelease:
                        gameModeHelper = new PrisonerReleaseModeHelper();
                        break;
                    default:
                        gameModeHelper = new PrisonerReleaseModeHelper();
                        break;
                }
                _lastHelper = gameModeHelper;
                instance._attackGameModeHelpers.Add(attackGameMode, gameModeHelper);
                gameModeHelper.InitDI(_diContainer);
                return gameModeHelper;
            }
        }
        public static GameModeHelper ProvideDefenceGameModeHelper(DefenceGameMode defenceGameMode)
        {
            if (instance._defenceGameModeHelpers.TryGetValue(defenceGameMode, out var helper))
            {
                _lastHelper = helper;
                return helper;
            }
            else
            {
                GameModeHelper gameModeHelper = null;
                switch (defenceGameMode)
                {
                    default:
                        break;
                }
                _lastHelper = gameModeHelper;
                instance._defenceGameModeHelpers.Add(defenceGameMode, gameModeHelper);
                gameModeHelper.InitDI(_diContainer);
                return gameModeHelper;
            }
        }
        public static GameModeHelper ProvideSurvivalGameModeHelper(SurvivalGameMode survivalGameMode)
        {
            if (instance._survivalGameModeHelpers.TryGetValue(survivalGameMode, out var helper))
            {
                _lastHelper = helper;
                return helper;
            }
            else
            {
                GameModeHelper gameModeHelper = null;
                switch (survivalGameMode)
                {
                    default:
                        break;
                }
                _lastHelper = gameModeHelper;
                instance._survivalGameModeHelpers.Add(survivalGameMode, gameModeHelper);
                gameModeHelper.InitDI(_diContainer);
                return gameModeHelper;
            }
        }
    }
}