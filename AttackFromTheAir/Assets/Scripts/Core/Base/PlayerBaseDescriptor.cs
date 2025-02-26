using Core.Buildings;
using System.Collections.Generic;
using UnityEngine;

namespace Core.LobbyBase
{
    [CreateAssetMenu(fileName =nameof(PlayerBaseDescriptor), menuName ="ScriptableObjects/"+nameof(PlayerBaseDescriptor))]
    public class PlayerBaseDescriptor : ScriptableObject
    {
        public string BaseName;
        public BaseView BaseView;
    }
}