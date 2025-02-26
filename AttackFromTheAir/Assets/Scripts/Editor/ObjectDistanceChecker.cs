using UnityEditor;
using UnityEngine;

public static class ObjectDistanceChecker
{
    [MenuItem("Tools/CheckDistance")]
    public static void CheckDistance()
    {
        var selection = Selection.gameObjects;
        if (selection != null && selection.Length > 1)
        {
            for (int i = 0; i < selection.Length - 1; i++)
            {
                var firstObject = selection[i];
                var secondObject = selection[i + 1];
                var distance = Vector3.Distance(firstObject.transform.position, secondObject.transform.position);
                Debug.Log($"Distance between {firstObject.name} and {secondObject.name} is {distance}");
            }
        }
    }
}
