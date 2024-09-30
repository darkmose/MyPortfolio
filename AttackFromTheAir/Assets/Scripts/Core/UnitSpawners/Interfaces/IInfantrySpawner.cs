using Core.Buildings;

namespace Core.GameLogic
{
    public interface IInfantrySpawner : IUnitSpawner
    {
        InfantryType InfantryType { get; }
    }
}