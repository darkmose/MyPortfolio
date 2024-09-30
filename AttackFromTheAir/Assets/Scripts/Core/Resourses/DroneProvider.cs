using Core.GameLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Resourses
{
    public enum PlayerDroneType
    {
        SmallDrone
    }

    [CreateAssetMenu(fileName = "DroneProvider", menuName = "ScriptableObjects/DroneProvider")]
    public class DroneProvider : ScriptableObject
    {
        [SerializeField] private List<DroneDescriptor> _drones;

        public DroneDescriptor ProvideByType(PlayerDroneType playerDroneType)
        {
            var drone = _drones.Find(descr=>descr.PlayerDroneType == playerDroneType);

            if (drone == null) 
            {
                throw new System.Exception($"Could not find Drone of type {playerDroneType}");
            }
            else
            {
                return drone;
            }
        }
    }

    [System.Serializable]
    public class DroneDescriptor
    {
        public PlayerDroneType PlayerDroneType;
        public PlayerWeaponType MainWeapon;
        public PlayerWeaponType SecondWeapon;
        public DroneCameraConfiguration DroneCameraConfiguration;
        public DroneView DroneViewPrefab;
    }

    [System.Serializable]
    public class DroneCameraConfiguration
    {
        public Vector3 CameraInitLocalRotation;
        public float TangageAllowedAngle;
        public float YawAllowedAngle;
    }
}