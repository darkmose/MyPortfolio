using Core.Resourses;
using Core.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameLogic
{
    public class BeforeLevelDroneSelectorView : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform _root;
        [SerializeField] private DroneStatusPanelView _droneStatusPanelPrefab;
        private List<DroneStatusPanelView> _droneStatusPanels = new List<DroneStatusPanelView>();
        private BeforeLevelDroneSelector _model;
        public SimpleEvent<PlayerDroneType> DronePanelClickEvent { get; } = new SimpleEvent<PlayerDroneType>();

        public void LinkModel(BeforeLevelDroneSelector model)
        {
            _model = model;
            var droneStatusDictionary = _model.DroneStatusDictionary;
            foreach (var item in droneStatusDictionary)
            {
                var droneType = item.Key;
                var droneStatusPanelInstance = Instantiate(_droneStatusPanelPrefab, _root);
                droneStatusPanelInstance.SetDroneType(droneType);
                droneStatusPanelInstance.DronePanelClickEvent.AddListener(OnStatusPanelViewClick);
                _droneStatusPanels.Add(droneStatusPanelInstance);
                LinkPanelModelView(item.Value, droneStatusPanelInstance);
            }
        }

        private void OnStatusPanelViewClick(DroneStatusPanelView view)
        {
            DronePanelClickEvent.Notify(view.DroneType);
        }

        private void LinkPanelModelView(DroneStatus model, DroneStatusPanelView view)
        {
            model.AvailableCount.RegisterValueChangeListener(view.SetAvailableCount);
            model.CooldownRemain.RegisterValueChangeListener(view.SetAvailabilityInfo);
            model.CanUse.RegisterValueChangeListener(view.SetDroneAvailability);
            model.IsLocked.RegisterValueChangeListener(view.SetLocked);
            model.IsSelected.RegisterValueChangeListener(view.SetSelection);

            view.SetAvailableCount(model.AvailableCount.Value);
            view.SetAvailabilityInfo(model.CooldownRemain.Value);
            view.SetDroneAvailability(model.CanUse.Value);
            view.SetLocked(model.IsLocked.Value);
            view.SetSelection(model.IsSelected.Value);
        }

        public void Dispose()
        {
            foreach (var panel in _droneStatusPanels)
            {
                panel.DronePanelClickEvent.RemoveAllListeners();
            }
            _droneStatusPanels.Clear();
        }
    }

}