namespace Core.MVP
{
    public class ControlPanelScreenProxyView : BaseProxyView<ControlPanelScreenView>
    {
        public ControlPanelScreenProxyView(IUIManager uIManager) : base(uIManager)
        {
        }

        public override void Prepare()
        {
            View = _uIManager.GetScreen<ControlPanelScreenView>(ScreenType.ControlPanel);
        }
    }
}