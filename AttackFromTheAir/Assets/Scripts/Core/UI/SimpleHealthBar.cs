using UnityEngine;
using UnityEngine.UI;

public class SimpleHealthBar : HealthBar
{
    [SerializeField] private Image _healthBar;

    public override void SetNormalizedHealth(float value)
    {
        value = Mathf.Clamp01(value);
        _healthBar.fillAmount = value;    
    }
}
