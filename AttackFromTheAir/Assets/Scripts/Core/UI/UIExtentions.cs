using UnityEngine;

namespace Core.UI
{
    public static class UIExtentions
    {
        // Clear all child gameobjects of the given gameobject.
        public static void ClearAllChild(this GameObject obj)
        {
            if (obj.transform.childCount > 0)
            {
                foreach (Transform child in obj.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        // Clear all child gameobjects of the given transform.
        public static void ClearAllChild(this Transform obj)
        {
            if (obj.childCount > 0)
            {
                foreach (Transform child in obj)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }

        // Clear all child gameobjects of the given rect transform.
        public static void ClearAllChild(this RectTransform obj)
        {
            if (obj.childCount > 0)
            {
                foreach (Transform child in obj)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
    }
}