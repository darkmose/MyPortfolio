using UnityEngine;

public class ConsoleHealthBar : HealthBar
{
    public override void SetNormalizedHealth(float value)
    {
#if UNITY_EDITOR
        Debug.Log($"[{gameObject.name}][SetNormalizedHealth] value : {value}");
#endif
    }
}