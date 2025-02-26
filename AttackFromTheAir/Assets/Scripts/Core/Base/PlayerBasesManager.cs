using Core.Buildings;
using Core.Utilities;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.LobbyBase
{
    public class PlayerBasesManager : IDisposable
    {
        private PlayerBasesManagerView _view;
        private List<PlayerBaseDescriptor> _playerBaseDescriptors;
        private List<Base> _bases;
        private int _currentBaseIndex;
        public int BaseDescriptorsCount => _playerBaseDescriptors.Count;
        public int CurrentBaseIndex => _currentBaseIndex;
        public Base CurrentBase => _bases[_currentBaseIndex];
        public Base NextBase => _bases[_currentBaseIndex+1];
        public bool HasNextBase => (_currentBaseIndex + 1) < _bases.Count;
        public List<Base> Bases => _bases;
        public SimpleEvent NewBaseAddedEvent { get; } = new SimpleEvent();
        public SimpleEvent<int> CurrentBaseIndexChanged { get; } = new SimpleEvent<int>();
        public PlayerBasesManagerView View => _view;

        public void InitBases(List<PlayerBaseDescriptor> playerBaseDescriptors, int currentBaseIndex)
        {
            _playerBaseDescriptors = playerBaseDescriptors;
            _bases = new List<Base>();
            _currentBaseIndex = currentBaseIndex;

            for (int i = 0; i < playerBaseDescriptors.Count; i++)
            {
                if (i > _currentBaseIndex + 1)
                {
                    break;
                }
                var playerBaseDescriptor = playerBaseDescriptors[i];
                var playerBase = new Base();
                playerBase.SetName(playerBaseDescriptor.BaseName);
                _bases.Add(playerBase);
            }
        }

        public void SetCurrentBaseIndex(int index)
        {
            _currentBaseIndex = index;
            var nextBaseIndex = index + 1;
            if (nextBaseIndex >= _bases.Count && nextBaseIndex < BaseDescriptorsCount)
            {
                if (!CurrentBase.IsLocked)
                {
                    var newBase = AddBase(nextBaseIndex);
                    _view.AddBase(nextBaseIndex);
                    newBase.SetVisibilityMode(false);
                    newBase.InitLoadData(true, new List<BaseObjectSaveData>());
                    InitBasesUpgradableData();
                    NewBaseAddedEvent.Notify();
                }
            }

            CurrentBaseIndexChanged.Notify(index);
        }

        private Base AddBase(int baseIndex)
        {
            var playerBaseDescriptor = _playerBaseDescriptors[baseIndex];
            var playerBase = new Base();
            playerBase.SetName(playerBaseDescriptor.BaseName);
            playerBase.IsLocked = true;
            _bases.Add(playerBase);
            return playerBase;
        }

        public void InitLoadData(List<BaseSaveData> basesSaveData, int selectedBaseIndex)
        {
            if (basesSaveData.Count == 0)
            {
                for (int i = 0; i < _bases.Count; i++)
                {
                    var @base = _bases[i];
                    var emptyObjectsData = new List<BaseObjectSaveData>();
                    @base.InitLoadData(i!=0, emptyObjectsData);
                }
                return;
            }
            for (int i = 0; i < basesSaveData.Count; i++)
            {
                var @base = _bases[i];
                var @data = basesSaveData[i];
                @base.InitLoadData(@data.IsLocked, @data.ObjectsData);
            }

            SetCurrentBaseIndex(selectedBaseIndex);
        }

        public void InitBasesUpgradableData()
        {
            foreach (var @base in _bases)
            {
                if (!@base.IsUpgradableInited)
                {
                    @base.InitBaseObjectsUpgradableData();
                }
            }
        }

        public void MoveCameraToBaseObject(BaseObject baseObject, Action onComplete = null)
        {
            _view?.MoveCameraTo(baseObject, onComplete);
        }

        public void EnableCameraObserveMode()
        {
            _view?.EnableCameraObserveMode();
        }
        
        public void DisableCameraObserveMode(Action onComplete = null)
        {
            _view?.DisableCameraObserveMode(onComplete);
        }
        
        public void MoveCamera(Vector2 moveDelta)
        {
            _view?.MoveCamera(moveDelta);
        }

        public void StopCameraMove()
        {
            _view?.StopCameraMove();
        }

        public BaseObject GetNearestBuilding()
        {
            return _view?.GetCurrentNearestBuilding();
        }

        public BaseObject SelectBaseObjectByScreenTap(Vector2 screenTapPosition)
        {
            return _view.SelectBuildingByScreenTap(screenTapPosition);
        }

        public void SetHammersAttractorTo(Vector3 pos)
        {
            _view.SetHammersAttractorTo(pos);
        }

        public void SetHammersParticlesToViewportPos(Vector3 viewportPos)
        {
            _view.SetHammersParticlesToViewportPos(viewportPos);
        }

        public void EmitHammers(int amount)
        {
            _view.EmitHammers(amount);
        }

        public void ShowBases()
        {
            _view.EnableRenderCamera();
            _view.EnableLight();
            _view.EnableBases();
        }

        public void HideBases()
        {
            _view.DisableRenderCamera();
            _view.DisableLight();
            _view.DisableBases();
        }

        public void LinkView(PlayerBasesManagerView view)
        {
            _view = view;
            view.LinkModel(this);
        }

        public void Dispose()
        {
            _view?.Dispose();
        }
    }
}