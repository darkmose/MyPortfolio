using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TerrainToMesh : MonoBehaviour
{
    [SerializeField] private Terrain terrain; // Ссылка на террейн, который нужно конвертировать
    [SerializeField] private int meshResolution = 128; // Разрешение меша

    [SerializeField] private int _indexMapMesh = 1;
    [SerializeField] private Material  _material;


    [Header("MeshCutter")]
    public int gridX = 4; // Количество частей по оси X
    public int gridY = 4; // Количество частей по оси Y



    [ContextMenu("Convert Terrain to Mesh")]
    private void ConvertTerrainToMesh()
    {

        _indexMapMesh++;

        Mesh mesh = ConvertTerrainToMesh(terrain, meshResolution);
        GameObject meshObject = new GameObject("MapMesh"+$"{_indexMapMesh}");
        MeshFilter meshFilter = meshObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer renderer = meshObject.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = /*terrain.materialTemplate;*/_material;

        meshObject.transform.position = terrain.transform.position;
        meshObject.transform.rotation = terrain.transform.rotation;


        //
        MeshCutter meshCutter = meshObject.AddComponent<MeshCutter>();//добавиь компоне для создания нескольких мешей
        string name = string.Format($"Map{_indexMapMesh}");
        meshCutter.SetNameFolder(name);
        meshCutter.SetGrid(gridX, gridY);
        //

        meshCutter.StartMeshCutter(); //старт создание нових месшей
    }

    private Mesh ConvertTerrainToMesh(Terrain terrain, int resolution)
    {
        TerrainData terrainData = terrain.terrainData;
        int width = terrainData.heightmapResolution;
        int height = terrainData.heightmapResolution;
        Vector3 meshScale = terrainData.size;
        float[,] heights = terrainData.GetHeights(0, 0, width, height);

        int meshWidth = Mathf.Min(width, resolution);
        int meshHeight = Mathf.Min(height, resolution);
        float stepSizeX = (float)(width - 1) / (meshWidth - 1);
        float stepSizeY = (float)(height - 1) / (meshHeight - 1);

        int numVertices = meshWidth * meshHeight;
        int numTriangles = (meshWidth - 1) * (meshHeight - 1) * 6;

        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uvs = new Vector2[numVertices];
        int[] triangles = new int[numTriangles];

        for (int y = 0; y < meshHeight; y++)
        {
            for (int x = 0; x < meshWidth; x++)
            {
                int vertexIndex = y * meshWidth + x;
                float heightValue = heights[Mathf.RoundToInt(y * stepSizeY), Mathf.RoundToInt(x * stepSizeX)];

                vertices[vertexIndex] = new Vector3(
                    (x / (float)(meshWidth - 1)) * meshScale.x,
                    heightValue * meshScale.y,
                    (y / (float)(meshHeight - 1)) * meshScale.z);

                uvs[vertexIndex] = new Vector2(x / (float)(meshWidth - 1), y / (float)(meshHeight - 1));
            }
        }

        int triangleIndex = 0;
        for (int y = 0; y < meshHeight - 1; y++)
        {
            for (int x = 0; x < meshWidth - 1; x++)
            {
                int vertexIndex = y * meshWidth + x;

                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + meshWidth;
                triangles[triangleIndex + 2] = vertexIndex + meshWidth + 1;

                triangles[triangleIndex + 3] = vertexIndex;
                triangles[triangleIndex + 4] = vertexIndex + meshWidth + 1;
                triangles[triangleIndex + 5] = vertexIndex + 1;

                triangleIndex += 6;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}
