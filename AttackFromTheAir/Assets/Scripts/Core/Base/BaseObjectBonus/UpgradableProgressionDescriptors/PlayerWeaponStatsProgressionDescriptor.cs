using Core.GameLogic;
using Sirenix.OdinInspector;
using System;

namespace Core.LobbyBase
{
    [ShowOdinSerializedPropertiesInInspector]
    public class PlayerWeaponStatsProgressionDescriptor : BaseUpgradableProgressionDescriptor
    {
        public BaseUpgradableProgressionDescriptor DamageProgression;
        public BaseUpgradableProgressionDescriptor ReloadSpeedProgression;
        public BaseUpgradableProgressionDescriptor ProjectileSpeedProgression;
        public BaseUpgradableProgressionDescriptor AmmoProgression;
        public BaseUpgradableProgressionDescriptor FireCountLimitProgression;
        public BaseUpgradableProgressionDescriptor ContinuousFirerateProgression;
        public BaseUpgradableProgressionDescriptor ContinuousFiresBeforeReloadProgression;
    }
}