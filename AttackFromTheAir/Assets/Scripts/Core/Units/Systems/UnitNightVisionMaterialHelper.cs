using Core.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Units
{
    public class UnitNightVisionMaterialHelper : MonoBehaviour
    {
        [SerializeField] private List<Renderer> _renderers;
        private Dictionary<Renderer, Material[]> _oldMaterialsDict;
        private Dictionary<Renderer, Material[]> _newMaterialsDict;

        private void PrepareOldMaterialsDict()
        {
            if (_oldMaterialsDict != null)
            {
                return;
            }
            _oldMaterialsDict = new Dictionary<Renderer, Material[]>();

            foreach (var renderer in _renderers)
            {
                _oldMaterialsDict.Add(renderer, new Material[renderer.sharedMaterials.Length]);

                var materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    var mat = materials[i];
                    _oldMaterialsDict[renderer][i] = mat;
                }
            }
        }
        
        private void PrepareNewMaterialsDict()
        {
            if (_newMaterialsDict != null)
            {
                return;
            }
            _newMaterialsDict = new Dictionary<Renderer, Material[]>();

            foreach (var renderer in _renderers)
            {
                _newMaterialsDict.Add(renderer, new Material[renderer.sharedMaterials.Length]);

                var materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    var mat = new Material(materials[i]);
                    mat.mainTexture = null;
                    _newMaterialsDict[renderer][i] = mat;
                }
            }
        }

        private void Awake()
        {
            PrepareOldMaterialsDict();
            PrepareNewMaterialsDict();
            EventAggregator.Subscribe<NightVisionSwitchedEvent>(OnNightVisionSwitched);
        }

        private void Start()
        {
            SetWhite();
        }

        private void OnNightVisionSwitched(object sender, NightVisionSwitchedEvent data)
        {
            if (data.isNightVision)
            {
                SetWhite();
            }
            else
            {
                SetTexture();
            }
        }

        private void OnDestroy()
        {
            EventAggregator.Unsubscribe<NightVisionSwitchedEvent>(OnNightVisionSwitched);
        }

        public void SetWhite()
        {
            PrepareNewMaterialsDict();
            foreach (var item in _newMaterialsDict)
            {
                item.Key.materials = item.Value;
            }
        }

        public void SetTexture()
        {
            PrepareOldMaterialsDict();
            foreach (var item in _oldMaterialsDict)
            {
                item.Key.materials = item.Value;
            }
        }
    }
}