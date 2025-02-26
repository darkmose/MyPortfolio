using Core.UI;
using Core.Utilities;

namespace Core.Level
{
    public class MapLevel
    {
        private BoolProperty _isFinished = new BoolProperty(false);
        private BoolProperty _isAvailable = new BoolProperty(false);
        public IPropertyReadOnly<bool> IsFinished => _isFinished;
        public IPropertyReadOnly<bool> IsAvailable => _isAvailable;
        public SimpleEvent<bool> SelectEvent { get; } = new SimpleEvent<bool>();
        public SimpleEvent<MapLevel> MapLevelClickEvent { get; } = new SimpleEvent<MapLevel>();
        public int LevelNumber { get; set; }

        public void Select()
        {
            SelectEvent.Notify(true);
        }

        public void Unselect()
        {
            SelectEvent.Notify(false);
        }

        public void SetFinished(bool isFinished)
        {
            _isFinished.SetValue(isFinished);
        }

        public void SetAvailable(bool isAvailable)
        {
            _isAvailable.SetValue(isAvailable);
        }

        public void OnMapLevelClick()
        {
            MapLevelClickEvent.Notify(this);
        }
    }
}