namespace Core.MVP
{
    public class SettingsWindowProxyView : BaseProxyView<SettingsWindowView>
    {
        public SettingsWindowProxyView(IUIManager uIManager) : base(uIManager)
        {
        }

        public override void Prepare()
        {
            View = _uIManager.GetWindow<SettingsWindowView>(WindowType.Settings);
        }
    }
}