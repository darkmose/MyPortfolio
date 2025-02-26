using Core.LobbyBase;
using Core.Resourses;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameLogic
{
    [CreateAssetMenu(fileName =nameof(PlayerDroneUpgradeConfiguration), menuName ="ScriptableObjects/"+nameof(PlayerDroneUpgradeConfiguration))]
    public class PlayerDroneUpgradeConfiguration : SerializedScriptableObject
    {
        [SerializeField] private List<DroneUpgradeDescriptor> _droneUpgradeDescriptors;
        private Dictionary<PlayerDroneType, DroneUpgradeDescriptor> _upgradeDescriptorsDictionary;

        private void Prepare()
        {
            _upgradeDescriptorsDictionary = new Dictionary<PlayerDroneType, DroneUpgradeDescriptor>();
            foreach (var item in _droneUpgradeDescriptors)
            {
                _upgradeDescriptorsDictionary.Add(item.PlayerDroneType, item);
            }
        }

        public DroneUpgradeDescriptor ProvideDroneUpgradeDescriptor(PlayerDroneType playerDroneType)
        {
            Prepare();
            if (_upgradeDescriptorsDictionary.TryGetValue(playerDroneType, out var droneUpgradeDescriptor))
            {
                return droneUpgradeDescriptor;
            }
            else
            {
                throw new System.Exception($"[PlayerDroneUpgradeConfiguration] Could not find upgrade descriptor for player Drone of type {playerDroneType}");
            }
        }
    }

    [ShowOdinSerializedPropertiesInInspector]
    public class DroneUpgradeDescriptor
    {
        public PlayerDroneType PlayerDroneType;
        public BaseUpgradableProgressionDescriptor SpeedProgression;
        public BaseUpgradableProgressionDescriptor ArmorProgression;
        public BaseUpgradableProgressionDescriptor ReloadSpeedProgression;
        public CostProgressionDescriptor SpeedSoftCostProgression;
        public CostProgressionDescriptor SpeedHardCostProgression;
        public CostProgressionDescriptor ReloadSoftCostProgression;
        public CostProgressionDescriptor ReloadHardCostProgression;
        public CostProgressionDescriptor ArmorSoftCostProgression;
        public CostProgressionDescriptor ArmorHardCostProgression;
    }
}