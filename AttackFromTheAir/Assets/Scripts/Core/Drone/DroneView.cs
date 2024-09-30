using Core.Resourses;
using UnityEngine;

namespace Core.GameLogic
{
    public class DroneView : MonoBehaviour
    {
        [SerializeField] private PlayerWeaponsHolder _playerWeaponsHolder;

        public void InitWeapon(PlayerWeaponType type)
        {
            //_playerWeaponsHolder.GetWeapon(type);
        }
    }
}