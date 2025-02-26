using Core.Level;

namespace Core.GameLogic
{
    public abstract class SurvivalGameModeHelper : GameModeHelper
    {
        public override GameMode GameMode => GameMode.Survival;
        public abstract SurvivalGameMode SurvivalGameMode { get; }
    }
}