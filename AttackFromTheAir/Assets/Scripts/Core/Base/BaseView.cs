using Core.Buildings;
using Core.Tools;
using Core.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.LobbyBase
{
    public class BaseView : MonoBehaviour, IDisposable
    {
        [SerializeField] private List<BaseObjectView> _objects;
        [SerializeField] private Material _inactiveMaterial;
        private Dictionary<Renderer, List<Material>> _oldMaterialsDictionary;
        private Dictionary<BuildingType, List<BaseObjectView>> _baseObjectsDict;
        public Dictionary<BuildingType, List<BaseObjectView>> BaseObjectsDict => _baseObjectsDict;
        private Base _model;

        private void PrepareBaseObjectsDictionary()
        {
            if (_baseObjectsDict == null)
            {
                _baseObjectsDict = new Dictionary<BuildingType, List<BaseObjectView>>();

                foreach (var obj in _objects)
                {
                    var buildingType = obj.ObjectDescriptor.BuildingType;
                    if (!_baseObjectsDict.ContainsKey(buildingType))
                    {
                        _baseObjectsDict.Add(buildingType, new List<BaseObjectView>());
                    }
                    _baseObjectsDict[buildingType].Add(obj);
                }
            }
        }

        private void PrepareOldMaterialsDictionary()
        {
            _oldMaterialsDictionary = new Dictionary<Renderer, List<Material>>();
            var allRenderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (var renderer in allRenderers)
            {
                if (renderer is ParticleSystemRenderer)
                {
                    continue;
                }
                var baseObjectView = renderer.gameObject.GetComponentInParent<BaseObjectView>();
                if (baseObjectView != null)
                {
                    continue;
                }
                var mats = renderer.sharedMaterials;
                _oldMaterialsDictionary.Add(renderer, new List<Material>(mats));
            }
        }

        public void SetVisibilityMode(bool isActive)
        {
            if (isActive)
            {
                SetActive();
            } 
            else 
            {
                SetInactive();
            }
        }
        
        private void SetActive()
        {
            foreach (var item in _oldMaterialsDictionary)
            {
                item.Key.sharedMaterials = item.Value.ToArray();
            }
        }

        private void SetInactive()
        {
            foreach (var renderer in _oldMaterialsDictionary.Keys)
            {
                var materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = _inactiveMaterial;
                }
                renderer.sharedMaterials = materials;
            }
        }

        public void LinkModel(Base model)
        {
            _model = model;
            PrepareBaseObjectsDictionary();
            PrepareOldMaterialsDictionary();
        }

        public BaseObjectView GetBaseObjectView(BaseObject baseObject)
        {
            var baseView = _objects.Find(pred=>pred.IsLinkedObjects(baseObject));
            return baseView;
        }

        public void Dispose()
        {
        }
    }
}