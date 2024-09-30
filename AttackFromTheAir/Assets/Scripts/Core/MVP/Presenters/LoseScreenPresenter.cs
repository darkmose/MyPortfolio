using Core.UI;
using Core.Utilities;

namespace Core.MVP
{
    public interface ILoseScreenPresenter : IPresenter<LoseScreenProxyView, LoseScreenView>
    {
        SimpleEvent RetryButtonClickEvent { get; }
    }

    public class LoseScreenPresenter : BasePresenter, ILoseScreenPresenter
    {
        private SimpleEvent _retryButtonClickEvent = new SimpleEvent();
        public LoseScreenProxyView ProxyView { get; }
        public override bool IsOpen { get; }
        public SimpleEvent RetryButtonClickEvent => _retryButtonClickEvent;

        public LoseScreenPresenter(LoseScreenProxyView proxyView)
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

        public void OnRetryButtonClick()
        {
            _retryButtonClickEvent.Notify();
        }
    }
}