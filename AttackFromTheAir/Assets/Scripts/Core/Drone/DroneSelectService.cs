using Core.Resourses;
using Core.UI;

namespace Core.GameLogic
{
    public interface IDroneSelectService
    {
        IPropertyReadOnly<PlayerDroneType> SelectedDrone { get; }
    }

    public class DroneSelectService : IDroneSelectService
    {
        private CustomProperty<PlayerDroneType> _selectedDrone = new CustomProperty<PlayerDroneType>(PlayerDroneType.SmallDrone);
        public IPropertyReadOnly<PlayerDroneType> SelectedDrone => _selectedDrone;

        public void NextDrone()
        {

        }

        public void PrevDrone() 
        {
        
        }
    }
}