using System;

namespace Core.LobbyBase
{
    [Serializable]
    public class SimpleUpgradableProgressionDescriptor : BaseUpgradableProgressionDescriptor
    {
        public float StartValue;
        public float ProgressionStepValue;
    }
}