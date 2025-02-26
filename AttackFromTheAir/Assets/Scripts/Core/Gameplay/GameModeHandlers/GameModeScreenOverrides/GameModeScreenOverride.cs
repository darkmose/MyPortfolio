using Core.Level;
using UnityEngine;

namespace Core.GameLogic
{
    public abstract class GameModeScreenOverride : MonoBehaviour
    {
        public abstract GameMode GameMode { get; }
    }
}