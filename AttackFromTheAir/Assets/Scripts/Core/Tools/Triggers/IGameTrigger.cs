using Core.Utilities;

namespace Core.Tools
{
    public enum GameTriggerType
    {
        PrisonerCageDestroyed
    }

    public interface IGameTrigger
    {
        public int ID { get; }
        GameTriggerType TriggerType { get; }
    }
}