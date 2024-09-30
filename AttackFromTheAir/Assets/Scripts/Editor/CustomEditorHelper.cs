using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class CustomEditorHelper
{
    private static Texture2D lineTexture;
    public static Texture2D LineTexture
    {
        get
        {
            if (lineTexture == null)
            {
                lineTexture = new Texture2D(1, 1);
                lineTexture.SetPixel(0, 0, new Color(37f / 255f, 37f / 255f, 37f / 255f));
                lineTexture.Apply();
            }

            return lineTexture;
        }
    }

    public static List<T> GetSubgridList<T>(List<List<T>> objects, int subgridSize, int subgridIndexOffsetX, int subgridIndexOffsetY)
    {
        var list = new List<T>();

        int startX = subgridIndexOffsetX * subgridSize;
        int startY = subgridIndexOffsetY * subgridSize;

        for (int i = 0; i < subgridSize; i++)
        {
            for (int k = 0; k < subgridSize; k++)
            {
                int x = startX + k;
                int y = startY + i;
                list.Add(objects[x][y]);
            }
        }

        return list;
    }

    public static List<T> GetSubgridList<T>(T[,] objects, int subgridSize, int subgridIndexOffsetX, int subgridIndexOffsetY)
    {
        var list = new List<T>();

        int startX = subgridIndexOffsetX * subgridSize;
        int startY = subgridIndexOffsetY * subgridSize;

        for (int i = 0; i < subgridSize; i++)
        {
            for (int k = 0; k < subgridSize; k++)
            {
                int x = startX + k;
                int y = startY + i;
                list.Add(objects[x, y]);
            }
        }

        return list;
    }

    public static List<List<T>> TransposeMatrix<T>(List<List<T>> matrix)
    {
        int rows = matrix.Count;
        int cols = matrix[0].Count;
        List<List<T>> transposed = new List<List<T>>();

        for (int j = 0; j < cols; j++)
        {
            List<T> newRow = new List<T>();
            for (int i = 0; i < rows; i++)
            {
                newRow.Add(matrix[i][j]);
            }
            transposed.Add(newRow);
        }

        return transposed;
    }

    public static T[,] TransposeMatrix<T>(T[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        T[,] transposed = new T[cols, rows];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                transposed[j, i] = matrix[i, j];
            }
        }

        return transposed;
    }

    public static bool DrawAddElementButton(string btnText)
    {
        Color bgColor = GUI.backgroundColor;

        GUI.backgroundColor = Color.grey;
        GUIStyle style2 = new GUIStyle(GUI.skin.button);
        style2.richText = true;

        bool isPressed = (GUILayout.Button("<b>" + btnText + "</b>", style2));
        GUI.backgroundColor = bgColor;
        return isPressed;
    }

    public static void DrawLine(int stize = 1)
    {
        GUIStyle lineStyle = new GUIStyle();
        lineStyle.normal.background = LineTexture;

        GUILayout.Space(3);
        GUILayout.BeginVertical(lineStyle);
        GUILayout.Space(1);
        GUILayout.EndVertical();
        GUILayout.Space(3);
    }

    public static void BeginBox(string boxTitle = "")
    {
        GUIStyle style = new GUIStyle("HelpBox");
        style.padding.left = 0;
        style.padding.right = 0;

        GUILayout.BeginVertical(style);

        if (!string.IsNullOrEmpty(boxTitle))
        {
            DrawBoldLabel(boxTitle);

            DrawLine();
        }
    }

    public static void EndBox()
    {
        GUILayout.EndVertical();
    }

    public static void DrawBoldLabel(string text)
    {
        EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
    }

    public static bool BeginFoldoutBox(string boxTitle)
    {
        GUIStyle style = new GUIStyle("HelpBox");
        style.padding.left = 15;
        style.padding.right = 0;

        GUILayout.BeginVertical(style);

        if (!string.IsNullOrEmpty(boxTitle))
        {
            bool wasExpanded = IsBoxExpanded(boxTitle);

            bool isExpanded = DrawBoldFoldout(wasExpanded, boxTitle);

            if (isExpanded)
            {
                //DrawLine();
            }

            if (wasExpanded != isExpanded)
            {
                if (isExpanded)
                {
                    SetBoxExpanded(boxTitle);
                }
                else
                {
                    SetBoxCollapsed(boxTitle);
                }
            }

            return isExpanded;
        }

        return true;
    }

    public static bool BeginSimpleFoldoutBox(string boxTitle)
    {
        if (!string.IsNullOrEmpty(boxTitle))
        {
            bool wasExpanded = IsBoxExpanded(boxTitle);
            bool isExpanded = DrawBoldFoldout(wasExpanded, boxTitle);

            if (wasExpanded != isExpanded)
            {
                if (isExpanded)
                {
                    SetBoxExpanded(boxTitle);
                }
                else
                {
                    SetBoxCollapsed(boxTitle);
                }
            }

            return isExpanded;
        }

        return true;
    }

    public static bool BeginSimpleFoldoutBox(string boxTitle, string toggleKey)
    {
        if (!string.IsNullOrEmpty(boxTitle))
        {
            bool wasExpanded = IsBoxExpanded(toggleKey);
            bool isExpanded = DrawBoldFoldout(wasExpanded, boxTitle);

            if (wasExpanded != isExpanded)
            {
                if (isExpanded)
                {
                    SetBoxExpanded(toggleKey);
                }
                else
                {
                    SetBoxCollapsed(toggleKey);
                }
            }

            return isExpanded;
        }

        return true;
    }

    public static bool BeginFoldoutBox(string boxTitle, string toggleKey)
    {
        GUIStyle style = new GUIStyle("HelpBox");
        style.padding.left = 15;
        style.padding.right = 0;

        GUILayout.BeginVertical(style);

        if (!string.IsNullOrEmpty(toggleKey))
        {
            bool wasExpanded = IsBoxExpanded(toggleKey);

            bool isExpanded = DrawBoldFoldout(wasExpanded, boxTitle);

            if (isExpanded)
            {
                //DrawLine();
            }

            if (wasExpanded != isExpanded)
            {
                if (isExpanded)
                {
                    SetBoxExpanded(toggleKey);
                }
                else
                {
                    SetBoxCollapsed(toggleKey);
                }
            }

            return isExpanded;
        }

        return true;
    }

    public static bool IsBoxExpanded(string key)
    {
        string[] editorExpandedBoxes = EditorPrefs.GetString("hb-toggle-on").Split(';');

        for (int i = 0; i < editorExpandedBoxes.Length; i++)
        {
            if (editorExpandedBoxes[i] == key)
            {
                return true;
            }
        }

        return false;
    }


    public static void SetBoxExpanded(string prefKey)
    {
        string boxExpandedStr = EditorPrefs.GetString("hb-toggle-on");

        if (!string.IsNullOrEmpty(boxExpandedStr))
        {
            boxExpandedStr += ";";
        }

        boxExpandedStr += prefKey;

        EditorPrefs.SetString("hb-toggle-on", boxExpandedStr);
    }

    public static void SetBoxCollapsed(string prefKey)
    {
        string[] editorExpandedBoxes = EditorPrefs.GetString("hb-toggle-on").Split(';');

        string expandName = "";

        for (int i = 0; i < editorExpandedBoxes.Length; i++)
        {
            if (editorExpandedBoxes[i] == prefKey)
            {
                continue;
            }

            if (!string.IsNullOrEmpty(expandName))
            {
                expandName += ";";
            }

            expandName += editorExpandedBoxes[i];
        }

        EditorPrefs.SetString("hb-toggle-on", expandName);
    }


    public static bool DrawBoldFoldout(bool isExpanded, string text)
    {
        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
        foldoutStyle.fontStyle = FontStyle.Bold;
        return EditorGUILayout.Foldout(isExpanded, text, foldoutStyle);
    }
    
}
