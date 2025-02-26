using Core.Buildings;
using UnityEngine;

namespace Core.LobbyBase
{
    [CreateAssetMenu(fileName =nameof(BaseObjectDescriptor), menuName ="ScriptableObjects/"+nameof(BaseObjectDescriptor))]
    public class BaseObjectDescriptor : ScriptableObject
    {
        public BuildingType BuildingType;
    }
}