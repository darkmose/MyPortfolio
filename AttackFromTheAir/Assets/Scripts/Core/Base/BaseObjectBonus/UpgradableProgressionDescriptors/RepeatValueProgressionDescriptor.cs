using System;
using System.Collections.Generic;

namespace Core.LobbyBase
{
    [Serializable]
    public class RepeatValueProgressionDescriptor : BaseUpgradableProgressionDescriptor
    {
        public float StartValue;
        public List<ProgressionRepeatDescriptor> ProgressionRepeatDescriptors;
        public float ProgressionStepValue;
    }

    [Serializable]
    public class ProgressionRepeatDescriptor
    {
        public int StepsAmount;
        public float Value;
    }
}