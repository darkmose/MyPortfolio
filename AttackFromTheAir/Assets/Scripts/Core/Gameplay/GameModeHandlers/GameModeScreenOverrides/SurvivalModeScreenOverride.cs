using Core.Level;

namespace Core.GameLogic
{
    public abstract class SurvivalModeScreenOverride : GameModeScreenOverride
    {
        public override GameMode GameMode => GameMode.Survival;
    }
}