using Core.GameLogic;
using UnityEngine;

namespace Core.Buildings
{
    public abstract class BaseBuildingView : BaseDamagableObjectView, IBuildingView
    {
        [SerializeField] private BuildingSystems _buildingSystems;
        public abstract BuildingType BuildingType { get; }
        public BuildingSystems BuildingSystems => _buildingSystems;
    }
}