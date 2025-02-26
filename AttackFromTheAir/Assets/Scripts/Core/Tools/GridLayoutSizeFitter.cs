using UnityEngine;
using UnityEngine.UI;

namespace Core.Utilities
{
    public class GridLayoutSizeFitter : MonoBehaviour
    {
        [SerializeField] private RectTransform _gridRoot;
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        [SerializeField] private bool _fitByWidth;
        [SerializeField] private bool _fitByHeight;
        [SerializeField] private int _fixedColumnCount = 1;
        [SerializeField] private int _fixedRowCount = 1;
        [SerializeField] private bool _considerSpacing;
        [SerializeField] private bool _considerPadding;

        private void Start()
        {
            Invoke(nameof(Fit), 0.1f);
        }

        private void Fit()
        {
            var size = _gridRoot.rect.size;
            if (_considerSpacing)
            {
                var spacingX = _gridLayoutGroup.spacing.x * (_fixedRowCount - 1);
                var spacingY = _gridLayoutGroup.spacing.y * (_fixedColumnCount - 1);
                size.x -= spacingX;
                size.y -= spacingY;
            }
            if (_considerPadding)
            {
                var paddingHorizontal = _gridLayoutGroup.padding.left + _gridLayoutGroup.padding.right;
                var paddingVerical = _gridLayoutGroup.padding.bottom + _gridLayoutGroup.padding.top;
                size.x -= paddingHorizontal;
                size.y -= paddingVerical;
            }

            Vector2 cellSize = Vector2.zero;
            size.x /= (float)_fixedColumnCount;
            size.y /= (float)_fixedRowCount;

            if (_fitByWidth && _fitByHeight)
            {
                cellSize = size;
            }
            else if (_fitByWidth && !_fitByHeight)
            {
                cellSize = new Vector2(size.x, size.x);
            }
            else if (!_fitByWidth && _fitByHeight)
            {
                cellSize = new Vector2(size.y, size.y);
            }

            _gridLayoutGroup.cellSize = cellSize;
        }
    }
}