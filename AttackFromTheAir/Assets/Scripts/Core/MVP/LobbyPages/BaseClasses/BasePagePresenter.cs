namespace Core.MVP
{
    public abstract class BasePagePresenter<V> : IPagePresenter
    where V : IPageView
    {
        public V View { get; private set; }
        protected bool IsOpened { get; private set; }

        public void HidePage()
        {
            if (!IsOpened)
            {
                return;
            }
            OnHidePage();
            View.Hide();
            IsOpened = false;
        }

        public void Init(V view)
        {
            View = view;

            view.InitPresenter(this);

            InitInner(view);
        }

        protected virtual void OnShowPage()
        {
        }

        protected virtual void OnHidePage()
        {
        }

        public void ShowPage()
        {
            if (IsOpened)
            {
                return;
            }
            View.Show();
            OnShowPage();
            IsOpened = true;
        }

        protected abstract void InitInner(V view);
    }
}