namespace Core.MVP
{
    public class WinScreenProxyView : BaseProxyView<WinScreenView>
    {
        public WinScreenProxyView(IUIManager uIManager) : base(uIManager)
        {
        }

        public override void Prepare()
        {
            View = _uIManager.GetScreen<WinScreenView>(ScreenType.Win);
        }
    }
}