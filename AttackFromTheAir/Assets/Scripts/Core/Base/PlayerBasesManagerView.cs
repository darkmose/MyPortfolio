using Core.Buildings;
using Core.Tools;
using Core.Utilities;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Core.LobbyBase
{
    public class PlayerBasesManagerView : MonoBehaviour, IDisposable
    {
        private const float MOVE_VECTOR_MULTIPLIER = 0.3f;
        private const float NEAR_ORTHO_SIZE = 60f;
        private const float FAR_ORTHO_SIZE = 90f;
        private const float OBSERVE_ORTHO_SIZE = 220f;
        [SerializeField] private PlayerBasesHolder _basesHolder;
        [SerializeField] private Transform _basesRoot;
        [SerializeField] private Camera _renderCamera;
        [SerializeField] private Light _light;
        [SerializeField] private Vector3 _cameraOffset;
        [SerializeField] private ParticlesWithAttractor _particlesWithAttractor;
        private LayerMask _baseLayerMask;
        private PlayerBasesManager _model;
        private List<BaseView> _baseViews = new List<BaseView>();
        private Sequence _cameraMoveSequence;
        private Vector3 _moveVector = Vector3.zero;
        private RawImage _renderImage;
        private Tweener _observeRotationTweener;
        public Camera BaseCamera => _renderCamera;

        public void LinkModel(PlayerBasesManager model)
        {
            _model = model;
            var currentBaseIndex = model.CurrentBaseIndex;

            if (currentBaseIndex < _basesHolder.PlayerBaseDescriptors.Count)
            {
                for (int i = 0; i < model.Bases.Count; i++)
                {
                    if (i > (currentBaseIndex + 1))
                    {
                        break;
                    }
                    var baseModel = model.Bases[i];
                    var baseViewPrefab = _basesHolder.PlayerBaseDescriptors[i].BaseView;
                    var baseInstance = Instantiate<BaseView>(baseViewPrefab, _basesRoot);

                    baseModel.LinkView(baseInstance);

                    _baseViews.Add(baseInstance);
                }
            }

            _baseLayerMask = LayerMask.GetMask("PlayerBase");
        }

        public void AddBase(int baseIndex)
        {
            if (baseIndex < _basesHolder.PlayerBaseDescriptors.Count)
            {
                var baseModel = _model.Bases[baseIndex];
                var baseViewPrefab = _basesHolder.PlayerBaseDescriptors[baseIndex].BaseView;
                var baseInstance = Instantiate<BaseView>(baseViewPrefab, _basesRoot);

                baseModel.LinkView(baseInstance);

                _baseViews.Add(baseInstance);
            }
        }

        public void InitCameraRenderImage(RawImage rawImage)
        {
            _renderImage = rawImage;
        }

        public BaseObject SelectBuildingByScreenTap(Vector2 screenTapPos)
        {
            var ray = GetRay(screenTapPos);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _baseLayerMask))
            {
                if (hitInfo.collider.TryGetComponent(out BaseObjectView baseObjectView))
                {
                    return baseObjectView.Model;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public void SetHammersAttractorTo(Vector3 pos)
        {
            pos.y += 10f;
            _particlesWithAttractor.SetAttractorPos(pos);
        }

        public void SetHammersParticlesToViewportPos(Vector3 viewportPos)
        {
            var ray = _renderCamera.ViewportPointToRay(viewportPos);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _baseLayerMask))
            {
                var pos = hitInfo.point - ray.direction * 20f;
                _particlesWithAttractor.SetParticlesPos(pos);
            }
        }

        public void EmitHammers(int amount)
        {
            _particlesWithAttractor.Emit(amount);   
        }

        private Ray GetRay(Vector2 screenTapPos)
        {
            var localPoint = _renderImage.rectTransform.InverseTransformPoint(screenTapPos);
            Debug.Log($"Local raw image point: {localPoint}");

            Vector2 viewportPoint = new Vector3(
                (localPoint.x / _renderImage.rectTransform.rect.width) + 0.5f,
                (localPoint.y / _renderImage.rectTransform.rect.height) + 0.5f);

            Ray ray = _renderCamera.ViewportPointToRay(viewportPoint);
            Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, Color.red, 10f);
            return ray;
        }

        public BaseObject GetCurrentNearestBuilding()
        {
            var viewportPoint = Vector3.one * 0.5f;
            var ray = _renderCamera.ViewportPointToRay(viewportPoint);
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _baseLayerMask))
            {
                if (hitInfo.collider.TryGetComponent(out BaseObjectView baseObjectView))
                {
                    return baseObjectView.Model;
                }
                else
                {
                    var overlapColliders = Physics.OverlapSphere(hitInfo.point, 100f, _baseLayerMask);
                    if (overlapColliders.Length > 0)
                    {
                        foreach (var collider in overlapColliders)
                        {
                            if (collider.TryGetComponent(out BaseObjectView baseObjectView1))
                            {
                                return baseObjectView1.Model;
                            }
                        }

                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }                
            }
            else
            {
                return null;
            }
        }

        public void MoveCamera(Vector2 moveDelta)
        {
            moveDelta = CameraVectorConversionService.ConvertVector(moveDelta, _renderCamera.transform.localEulerAngles.y);

            _moveVector.x = moveDelta.x;
            _moveVector.z = moveDelta.y;


            var position = _renderCamera.transform.localPosition + _moveVector * MOVE_VECTOR_MULTIPLIER;
            position.y = _cameraOffset.y;
            _renderCamera.transform.localPosition = position;
        }

        public void MoveCameraTo(BaseObject baseObject, Action onComplete = null)
        {
            var currentBaseIndex = _model.CurrentBaseIndex; 
            var currentBase = _baseViews[currentBaseIndex];
            var objectView = currentBase.GetBaseObjectView(baseObject);

            if (objectView != null)
            {
                _cameraMoveSequence?.Kill();
                var objectPosition = transform.InverseTransformPoint(objectView.transform.position);
                var position = _cameraOffset + objectPosition;
                position.y = _cameraOffset.y;
                var move = _renderCamera.transform.DOLocalMove(position, 0.5f).SetEase(Ease.InOutQuart);
                var fovDown = _renderCamera.DOOrthoSize(NEAR_ORTHO_SIZE, 0.5f);
                _cameraMoveSequence = DOTween.Sequence().Append(move).Append(fovDown).OnComplete(()=>onComplete?.Invoke()).SetEase(Ease.InOutQuad);
                if (_renderCamera.orthographicSize <= 61f)
                {
                    var fovUp = _renderCamera.DOOrthoSize(FAR_ORTHO_SIZE, 0.5f);
                    _cameraMoveSequence.Insert(0f, fovUp);
                }
                _cameraMoveSequence.Play();
            }
            else
            {
                Debug.LogWarning("Current BaseVIEW doesn't conaint building of type " + baseObject + ", but BaseModel does.");
            }
        }

        public void EnableCameraObserveMode()
        {
            _cameraMoveSequence?.Kill();
            var move = _renderCamera.transform.DOLocalMove(_cameraOffset, 1f).SetEase(Ease.InOutQuart);
            var fovUp = _renderCamera.DOOrthoSize(OBSERVE_ORTHO_SIZE, 0.5f);
            _cameraMoveSequence = DOTween.Sequence().Append(move).Join(fovUp);
            _observeRotationTweener?.Kill();
            _observeRotationTweener = BasesObserveRotation(1f);
        }

        private Tweener BasesObserveRotation(float delay = 0f)
        {
            var rotation = Vector3.up * 360f;
            return _basesRoot.DORotate(rotation, 20f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).SetDelay(delay);
        }
        
        public void DisableCameraObserveMode(Action onComplete)
        {
            _cameraMoveSequence?.Kill();
            _observeRotationTweener?.Kill();
            var rotation = Quaternion.Euler(Vector3.zero);
            _observeRotationTweener = _basesRoot.DOLocalRotateQuaternion(rotation, 1f).OnComplete(()=>onComplete?.Invoke());
        }

        public void StopCameraMove()
        {
            _cameraMoveSequence?.Kill();
        }

        public void EnableRenderCamera()
        {
            _renderCamera.enabled = true;
        }

        public void DisableRenderCamera()
        {
            _renderCamera.enabled = false;
        }

        public void EnableLight()
        {
            _light.enabled = true;
        }

        public void DisableLight()
        {
            _light.enabled = false;
        }

        public void EnableBases()
        {
            _basesRoot.gameObject.SetActive(true);
        }

        public void DisableBases()
        {
            _basesRoot.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            DisableRenderCamera();
            DisableLight();
        }
    }
}