using Core.Level;

namespace Core.GameLogic
{
    public abstract class DefenceGameModeHelper : GameModeHelper
    {
        public override GameMode GameMode => GameMode.Defence;
        public abstract DefenceGameMode DefenceGameMode { get; }
    }
}