using Core.Level;
using Core.UI;

namespace Core.MVP
{
    public class LoadingScreenModel : IModel
    {
        public IPropertyReadOnly<int> CurrentLevel { get; }

        public LoadingScreenModel(LevelSelector levelSelector)
        {
            CurrentLevel = levelSelector.CurrentLevel;
        }
    }
}