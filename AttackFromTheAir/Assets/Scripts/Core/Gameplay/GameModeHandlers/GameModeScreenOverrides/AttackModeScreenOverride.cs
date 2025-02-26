using Core.Level;

namespace Core.GameLogic
{
    public abstract class AttackModeScreenOverride : GameModeScreenOverride
    {
        public override GameMode GameMode => GameMode.Attack; 
        public abstract AttackGameMode AttackGameMode { get; }
    }
}