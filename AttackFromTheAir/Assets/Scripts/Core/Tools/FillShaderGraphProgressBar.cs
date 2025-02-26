using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Core.Utilities
{
    public class FillShaderGraphProgressBar : ProgressBar, IDisposable
    {
        public enum FillShaderProgressBarStates
        {
            None = -1,
            Inactive,
            ProgressActive,
            Unlocked
        }

        [SerializeField] private List<Renderer> _renderers;
        [SerializeField] private Material _progressBarMaterial;
        [SerializeField] private Material _inactiveMaterial;
        [SerializeField] private Transform _downPoint;
        [SerializeField] private Transform _upPoint;
        private List<Material> _oldMaterials = new List<Material>();
        private List<Material> _materials = new List<Material>();
        private int _mainTextureKey = Shader.PropertyToID("_MainTexture");
        private int _mainColorKey = Shader.PropertyToID("_MainColor");
        private int _progressValueKey = Shader.PropertyToID("_Progress");
        private bool _isDisposed;
        private FillShaderProgressBarStates _progressBarState = FillShaderProgressBarStates.None;
        public FillShaderProgressBarStates ProgressBarState => _progressBarState;
        public override float Value { get; protected set; }

        public void Init()
        {
            SaveOldMaterials();
            GenerateProgressBarShaderMaterials();
        }

        private void SaveOldMaterials()
        {
            foreach (var renderer in _renderers)
            {
                var materials = renderer.materials;
                _oldMaterials.AddRange(materials);
            }
        }

        public Bounds GetCombinedBoundsOfObject()
        {
            if (_renderers.Count == 0)
            {
                return new Bounds(transform.position, Vector3.zero);
            }

            Bounds combinedBounds = _renderers[0].bounds;

            foreach (Renderer renderer in _renderers)
            {
                combinedBounds.Encapsulate(renderer.bounds);
            }

            return combinedBounds;
        }

        private void GenerateProgressBarShaderMaterials()
        {
            foreach (var renderer in _renderers)
            {
                var materials = renderer.materials;
                var newMaterials = new Material[materials.Length];

                for (int i = 0; i < materials.Length; i++)
                {
                    var mat = materials[i];
                    var newMat = new Material(_progressBarMaterial);
                    newMat.mainTexture = mat.mainTexture;
                    newMat.color = mat.color;
                    newMat.SetFloat("_ProgressYDownPoint", _downPoint.position.y);
                    newMat.SetFloat("_ProgressYUpPoint", _upPoint.position.y);
                    newMaterials[i] = newMat;
                }
                _materials.AddRange(newMaterials);
            }
        }

        public void SetState(FillShaderProgressBarStates progressBarState)
        {
            if (_progressBarState == progressBarState)
            {
                return;
            }

            _progressBarState = progressBarState;

            switch (progressBarState)
            {
                case FillShaderProgressBarStates.Inactive:
                    SetInactiveMaterials();
                    break;
                case FillShaderProgressBarStates.ProgressActive:
                    SetShaderProgressBarMaterials();
                    break;
                case FillShaderProgressBarStates.Unlocked:
                    SetOldMaterials();
                    Dispose();
                    break;
                default:
                    break;
            }
        }

        public override void SetProgressValue(float value)
        {
            Value = value;
            if (_isDisposed)
            {
                return;
            }
            foreach (var mat in _materials)
            {
                mat.SetFloat(_progressValueKey, value);
            }
        }

        private void SetOldMaterials()
        {
            int index = 0;

            foreach (var renderer in _renderers)
            {
                var materialsCount = renderer.materials.Length;
                var newMaterials = new Material[materialsCount];
                for (int i = 0; i < materialsCount; i++)
                {
                    newMaterials[i] = _oldMaterials[index];
                    index++;
                }
                renderer.materials = newMaterials;
            }
        }

        private void SetInactiveMaterials()
        {
            foreach (var renderer in _renderers)
            {
                var materialsCount = renderer.materials.Length;
                var newMaterials = new Material[materialsCount];
                for (int i = 0; i < materialsCount; i++)
                {
                    newMaterials[i] = _inactiveMaterial;
                }
                renderer.materials = newMaterials;
            }
        }

        private void SetShaderProgressBarMaterials()
        {
            int index = 0;

            foreach (var renderer in _renderers)
            {
                var materialsCount = renderer.materials.Length;
                var newMaterials = new Material[materialsCount];
                for (int i = 0; i < materialsCount; i++)
                {
                    newMaterials[i] = _materials[index];
                    index++;
                }
                renderer.materials = newMaterials;
            }
        }

        public void Dispose()
        {
            SetOldMaterials();
            foreach (var mat in _materials)
            {
                Destroy(mat);
            }
            _isDisposed = true;
        }
    }
}