//using System.Collections.Generic;
//using UnityEngine;

//public class MeshCutter : MonoBehaviour
//{
//    public int gridX = 4; // Количество частей по оси X
//    public int gridY = 4; // Количество частей по оси Y


//    [ContextMenu("Start Mesh Cutter ")]
//    private void StartMeshCutter()
//    {

//        CutMesh();
//    }

//    private void CutMesh()
//    {
//        Mesh mesh = GetComponent<MeshFilter>().mesh;
//        Vector3[] vertices = mesh.vertices;
//        int[] triangles = mesh.triangles;
//        Vector2[] uv = mesh.uv;

//        Bounds bounds = mesh.bounds;
//        float stepX = bounds.size.x / gridX;
//        float stepZ = bounds.size.z / gridY;

//        List<Mesh> newMeshes = new List<Mesh>();

//        for (int i = 0; i < gridX; i++)
//        {
//            for (int j = 0; j < gridY; j++)
//            {
//                List<Vector3> partVertices = new List<Vector3>();
//                List<Vector2> partUVs = new List<Vector2>();
//                List<int> partTriangles = new List<int>();

//                Dictionary<int, int> vertexMap = new Dictionary<int, int>();

//                Vector3 min = new Vector3(bounds.min.x + i * stepX, bounds.min.y, bounds.min.z + j * stepZ);
//                Vector3 max = new Vector3(bounds.min.x + (i + 1) * stepX, bounds.max.y, bounds.min.z + (j + 1) * stepZ);

//                for (int t = 0; t < triangles.Length; t += 3)
//                {
//                    Vector3 v1 = vertices[triangles[t]];
//                    Vector3 v2 = vertices[triangles[t + 1]];
//                    Vector3 v3 = vertices[triangles[t + 2]];

//                    if (IsTriangleInBounds(v1, v2, v3, min, max))
//                    {
//                        AddTriangle(partVertices, partUVs, partTriangles, vertexMap, v1, v2, v3, uv, triangles[t], triangles[t + 1], triangles[t + 2]);
//                    }
//                }

//                if (partVertices.Count > 0)
//                {
//                    Mesh partMesh = new Mesh();
//                    partMesh.vertices = partVertices.ToArray();
//                    partMesh.uv = partUVs.ToArray();
//                    partMesh.triangles = partTriangles.ToArray();
//                    partMesh.RecalculateNormals();
//                    partMesh.RecalculateBounds();

//                    newMeshes.Add(partMesh);
//                }
//            }
//        }



//        foreach (Mesh partMesh in newMeshes)
//        {
//            GameObject partObject = new GameObject("MeshPart");
//            partObject.transform.position = transform.position;
//            partObject.AddComponent<MeshFilter>().mesh = partMesh;
//            partObject.AddComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
//        }

//        GetComponent<MeshRenderer>().enabled = false;
//    }

//    private void AddTriangle(List<Vector3> partVertices, List<Vector2> partUVs, List<int> partTriangles, Dictionary<int, int> vertexMap,
//                     Vector3 v1, Vector3 v2, Vector3 v3, Vector2[] uv, int index1, int index2, int index3)
//    {
//        int[] indices = new int[3];
//        indices[0] = AddVertex(partVertices, partUVs, vertexMap, v1, uv[index1], index1);
//        indices[1] = AddVertex(partVertices, partUVs, vertexMap, v2, uv[index2], index2);
//        indices[2] = AddVertex(partVertices, partUVs, vertexMap, v3, uv[index3], index3);

//        partTriangles.Add(indices[0]);
//        partTriangles.Add(indices[1]);
//        partTriangles.Add(indices[2]);
//    }

//    private int AddVertex(List<Vector3> partVertices, List<Vector2> partUVs, Dictionary<int, int> vertexMap, Vector3 vertex, Vector2 uv, int originalIndex)
//    {
//        if (vertexMap.TryGetValue(originalIndex, out int index))
//        {
//            return index;
//        }
//        else
//        {
//            index = partVertices.Count;
//            partVertices.Add(vertex);
//            partUVs.Add(uv);
//            vertexMap[originalIndex] = index;
//            return index;
//        }
//    }

//    private bool IsTriangleInBounds(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 min, Vector3 max)
//    {
//        return IsPointInBounds(v1, min, max) || IsPointInBounds(v2, min, max) || IsPointInBounds(v3, min, max);
//    }

//    private bool IsPointInBounds(Vector3 point, Vector3 min, Vector3 max)
//    {
//        return point.x >= min.x && point.x <= max.x && point.z >= min.z && point.z <= max.z;
//    }

//}
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeshCutter : MonoBehaviour
{
    private int _gridX = 4; // Количество частей по оси X
    private int _gridY = 4; // Количество частей по оси Y

    private int meshCount = 0; // Счетчик для присвоения номеров мешам
    private string _nameRootFolder = "LevelMesh";
    private string _nameFolder = "Map1";

    private GameObject folderObject;
  
    public void SetGrid(int gridX,int gridY)
    {
        _gridX = gridX;
        _gridY = gridY;
    }
    public void SetNameFolder(string nameFolder)
    {
        _nameFolder = nameFolder;
    }

    [ContextMenu("Start Mesh Cutter")]
    public void StartMeshCutter()
    {
        folderObject = new GameObject(_nameFolder); //создание корневой папки
        CutMesh();
    }

    private void CutMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector2[] uv = mesh.uv;

        Bounds bounds = mesh.bounds;
        float stepX = bounds.size.x / _gridX;
        float stepZ = bounds.size.z / _gridY;

        List<Mesh> newMeshes = new List<Mesh>();

        for (int i = 0; i < _gridX; i++)
        {
            for (int j = 0; j < _gridY; j++)
            {
                List<Vector3> partVertices = new List<Vector3>();
                List<Vector2> partUVs = new List<Vector2>();
                List<int> partTriangles = new List<int>();

                Dictionary<int, int> vertexMap = new Dictionary<int, int>();

                Vector3 min = new Vector3(bounds.min.x + i * stepX, bounds.min.y, bounds.min.z + j * stepZ);
                Vector3 max = new Vector3(bounds.min.x + (i + 1) * stepX, bounds.max.y, bounds.min.z + (j + 1) * stepZ);

                for (int t = 0; t < triangles.Length; t += 3)
                {
                    Vector3 v1 = vertices[triangles[t]];
                    Vector3 v2 = vertices[triangles[t + 1]];
                    Vector3 v3 = vertices[triangles[t + 2]];

                    if (IsTriangleInBounds(v1, v2, v3, min, max))
                    {
                        AddTriangle(partVertices, partUVs, partTriangles, vertexMap, v1, v2, v3, uv, triangles[t], triangles[t + 1], triangles[t + 2]);
                    }
                }

                if (partVertices.Count > 0)
                {
                    Mesh partMesh = new Mesh();
                    partMesh.vertices = partVertices.ToArray();
                    partMesh.uv = partUVs.ToArray();
                    partMesh.triangles = partTriangles.ToArray();
                    partMesh.RecalculateNormals();
                    partMesh.RecalculateBounds();

                    newMeshes.Add(partMesh);
                }
            }
        }

        foreach (Mesh partMesh in newMeshes)
        {
            // Увеличиваем счетчик для каждой части
            meshCount++;

            // Создаем новый объект для каждой части
            GameObject partObject = new GameObject("MeshPart_" + meshCount); // Имя с порядковым номером
            partObject.transform.position = transform.position;
            partObject.AddComponent<MeshFilter>().mesh = partMesh;
            partObject.AddComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;
            partObject.AddComponent<MeshCollider>();

#if UNITY_EDITOR

            // Создаем или проверяем наличие корневой папки и подпапки
            string rootFolderPath = "Assets/" + _nameRootFolder;
            if (!AssetDatabase.IsValidFolder(rootFolderPath))
            {
                AssetDatabase.CreateFolder("Assets", _nameRootFolder);
            }

            string finalFolderPath = rootFolderPath + "/" + _nameFolder;
            if (!AssetDatabase.IsValidFolder(finalFolderPath))
            {
                AssetDatabase.CreateFolder(rootFolderPath, _nameFolder);
            }

            // Сохранение меша в соответствующую папку
            string path = finalFolderPath + "/MeshPart_" + meshCount + ".asset";
            AssetDatabase.CreateAsset(partMesh, path);
            AssetDatabase.SaveAssets();

            partObject.transform.SetParent(folderObject.transform); // добавление в папку 
#endif
        }

        GetComponent<MeshRenderer>().enabled = false;

        //DestroyImmediate(this); //удаление могу быть ошыьки 

        DestroyImmediate(gameObject);
    }

    private void AddTriangle(List<Vector3> partVertices, List<Vector2> partUVs, List<int> partTriangles, Dictionary<int, int> vertexMap,
                     Vector3 v1, Vector3 v2, Vector3 v3, Vector2[] uv, int index1, int index2, int index3)
    {
        int[] indices = new int[3];
        indices[0] = AddVertex(partVertices, partUVs, vertexMap, v1, uv[index1], index1);
        indices[1] = AddVertex(partVertices, partUVs, vertexMap, v2, uv[index2], index2);
        indices[2] = AddVertex(partVertices, partUVs, vertexMap, v3, uv[index3], index3);

        partTriangles.Add(indices[0]);
        partTriangles.Add(indices[1]);
        partTriangles.Add(indices[2]);
    }

    private int AddVertex(List<Vector3> partVertices, List<Vector2> partUVs, Dictionary<int, int> vertexMap, Vector3 vertex, Vector2 uv, int originalIndex)
    {
        if (vertexMap.TryGetValue(originalIndex, out int index))
        {
            return index;
        }
        else
        {
            index = partVertices.Count;
            partVertices.Add(vertex);
            partUVs.Add(uv);
            vertexMap[originalIndex] = index;
            return index;
        }
    }

    private bool IsTriangleInBounds(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 min, Vector3 max)
    {
        return IsPointInBounds(v1, min, max) || IsPointInBounds(v2, min, max) || IsPointInBounds(v3, min, max);
    }

    private bool IsPointInBounds(Vector3 point, Vector3 min, Vector3 max)
    {
        return point.x >= min.x && point.x <= max.x && point.z >= min.z && point.z <= max.z;
    }
}
