using Zenject;

namespace Core.MVP
{
    public abstract class BasePagePresenterModel<V, M> : IPagePresenter
    where V : IPageView
    where M : IPageModel
    {
        private DiContainer _diContainer;

        public V View { get; private set; }
        public M Model { get; private set; }
        protected bool IsOpened { get; private set; }

        protected BasePagePresenterModel(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void Init(V view, M model)
        {
            View = view;
            Model = model;

            view.InitPresenter(this);
            model.Init(_diContainer);

            InitInner(view, model);
        }

        protected abstract void InitInner(V view, M model);

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

        protected virtual void OnShowPage()
        {
        }

        protected virtual void OnHidePage()
        {
        }

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
    }
}