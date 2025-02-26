using System.Collections.Generic;
using UnityEngine;

namespace Core.GameLogic
{
    [CreateAssetMenu(fileName =nameof(DroneCustomizeConfiguration), menuName = "ScriptableObjects/"+nameof(DroneCustomizeConfiguration))]
    public class DroneCustomizeConfiguration : ScriptableObject
    {
        [SerializeField] private List<DroneCustomizeDetailDescriptor> _droneCustomizeDetailDescriptors;
        [SerializeField] private List<DroneCustomizeDetailLayoutConfiguration> _droneCustomizeDetailLayoutConfigurations;
        private Dictionary<PlayerWeaponType, GameObject> _weaponDetailsDict;
        private Dictionary<PlayerWeaponType, DroneCustomizeDetailLayoutConfiguration> _detailLayoutsDict;

        private void Prepare()
        {
            if (_weaponDetailsDict == null)
            {
                _weaponDetailsDict = new Dictionary<PlayerWeaponType, GameObject>();
                foreach (var item in _droneCustomizeDetailDescriptors)
                {
                    _weaponDetailsDict.Add(item.WeaponType, item.DetailPrefab);
                }
            }
            if (_detailLayoutsDict == null)
            {
                _detailLayoutsDict = new Dictionary<PlayerWeaponType, DroneCustomizeDetailLayoutConfiguration>();
                foreach (var item in _droneCustomizeDetailLayoutConfigurations)
                {
                    _detailLayoutsDict.Add(item.WeaponType, item);
                }
            }
        }

        public GameObject ProvideDetail(PlayerWeaponType weaponType)
        {
            Prepare();
            if (_weaponDetailsDict.TryGetValue(weaponType, out var detail))
            {
                return detail;
            }
            else
            {
                throw new System.Exception($"Could not find detail for customize weapon {weaponType}");
            }
        }

        public DroneCustomizeDetailLayoutConfiguration ProvideLayoutConfiguration(PlayerWeaponType weaponType)
        {
            Prepare();
            if (_detailLayoutsDict.TryGetValue(weaponType, out var layoutConfiguration))
            {
                return layoutConfiguration;
            }
            else
            {
                throw new System.Exception($"Could not find detail for customize weapon {weaponType}");
            }
        }
    }

    [System.Serializable]
    public class DroneCustomizeDetailDescriptor
    {
        public PlayerWeaponType WeaponType;
        public GameObject DetailPrefab;
    }

    [System.Serializable]
    public class DroneCustomizeDetailLayoutConfiguration
    {
        public PlayerWeaponType WeaponType;
        public List<CustomizeDetailLayout> AvailableLayouts;
    }

    public enum CustomizeDetailLayout
    {
        Left, Right, Bottom, TopLeft, TopRight, BottomLeft, BottomRight, Top
    }
}