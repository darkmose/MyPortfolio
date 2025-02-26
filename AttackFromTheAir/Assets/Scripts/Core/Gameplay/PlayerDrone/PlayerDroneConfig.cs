namespace Core.GameLogic
{
    [System.Serializable]
    public struct PlayerDroneConfig
    {
        public float Speed, Armor, Reload;

        public static PlayerDroneConfig operator +(PlayerDroneConfig a, PlayerDroneConfig b)
        {
            PlayerDroneConfig c = new PlayerDroneConfig();
            c.Speed = a.Speed + b.Speed;
            c.Armor = a.Armor+ b.Armor;
            c.Reload = a.Reload + b.Reload;
            return c;
        }
    }
}