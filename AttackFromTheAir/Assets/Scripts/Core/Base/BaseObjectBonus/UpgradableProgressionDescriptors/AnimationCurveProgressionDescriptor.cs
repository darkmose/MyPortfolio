using UnityEngine;

namespace Core.LobbyBase
{
    [System.Serializable]
    public class AnimationCurveProgressionDescriptor : BaseUpgradableProgressionDescriptor
    {
        public int MinLevel;
        public int MaxLevel;
        public float MinValue;
        public float MaxValue;
        public AnimationCurve ProgressionCurve;
    }
}