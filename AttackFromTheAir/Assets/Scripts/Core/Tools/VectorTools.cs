using UnityEngine;

namespace Core.Tools
{
    public static class VectorTools
    {
        public static Vector3 GetParabolaVertex(Vector3 pointA, Vector3 pointB, float compressionFactor)
        {
            Vector3 midPoint = (pointA + pointB) / 2f;

            float yMax = midPoint.y + Vector3.Distance(pointA, pointB) / compressionFactor;

            Vector3 vertex = new Vector3(midPoint.x, yMax, midPoint.z);

            return vertex;
        }

        public static Vector3[] GetParabolaPoints(Vector3 pointA, Vector3 vertex, Vector3 pointB, int pointCount)
        {
            Vector3[] points = new Vector3[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                float t = i / (float)(pointCount - 1);

                float x = Mathf.Lerp(Mathf.Lerp(pointA.x, vertex.x, t), Mathf.Lerp(vertex.x, pointB.x, t), t);
                float y = Mathf.Lerp(Mathf.Lerp(pointA.y, vertex.y, t), Mathf.Lerp(vertex.y, pointB.y, t), t);
                float z = Mathf.Lerp(Mathf.Lerp(pointA.z, vertex.z, t), Mathf.Lerp(vertex.z, pointB.z, t), t);

                points[i] = new Vector3(x, y, z);
            }

            return points;
        }
    }
}