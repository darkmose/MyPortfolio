using UnityEngine;
using UnityEngine.UI;

public class UITargetArrow : MonoBehaviour
{
    [SerializeField] private Image _arrow;
    public RectTransform Rect => _arrow.rectTransform;

    public void SetColor(Color color)
    {
        _arrow.color = color;
    }
}
