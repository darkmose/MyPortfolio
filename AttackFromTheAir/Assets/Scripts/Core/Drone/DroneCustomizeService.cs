using Core.Tools;
using Core.UI;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameLogic
{
    public class DroneCustomizeService : MonoBehaviour
    {
        [SerializeField] private List<CustomizationLayoutRoot> _layoutRoots;
        [SerializeField] private DroneCustomizeConfiguration _droneCustomizeConfiguration;

        public void InitWeapons(params PlayerWeaponType[] weaponTypes)
        {
            ClearCustomization();

            weaponTypes.Sort((x, y) =>
            {
                var configX = _droneCustomizeConfiguration.ProvideLayoutConfiguration(x);
                var configY = _droneCustomizeConfiguration.ProvideLayoutConfiguration(y);
                return configX.AvailableLayouts.Count.CompareTo(configY.AvailableLayouts.Count);            
            });

            var layoutsDict = new Dictionary<CustomizeDetailLayout, PlayerWeaponType>();

            foreach (var weaponType in weaponTypes)
            {
                var layouts = _droneCustomizeConfiguration.ProvideLayoutConfiguration(weaponType);

                foreach (var layout in layouts.AvailableLayouts)
                {
                    if (!layoutsDict.ContainsKey(layout))
                    {
                        layoutsDict.Add(layout, weaponType);
                        if (layout.TryGetOppositeSide(layouts, out var oppositeSide))
                        {
                            if (!layoutsDict.ContainsKey(oppositeSide))
                            {
                                layoutsDict.Add(oppositeSide, weaponType);
                            }
                        }
                        break;
                    }
                }
            }

            foreach (var item in layoutsDict)
            {
                var root = _layoutRoots.Find(pred=>pred.CustomizeDetailLayout == item.Key);
                if (root != null)
                {
                    var customizeWeapon = _droneCustomizeConfiguration.ProvideDetail(item.Value);
                    var instance = Instantiate(customizeWeapon, root.Root);
                }
            }
        }

        private void ClearCustomization()
        {
            foreach (var root in _layoutRoots)
            {
                root.Root.ClearAllChild();
            }
        }
    }

    [System.Serializable]
    public class CustomizationLayoutRoot
    {
        public CustomizeDetailLayout CustomizeDetailLayout;
        public Transform Root;
    }
}