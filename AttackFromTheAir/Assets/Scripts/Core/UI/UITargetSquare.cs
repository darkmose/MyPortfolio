using UnityEngine;
using UnityEngine.UI;

public class UITargetSquare : MonoBehaviour
{
    [SerializeField] private Image _square;
    [SerializeField] private Image _health;
    [SerializeField] private GameObject _healthPanel;
    public RectTransform Rect => _square.rectTransform;

    public void SetColor(Color color)
    {
        _square.color = color;
        _health.color = color;
    }

    public void SetActiveHealth(bool isActive)
    {
        _healthPanel.SetActive(isActive);
    }

    public void SetHealthNormalized(float health)
    {
        _health.fillAmount = health;
    }
}