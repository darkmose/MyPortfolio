using Core.Level;

namespace Core.GameLogic
{
    public abstract class AttackGameModeHelper : GameModeHelper
    {
        public override GameMode GameMode => GameMode.Attack;
        public abstract AttackGameMode AttackGameMode { get; }
    }
}