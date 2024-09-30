namespace Core.MVP
{
    public class LoseScreenProxyView : BaseProxyView<LoseScreenView>
    {
        public LoseScreenProxyView(IUIManager uIManager) : base(uIManager)
        {
        }

        public override void Prepare()
        {
            View = _uIManager.GetScreen<LoseScreenView>(ScreenType.Lose);
        }
    }
}