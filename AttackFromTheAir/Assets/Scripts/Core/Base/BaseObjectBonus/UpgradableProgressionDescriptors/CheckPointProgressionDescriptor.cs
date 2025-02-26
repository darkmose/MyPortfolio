using System;
using System.Collections.Generic;

namespace Core.LobbyBase
{
    [Serializable]
    public class CheckPointProgressionDescriptor : BaseUpgradableProgressionDescriptor
    {
        public float StartValue;
        public List<ProgressionCheckPoint> ProgressionCheckPoints;
    }

    [Serializable]
    public class ProgressionCheckPoint
    {
        public int OnLevel;
        public float Value;
    }
}