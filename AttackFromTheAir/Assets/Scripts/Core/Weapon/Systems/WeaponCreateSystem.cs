using Core.Resourses;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Weapon
{
    public class WeaponCreateSystem
    {
        private WeaponsHolder _weaponsHolder;
        private WeaponConfigsHolder _weaponConfigsHolder;
        private Dictionary<IWeapon, BaseWeaponView> _weaponViewsDict;

        public WeaponCreateSystem()
        {
            _weaponsHolder = Resources.Load<WeaponsHolder>("ScriptableObjects/WeaponsHolder");
            _weaponConfigsHolder = Resources.Load<WeaponConfigsHolder>("ScriptableObjects/WeaponConfigsHolder");
            _weaponViewsDict = new Dictionary<IWeapon, BaseWeaponView>();
        }

        public void DestroyWeapon(IWeapon weapon)
        {
            if (_weaponViewsDict.TryGetValue(weapon, out var view)) 
            {
                weapon.FireEvent.RemoveListener(view.OnFire);
                weapon.FireStopEvent.RemoveListener(view.OnStopFire);

                if (view != null)
                {
                    Object.Destroy(view.gameObject);
                }
            }
        }

        public IBulletsWeapon GetBulletsWeapon(BulletsWeaponType bulletsWeaponType)
        {
            switch (bulletsWeaponType)
            {
                case BulletsWeaponType.Rifle:
                    var weaponConfig = _weaponConfigsHolder.GetBulletsWeapon(bulletsWeaponType);
                    var weapon = new Rifle(weaponConfig);
                    var weaponViewPrefab = _weaponsHolder.GetBulletsWeapon(bulletsWeaponType);
                    var weaponView = Object.Instantiate(weaponViewPrefab);
                    weaponView.InitDamage(weaponConfig.Damage);
                    _weaponViewsDict.Add(weapon, weaponView);

                    weapon.WeaponView = weaponView; 
                    weapon.FireEvent.AddListener(weaponView.OnFire);
                    weapon.FireStopEvent.AddListener(weaponView.OnStopFire);
                    return weapon;
                default:
                    throw default;
            }
        }

        public IExploProjectileWeapon GetExploProjectileWeapon(ExploProjectileWeaponType exploProjectileWeapon)
        {
            switch (exploProjectileWeapon)
            {
                case ExploProjectileWeaponType.TankTurret:
                    var weaponConfig = _weaponConfigsHolder.GetExploProjectileWeapon(exploProjectileWeapon);
                    var weapon = new TankTurret(weaponConfig);
                    var weaponViewPrefab = _weaponsHolder.GetExploProjectileWeapon(exploProjectileWeapon);
                    var weaponView = Object.Instantiate(weaponViewPrefab);
                    weaponView.InitDamage(weaponConfig.Damage);
                    _weaponViewsDict.Add(weapon, weaponView);

                    weapon.WeaponView = weaponView;
                    weapon.FireEvent.AddListener(weaponView.OnFire);
                    weapon.FireStopEvent.AddListener(weaponView.OnStopFire);
                    return weapon;

                case ExploProjectileWeaponType.RocketTurret:
                    var weaponConfig2 = _weaponConfigsHolder.GetExploProjectileWeapon(exploProjectileWeapon);
                    var weapon2 = new TankTurret(weaponConfig2);
                    var weaponViewPrefab2 = _weaponsHolder.GetExploProjectileWeapon(exploProjectileWeapon);
                    var weaponView2 = Object.Instantiate(weaponViewPrefab2);
                    weaponView2.InitDamage(weaponConfig2.Damage);
                    _weaponViewsDict.Add(weapon2, weaponView2);

                    weapon2.WeaponView = weaponView2;
                    weapon2.FireEvent.AddListener(weaponView2.OnFire);
                    weapon2.FireStopEvent.AddListener(weaponView2.OnStopFire);
                    return weapon2;

                default:
                    throw default;
            }
        }


        //TODO : ADD WEAPONS
    }
}