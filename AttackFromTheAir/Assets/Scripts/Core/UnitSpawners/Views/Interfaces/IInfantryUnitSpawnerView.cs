using Core.Buildings;

namespace Core.GameLogic
{
    public interface IInfantryUnitSpawnerView : IUnitSpawnerView
    {
        InfantryType InfantryType { get; }
    }
}