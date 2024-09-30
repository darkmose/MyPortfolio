using UnityEngine;

namespace Core.Tools
{
    public class TransformsHolderService : MonoBehaviour
    {
        private static TransformsHolderService _innerInstance;
        [SerializeField] private Transform _projectilesContainer;
        [SerializeField] private Transform _usedProjectilesContainer;
        public static Transform ProjectilesContainer => _innerInstance._projectilesContainer;
        public static Transform UsedProjectilesContainer => _innerInstance._usedProjectilesContainer;
        private void Awake()
        {
            if (_innerInstance == null)
            {
                _innerInstance = this;
            }
            else 
            {
                Destroy(gameObject);
            }
        }
        
    }
}