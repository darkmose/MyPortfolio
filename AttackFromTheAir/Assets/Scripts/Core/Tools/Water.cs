using UnityEngine;

namespace Core.Tools
{
    public class Water : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Vector2 _waterSpeed;
        private Vector2 _waterOffset;
        private Material _material;

        private void Awake()
        {
            _material = _renderer.sharedMaterial;    
        }

        private void LateUpdate()
        {
            _waterOffset += _waterSpeed;
            if (_waterOffset.x > 9999 || _waterOffset.y > 9999)
            {
                _waterOffset = Vector2.zero;
            }
            _material.mainTextureOffset = _waterOffset;
        }
    }
}