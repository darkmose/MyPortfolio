using Core.UI;
using Core.Utilities;
using static Core.GameLogic.PlayerDroneProgressionService;

namespace Core.GameLogic
{
    public interface IPlayerDroneDecorator
    {
        SimpleEvent DataChangedEvent { get; }
        PlayerDroneConfig GetDroneConfig();
    }
}