namespace Core.MVP
{
    public interface ISettingsWindowPresenter : IPresenter<SettingsWindowModel, SettingsWindowProxyView, SettingsWindowView>
    {
        void SetActiveGoLobbyButton(bool isActive);
    }

    public class SettingsWindowPresenter : ISettingsWindowPresenter
    {
        public SettingsWindowModel Model { get; }
        public SettingsWindowUseCases UseCases { get; }
        public SettingsWindowProxyView ProxyView { get; }
        private bool _isGoLobbyButtonActive;

        public SettingsWindowPresenter(SettingsWindowModel model, SettingsWindowUseCases useCases, SettingsWindowProxyView proxyView)
        {
            Model = model;
            UseCases = useCases;
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
                ProxyView.View.SetActiveGoToLobbyButton(_isGoLobbyButtonActive);  
            }
        }

        public void Show()
        {
            if (ProxyView.IsPrepared)
            {
                ProxyView.View.Show();
            }
        }

        public void OnGoToLobbyButtonClick()
        {
            UseCases.OnGoToLobbyButtonClick();
            Hide();
        }

        public void SetActiveGoLobbyButton(bool isActive)
        {
            if (ProxyView.IsPrepared)
            {
                ProxyView.View.SetActiveGoToLobbyButton(isActive);
            }
            else
            {
                _isGoLobbyButtonActive = isActive;
            }
        }
    }
}