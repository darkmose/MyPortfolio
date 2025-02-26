using Core.Level;
using Zenject;

namespace Core.GameLogic
{
    public abstract class GameModeHelper
    {
        public abstract GameMode GameMode { get; }

        public virtual void InitDI(DiContainer container)
        {
        }

        public abstract void Start();
        public abstract void Stop();
    }
}