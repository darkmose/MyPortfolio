using Core.UI;
using Core.Utilities;

namespace Core.MVP
{
    public interface IWinScreenPresenter : IPresenter<WinScreenProxyView, WinScreenView>
    {
        SimpleEvent NextLevelButtonClickEvent { get; }
    }

    public class WinScreenPresenter : BasePresenter, IWinScreenPresenter
    {
        private SimpleEvent _nextLevelButtonClickEvent = new SimpleEvent();
        public WinScreenProxyView ProxyView { get; }
        public override bool IsOpen { get; }
        public SimpleEvent NextLevelButtonClickEvent => _nextLevelButtonClickEvent;

        public WinScreenPresenter(WinScreenProxyView proxyView)
        {
            ProxyView = proxyView;
        }

        public void Destroy()
        {
            if (ProxyView.IsPrepared)
            {
                ProxyView.View.Destroy();
            }
        }

        public override void Hide()
        {
            if (ProxyView.IsPrepared)
            {
                ProxyView.View.Hide();
            }
        }

        public override void Init()
        {
            if (!ProxyView.IsPrepared)
            {
                ProxyView.Prepare();
                ProxyView.View.InitPresenter(this);
            }
        }

        public override void Show()
        {
            if (ProxyView.IsPrepared)
            {
                ProxyView.View.Show();
            }
        }

        public void OnNextLevelButtonClick()
        {
            _nextLevelButtonClickEvent.Notify();
        }
    }
}