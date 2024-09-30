using Configuration;
using System;

namespace Core.GameLogic
{
    [Serializable]
    public struct PlayerWeaponConfig
    {
        public float ReloadSpeed;
        public float Speed;
        public int Damage;
        public int ProjectileCount;

        public bool IsContinuousFire;
        public int ContinuousFirerate;
        public int ContinuousFiresBeforeReload;

        public static PlayerWeaponConfig operator +(PlayerWeaponConfig a, PlayerWeaponConfig b)
        {
            PlayerWeaponConfig c = new PlayerWeaponConfig();
            c.ReloadSpeed = a.ReloadSpeed + b.ReloadSpeed;
            c.Damage = a.Damage + b.Damage;
            c.Speed = a.Speed + b.Speed;
            c.ProjectileCount = a.ProjectileCount + b.ProjectileCount;
            c.ContinuousFirerate = a.ContinuousFirerate + b.ContinuousFirerate;
            c.ContinuousFiresBeforeReload = a.ContinuousFiresBeforeReload + b.ContinuousFiresBeforeReload; 
            c.IsContinuousFire = a.IsContinuousFire || b.IsContinuousFire;
            return c;
        }
    }
}