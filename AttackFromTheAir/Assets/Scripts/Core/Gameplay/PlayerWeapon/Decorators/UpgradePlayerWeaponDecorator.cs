namespace Core.GameLogic
{
    public class UpgradePlayerWeaponDecorator : BasePlayerWeaponDecorator
    {
        private PlayerWeaponConfig _additionalStats;

        public UpgradePlayerWeaponDecorator(IPlayerWeaponDecorator decorator) : base(decorator)
        {
            _additionalStats = new PlayerWeaponConfig();
        }

        protected override PlayerWeaponConfig GetWeaponConfigInner()
        {
            var config = PlayerWeaponDecorator.GetWeaponConfig();
            config += _additionalStats;
            return config;
        }

        public void SetStats(PlayerWeaponConfig stats)
        {
            _additionalStats = stats;
            RaiseDataChangedEvent();
        }
    }
}