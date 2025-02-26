using UnityEngine;

namespace Core.Tools
{
    public static class LayerTools
    {
        public static bool LayerIsInLayerMask(int layer, LayerMask layerMask)
        {
            var result = (1 << layer) & layerMask;
            return result != 0;
        }
    }
}