using Core.Utilities;

namespace Core.MVP
{
    public interface IInfoPopupPresenter : IPresenter<InfoPopupProxyView, InfoPopupView>
    {
        void SetInfo(string info);
        SimpleEvent CloseEvent { get; }
    }

    public class InfoPopupPresenter : IInfoPopupPresenter
    {
        public InfoPopupProxyView ProxyView { get; }
        public SimpleEvent CloseEvent { get; } = new SimpleEvent();

        public InfoPopupPresenter(InfoPopupProxyView proxyView)
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

        public void Hide()
        {
            if (ProxyView.IsPrepared)
            {
                ProxyView.View.Hide();
            }
        }

        public void Init()
        {
            if (!ProxyView.IsPrepared)
            {
                ProxyView.Prepare();
                ProxyView.View.InitPresenter(this);
            }
        }

        public void Show()
        {
            if (ProxyView.IsPrepared)
            {
                ProxyView.View.Show();
            }
        }

        public void SetInfo(string info)
        {
            if (ProxyView.IsPrepared)
            {
                ProxyView.View.SetText(info);
            }
        }

        public void OnCloseButtonClick()
        {
            CloseEvent.Notify();
            Hide();
        }
    }

}