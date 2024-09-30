using Core.GameLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Resourses
{
    [CreateAssetMenu(fileName = "PlayerWeaponsHolder", menuName = "ScriptableObjects/PlayerWeaponsHolder")]
    public class PlayerWeaponsHolder : ScriptableObject
    {
        [SerializeField] private List<PlayerWeaponDescriptor> WeaponDescriptors;
        [SerializeField] private List<PlayerExtraWeaponDescriptor> ExtraWeaponDescriptors;

        public PlayerWeapon GetWeapon(PlayerWeaponType weaponType)
        {
            var weapon = WeaponDescriptors.Find(descr=>descr.WeaponType == weaponType);
            if (weapon != null)
            {
                return weapon.Prefab;
            }
            else
            {
                throw new System.Exception($"Could not find Weapon of type {weaponType}");
            }
        }

        public PlayerExtraWeapon GetExtraWeapon(PlayerExtraWeaponType weaponType)
        {
            var weapon = ExtraWeaponDescriptors.Find(descr=>descr.WeaponType == weaponType);
            if (weapon != null)
            {
                return weapon.Prefab;
            }
            else
            {
                throw new System.Exception($"Could not find Extra Weapon of type {weaponType}");
            }
        }
    }


    [System.Serializable]
    public class PlayerWeaponDescriptor
    {
        public PlayerWeaponType WeaponType;
        public PlayerWeapon Prefab;
    }

    [System.Serializable]
    public class PlayerExtraWeaponDescriptor
    {
        public PlayerExtraWeaponType WeaponType;
        public PlayerExtraWeapon Prefab;
    }
}