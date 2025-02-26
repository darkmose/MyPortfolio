using Core.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.LobbyBase
{
    public class BaseObjectSelectorView : MonoBehaviour, IDisposable
    {
        [SerializeField] private RawImage _renderTexture;
        [SerializeField] private BaseObjectSelectRect _selectRectPrefab;
        [SerializeField] private RectTransform _rectRoot;
        private BaseObjectSelector _model;
        private List<BaseObjectSelectRect> _rects = new List<BaseObjectSelectRect>();
        private Camera _baseCamera;

        public void LinkModel(BaseObjectSelector baseObjectSelector)
        {
            _model = baseObjectSelector;
        }

        public void InitBaseCamera(Camera baseCamera)
        {
            _baseCamera = baseCamera;
        }

        //public void SelectBaseObjects(List<BaseObject> baseObjects, int mainObjectIndex)
        //{
        //    for (int i = 0; i < baseObjects.Count; i++)
        //    {
        //        var obj = baseObjects[i];
        //        var objPos = obj.View.transform.position;
        //        var viewportPoint = _baseCamera.WorldToViewportPoint(objPos);
        //        if ((viewportPoint.x < 0 || viewportPoint.x > 1) || (viewportPoint.y < 0 || viewportPoint.y > 1))
        //        {
        //            continue;
        //        }

        //        var rect = Instantiate<BaseObjectSelectRect>(_selectRectPrefab, _rectRoot);

        //        InitRectSize(rect, obj.View);

        //        if (i == mainObjectIndex)
        //        {
        //            rect.SetActiveSprite();
        //            rect.SetActiveObjectLevel(true);
        //        }
        //        else
        //        {
        //            rect.transform.SetAsFirstSibling();
        //            rect.SetInactiveSprite();
        //            rect.SetActiveObjectLevel(false);
        //        }
        //    }
        //}

        public void SelectBaseObject(BaseObject baseObject)
        {
            var rect = Instantiate<BaseObjectSelectRect>(_selectRectPrefab, _rectRoot);
            InitRectSize(rect, baseObject.View);

            rect.SetActiveSprite();
            rect.SetActiveObjectLevel(true);       
            rect.LinkBaseObject(baseObject);
            _rects.Add(rect);
        }

        public void InitRectSize(BaseObjectSelectRect rect, BaseObjectView objectView)
        {
            Vector3[] corners = new Vector3[8];
            Bounds bounds = objectView.GetObjectBounds();

            corners[0] = bounds.min;
            corners[1] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            corners[3] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
            corners[4] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
            corners[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            corners[6] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
            corners[7] = bounds.max;

            Vector3[] screenPoints = new Vector3[8];
            Camera cam = _baseCamera;

            for (int i = 0; i < corners.Length; i++)
            {
                screenPoints[i] = cam.WorldToScreenPoint(corners[i]);
            }

            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;

            foreach (Vector3 point in screenPoints)
            {
                minX = Mathf.Min(minX, point.x);
                minY = Mathf.Min(minY, point.y);
                maxX = Mathf.Max(maxX, point.x);
                maxY = Mathf.Max(maxY, point.y);
            }

            float width = maxX - minX;
            float height = maxY - minY;
            //var sideSize = Mathf.Max(width, height);
            //var size = new Vector2(sideSize, sideSize);
            var screenBoundsCenterPosition = new Vector2(minX + width / 2f, minY + height / 2f);
            Vector2 viewportPoint = _baseCamera.ScreenToViewportPoint(screenBoundsCenterPosition);
            var renderTextureSize = _renderTexture.rectTransform.rect.size;
            var renderTextureBottomLeftCorner = _renderTexture.rectTransform.position - (Vector3)(renderTextureSize / 2f);
            var screenXPos = renderTextureBottomLeftCorner.x + (viewportPoint.x * renderTextureSize.x);
            var screenYPos = renderTextureBottomLeftCorner.y + (viewportPoint.y * renderTextureSize.y);

            var screenPos = new Vector2(screenXPos, screenYPos);
            rect.transform.position = screenPos;
            //rect.SetSize(size);
        }

        public void ClearRoot()
        {
            foreach (var rect in _rects)
            {
                rect.Dispose();
            }
            _rects.Clear();
            _rectRoot.ClearAllChild();
        }

        public void Dispose()
        {
            ClearRoot();
        }
    }
}