using System.Collections.Generic;
using UnityEngine;

namespace Core.LobbyBase
{
    [CreateAssetMenu(fileName =nameof(PlayerBasesHolder), menuName ="ScriptableObjects/"+nameof(PlayerBasesHolder))]
    public class PlayerBasesHolder : ScriptableObject
    {
        public List<PlayerBaseDescriptor> PlayerBaseDescriptors;
    }
}