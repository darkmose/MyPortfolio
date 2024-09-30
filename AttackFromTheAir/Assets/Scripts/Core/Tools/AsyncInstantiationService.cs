using System;
using UnityEngine;

namespace Core.Tools
{
    public class AsyncInstantiationService : MonoBehaviour
    {
        public void Instantiate<T>(T obj, Vector3 position, Quaternion rotation, Transform parent = null, Action<T> onComplete = null) where T : UnityEngine.Object
        {
            var asyncInstantiateOperation = InstantiateAsync<T>(obj, parent, position, rotation);
            asyncInstantiateOperation.completed += (operation) => 
            {
                onComplete?.Invoke(asyncInstantiateOperation.Result[0]); 
            };
        }
    }
}