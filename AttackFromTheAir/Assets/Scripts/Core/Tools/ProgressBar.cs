using UnityEngine;

namespace Core.Utilities
{
    public abstract class ProgressBar : MonoBehaviour
    {
        public abstract float Value { get; protected set; }
        public abstract void SetProgressValue(float value);
    }
}