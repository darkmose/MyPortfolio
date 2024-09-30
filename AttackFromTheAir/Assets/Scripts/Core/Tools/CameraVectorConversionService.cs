using UnityEngine;

namespace Core.Tools
{
    public class CameraVectorConversionService
    {
        private static Camera _camera;
        private static Vector3 _upVector = Vector3.up;
        private static Vector2 _cachedVector = Vector2.zero;

        public CameraVectorConversionService()
        {
            _camera = Camera.main;
        }

        public static Vector2 ConvertVector(Vector2 vector)
        {
            var angle = _camera.transform.eulerAngles.y;
            var vec3 = new Vector3(vector.x, 0, vector.y);
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            var convertedVec3 = rotation * vec3;
            _cachedVector.x = convertedVec3.x;
            _cachedVector.y = convertedVec3.z;
            return _cachedVector;
        }

        public static Vector2 ConvertVector(Vector2 vector, Vector3 cameraRotation)
        {
            var vec3 = new Vector3(vector.x, 0, vector.y);
            Quaternion rotation = Quaternion.AngleAxis(cameraRotation.y, _upVector);
            var convertedVec3 = rotation * vec3;
            _cachedVector.x = convertedVec3.x;
            _cachedVector.y = convertedVec3.z;
            return _cachedVector;
        }
    }
}