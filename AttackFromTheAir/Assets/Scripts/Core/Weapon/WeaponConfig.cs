namespace Core.Weapon
{
    [System.Serializable]
    public struct WeaponConfig
    {
        public float Firerate;
        public int ShotsBeforeCooldown;
        public float CooldownDuration;
        public float FireDistance;
        public int Damage;

        public static WeaponConfig operator+(WeaponConfig a, WeaponConfig b)
        {
            var conf = a;
            conf.Firerate += b.Firerate;
            conf.CooldownDuration += b.CooldownDuration;
            conf.FireDistance += b.FireDistance; 
            conf.Damage += b.Damage;
            return conf;
        }
    }
}