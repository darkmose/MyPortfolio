using Core.Resourses;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameLogic
{
    [CreateAssetMenu(fileName =nameof(PlayerWeaponSelectorConfiguration), menuName = "ScriptableObjects/"+nameof(PlayerWeaponSelectorConfiguration))]
    public class PlayerWeaponSelectorConfiguration : ScriptableObject
    {
        [SerializeField] private List<DroneAvailableWeaponsDescriptor> _droneAvailableWeaponsDescriptors;
        [SerializeField] private List<PlayerDroneStartWeaponDescriptor> _dronesStartWeaponDescriptors;
        private Dictionary<PlayerDroneType, DroneAvailableWeaponsDescriptor> _availableWeaponsDict;
        private Dictionary<PlayerDroneType, PlayerDroneStartWeaponDescriptor> _dronesStartWeaponDict;
        public int LevelToUnlockSecondWeapon = 10;
        public int LevelToUnlockExtraWeapon = 10;

        private void PrepareAvailableWeaponsDict()
        {
            if (_availableWeaponsDict == null)
            {
                _availableWeaponsDict = new Dictionary<PlayerDroneType, DroneAvailableWeaponsDescriptor>();
                foreach (var descr in _droneAvailableWeaponsDescriptors)
                {
                    _availableWeaponsDict.Add(descr.DroneType, descr);
                }
            }
        }

        private void PrepareStartWeaponsDict()
        {
            if (_dronesStartWeaponDict == null)
            {
                _dronesStartWeaponDict = new Dictionary<PlayerDroneType, PlayerDroneStartWeaponDescriptor>();
                foreach (var descr in _dronesStartWeaponDescriptors)
                {
                    _dronesStartWeaponDict.Add(descr.PlayerDroneType, descr);
                }
            }
        }

        public DroneAvailableWeaponsDescriptor GetAvailableWeaponsDescriptor(PlayerDroneType droneType)
        {
            PrepareAvailableWeaponsDict();
            if (_availableWeaponsDict.TryGetValue(droneType, out var descriptor))
            {
                return descriptor;
            }
            else
            {
                throw new System.Exception($"Could not find available weapons for drone {droneType}");
            }
        }

        public PlayerDroneStartWeaponDescriptor ProvideDroneStartWeapon(PlayerDroneType playerDroneType)
        {
            PrepareStartWeaponsDict();
            if (_dronesStartWeaponDict.TryGetValue(playerDroneType, out var weapon))
            {
                return weapon;
            }
            else
            {
                throw new System.Exception($"Could not find start weapon for drone {playerDroneType}");
            }
        }        
    }

    [System.Serializable]
    public class DroneAvailableWeaponsDescriptor
    {
        public PlayerDroneType DroneType;
        public List<PlayerWeaponType> AvailableWeapons;
        public List<PlayerExtraWeaponType> AvailableExtraWeapons;
    }

    [System.Serializable]
    public class PlayerDroneStartWeaponDescriptor
    {
        public PlayerDroneType PlayerDroneType;
        public PlayerWeaponType PlayerWeaponType;
        public PlayerWeaponType PlayerSecondWeaponType;
    }
}