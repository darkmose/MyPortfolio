namespace Core.MVP
{
    public class LoadingScreenProxyView : BaseProxyView<LoadingScreenView>
    {
        public LoadingScreenProxyView(IUIManager uIManager) : base(uIManager)
        {
        }

        public override void Prepare()
        {
            View = _uIManager.GetScreen<LoadingScreenView>(ScreenType.Loading);
        }
    }
}