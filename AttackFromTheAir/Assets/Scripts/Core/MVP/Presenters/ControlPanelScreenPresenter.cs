using Core.ControlLogic;

namespace Core.MVP
{
    public interface IControlPanelScreenPresenter : IPresenter<ControlPanelScreenProxyView, ControlPanelScreenView>
    {
        IControlPanel ControlPanel { get; }
    }

    public class ControlPanelScreenPresenter : IControlPanelScreenPresenter
    {
        public ControlPanelScreenProxyView ProxyView { get; }
        public IControlPanel ControlPanel => ProxyView.View.ControlPanel;

        public ControlPanelScreenPresenter(ControlPanelScreenProxyView proxyView)
        {
            ProxyView = proxyView;
        }

        public void Destroy()
        {
            if (ProxyView.IsPrepared)
            {
                ProxyView.View.ControlPanel.Reset();
                ProxyView.View.ControlPanel.Disable();
                ProxyView.View.Destroy();
            }
        }

        public void Hide()
        {
            if (ProxyView.IsPrepared)
            {
                ProxyView.View.Hide();
                ProxyView.View.ControlPanel.Disable();  
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
                ProxyView.View.ControlPanel.Reset();
                ProxyView.View.ControlPanel.Enable();
                ProxyView.View.transform.SetAsFirstSibling();
            }
        }
    }
}
