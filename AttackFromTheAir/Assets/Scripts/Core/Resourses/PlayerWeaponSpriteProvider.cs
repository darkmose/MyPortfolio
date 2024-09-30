using Core.GameLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Resourses
{
    [CreateAssetMenu(fileName = "WeaponSpriteProvider", menuName = "ScriptableObjects/WeaponSpriteProvider")]
    public class PlayerWeaponSpriteProvider : ScriptableObject
    {
        [SerializeField] private List<PlayerWeaponIconDescriptor> WeaponConfigDescriptors;
        [SerializeField] private List<PlayerExtraWeaponIconDescriptor> ExtraWeaponConfigDescriptors;

        public Sprite ProvideByType(PlayerWeaponType weaponType)
        {
            var config = WeaponConfigDescriptors.Find(descr=>descr.PlayerWeaponType == weaponType);
            if (config != null)
            {
                return config.Icon;
            }
            else
            {
                throw new System.Exception($"Could not find Weapon Icon of weapon type {weaponType}");
            }
        }

        public Sprite ProvideByType(PlayerExtraWeaponType weaponType)
        {
            var config = ExtraWeaponConfigDescriptors.Find(descr=>descr.PlayerWeaponType == weaponType);
            if (config != null)
            {
                return config.Icon;
            }
            else
            {
                throw new System.Exception($"Could not find Extra Weapon Icon of weapon type {weaponType}");
            }
        }
    }


    [System.Serializable]
    public class PlayerWeaponIconDescriptor
    {
        public PlayerWeaponType PlayerWeaponType;
        public Sprite Icon;
    }

    [System.Serializable]
    public class PlayerExtraWeaponIconDescriptor
    {
        public PlayerExtraWeaponType PlayerWeaponType;
        public Sprite Icon;
    }
}