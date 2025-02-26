namespace Core.MVP
{
    public interface IPageView : IView
    {
        void InitPresenter(IPagePresenter presenter);
    }
}