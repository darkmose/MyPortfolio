using Core.Buildings;
using Core.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.LobbyBase
{
    public class BaseObjectSelector : IDisposable
    {
        private BaseObjectSelectorView _view;
        private PlayerBasesManager _playerBasesManager;
        private BuildingType _lastSelectedBuildingType = BuildingType.None;
        private int _selectedBuildingIndex = -1;
        private CustomProperty<BaseObject> _currentBaseObject = new CustomProperty<BaseObject>(null);
        private ListProperty<BaseObject> _selectedObjects = new ListProperty<BaseObject>();
        private List<BaseObject> _currentObjectsList = new List<BaseObject>();
        public IPropertyReadOnly<BaseObject> CurrentBaseObject => _currentBaseObject;
        public IPropertyReadOnly<List<BaseObject>> SelectedObjects => _selectedObjects;

        public BaseObjectSelector(PlayerBasesManager playerBasesManager)
        {
            _playerBasesManager = playerBasesManager;
        }

        public void SelectBaseObject(BuildingType buildingType)
        {
            var currentBase = _playerBasesManager.CurrentBase;
            if (currentBase.BaseObjectsDict.TryGetValue(buildingType, out var list))
            {
                _currentObjectsList = list;

                if (_lastSelectedBuildingType != buildingType)
                {
                    _lastSelectedBuildingType = buildingType;
                    _selectedBuildingIndex = -1;
                }
                else if(list.Count == 1)
                {
                    return;
                }

                _selectedObjects.Notify(list);
                NextSelectedObject();
            }
        }

        public void SelectBaseObject(BaseObject baseObject)
        {
            var currentBase = _playerBasesManager.CurrentBase;
            if (currentBase.BaseObjectsDict.TryGetValue(baseObject.BaseObjectType, out var list))
            {
                var objectIndex = list.IndexOf(baseObject);
                _lastSelectedBuildingType = baseObject.BaseObjectType;
                _selectedBuildingIndex = objectIndex;
                _currentObjectsList = list;
                _selectedObjects.Notify(list);
                SelectByIndex();
            }
        }

        public void ClearViewSelection()
        {
            _playerBasesManager.StopCameraMove();
            _view.ClearRoot();
        }

        private void SelectByIndex()
        {
            var selectedObject = _currentObjectsList[_selectedBuildingIndex];
            _currentBaseObject.SetValue(selectedObject, false);
            ClearViewSelection();
            _playerBasesManager.MoveCameraToBaseObject(selectedObject, () =>
            {
                _view.SelectBaseObject(selectedObject);
            });
        }

        public void NextSelectedObject()
        {
            _selectedBuildingIndex++;
            if (_selectedBuildingIndex >= _currentObjectsList.Count)
            {
                _selectedBuildingIndex = 0;
            }
            SelectByIndex();
        }

        public void PrevSelectedObject()
        {
            _selectedBuildingIndex--;
            if (_selectedBuildingIndex < 0)
            {
                _selectedBuildingIndex = _currentObjectsList.Count - 1;
            }
            SelectByIndex();
        }

        public void LinkView(BaseObjectSelectorView view)
        {
            _view = view;
            _view.InitBaseCamera(_playerBasesManager.View.BaseCamera);
            view.LinkModel(this);
        }

        public void Dispose()
        {
            _view?.Dispose();
        }
    }
}