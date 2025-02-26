using System;
using Zenject;

namespace Core.MVP
{
    public abstract class BasePagePresenterModelUseCases<V, M, U> : IPagePresenter
    where V : IPageView
    where M : IPageModel
    where U : IPageUseCases
    {
        private DiContainer _diContainer;
        public V View { get; private set; }
        public M Model { get; private set; }
        public U UseCases { get; private set; }
        protected bool IsOpened { get; private set; }

        protected BasePagePresenterModelUseCases(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void Init(V view, M model, U useCases)
        {
            View = view;
            Model = model;
            UseCases = useCases;

            view.InitPresenter(this);
            model.Init(_diContainer);
            useCases.Init(_diContainer);

            InitInner(view, model, useCases);
        }

        protected abstract void InitInner(V view, M model, U useCases);

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