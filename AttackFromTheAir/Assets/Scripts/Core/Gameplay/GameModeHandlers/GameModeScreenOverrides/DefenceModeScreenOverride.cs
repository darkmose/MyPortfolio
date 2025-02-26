using Core.Level;

namespace Core.GameLogic
{
    public abstract class DefenceModeScreenOverride : GameModeScreenOverride
    {
        public override GameMode GameMode => GameMode.Defence;
        public abstract DefenceGameMode DefenceGameMode { get; }
    }
}