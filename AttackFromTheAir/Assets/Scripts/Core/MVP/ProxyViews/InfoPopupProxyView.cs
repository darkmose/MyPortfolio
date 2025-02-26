namespace Core.MVP
{
    public class InfoPopupProxyView : BaseProxyView<InfoPopupView>
    {
        public InfoPopupProxyView(IUIManager uIManager) : base(uIManager)
        {
        }

        public override void Prepare()
        {
            View = _uIManager.GetPopup<InfoPopupView>(UI.PopupType.Info);
        }
    }
}