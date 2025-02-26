using System.Collections.Generic;
using UnityEngine;
using Core.Events;
using System.Drawing;
using Core.GameLogic;

namespace Core.Tools.Effect
{
    public interface IVisualEffectService
    {
        void ClearTrails();
    }
    public class VisualEffectService : IVisualEffectService
    {
        private List<GameObject> activeTrails = new List<GameObject>();
        private List<Sprite> _listExplovetSprite;
        private Vector3 _rotatonSprite = new Vector3(-90f, 0f, 0f);
        private float _verticalOffset = 0.02f;
        private LayerMask groundLayer = LayerMask.GetMask("Ground");
        private float raycastDistance = 100f;
        private Vector3 TrailSizeBomb = new Vector3(.9f, .9f, .9f);
        private Vector3 TrailSizeBullet = new Vector3(0.12f, 0.12f, 0.12f);


        private Vector3 GetGroundPoint(Vector3 startPosition)
        {
            if (Physics.Raycast(startPosition, Vector3.down, out RaycastHit hitInfo, raycastDistance, groundLayer))
            {
                _rotatonSprite = hitInfo.normal;
                return hitInfo.point;
            }
            return startPosition;

        }

        public VisualEffectService(List<Sprite> explosionSprite)
        {
            _listExplovetSprite = explosionSprite;
            EventAggregator.Subscribe<ExplosionEvent>(OnExplosionEffect); 
        }

        public void ClearTrails() 
        {
            EventAggregator.Unsubscribe<ExplosionEvent>(OnExplosionEffect);  

            foreach (var trail in activeTrails)
            {
                UnityEngine.Object.Destroy(trail);
            }
            activeTrails.Clear();
        }

        private Sprite GetRandomTrailSprite() 
        {
            return _listExplovetSprite[UnityEngine.Random.Range(0, _listExplovetSprite.Count)];
        }

        private void PlaceTrail(Vector3 position, PlayerWeaponType playerWeaponType)
        {

            GameObject trail = new GameObject("Trail");

            SpriteRenderer renderer = trail.AddComponent<SpriteRenderer>();
            renderer.sprite = GetRandomTrailSprite();  

            Vector3 posStart = GetGroundPoint(position);
            Vector3 positionSprite = new Vector3(posStart.x, posStart.y + _verticalOffset, posStart.z);
            Quaternion rot = Quaternion.LookRotation(_rotatonSprite);

            trail.transform.position = positionSprite;
            trail.transform.rotation = rot;
            var trailSize = playerWeaponType == PlayerWeaponType.ThrowOffBomb ? TrailSizeBomb : TrailSizeBullet;
            var randomMultiplier = Random.Range(0.7f, 1.3f);
            trail.transform.localScale = trailSize * randomMultiplier;

            Material material = new Material(Shader.Find("Sprites/Default"));
            renderer.material = material;
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0); 
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            UnityEngine.Color color = renderer.material.color;
            color.a = 0.6f;  
            renderer.material.color = color;
            activeTrails.Add(trail);
        }

        private void OnExplosionEffect(object sender, ExplosionEvent data)
        { 
            PlaceTrail(data.Position,data.playerWeaponType);
        }
    }
}

