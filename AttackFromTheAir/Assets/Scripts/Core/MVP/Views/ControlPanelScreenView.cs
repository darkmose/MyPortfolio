using Core.ControlLogic;
using UnityEngine;

namespace Core.MVP
{
    public class ControlPanelScreenView : BaseScreenView
    {
        [SerializeField] private ControlPanelView _controlPanelView;
        public IControlPanel ControlPanel => _controlPanelView;
        public override ViewType View => ViewType.Panel;
        public override ScreenType Type => ScreenType.ControlPanel;

        public void InitPresenter(IControlPanelScreenPresenter presenter)
        {
        }
    }
}