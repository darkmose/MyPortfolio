using Core.Utilities;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public static class DeleteSaveHelper
{
    private static bool IsMacOS => SystemInfo.operatingSystem.Contains("Mac OS");
    private static bool IsWinOS => SystemInfo.operatingSystem.Contains("Windows");


    [MenuItem("Tools/ClearAllSaves", false, 2)]
    public static void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
        File.Delete(Application.persistentDataPath + "/" + "/playerData.json");
        Debug.Log($"[Time: {DateTime.Now }] [Delete All PlayerPrefs and File Saves]");
    }

    [MenuItem("Tools/Open Saves", false, 3)]
    public static void OpenSaveFolder()
    {
        Debug.Log($"Path : {Application.persistentDataPath}");
        Open(Application.persistentDataPath);
    }

    public static void Open(string path)
    {
        if (IsWinOS == true)
        {
            OpenInWin(path);
        }
        else if (IsMacOS == true)
        {
            OpenInMac(path);
        }
        else
        {
           Debug.LogWarning($"[{nameof(DeleteSaveHelper)}] OS is not supported");
        }
    }

    private static void OpenInMac(string path)
    {
        bool openInsideFolder = false;
        string macPath = path.Replace("\\", "/");

        if (Directory.Exists(macPath) == true)
        {
            openInsideFolder = true;
        }

        if (macPath.StartsWith("\"") == false)
        {
            macPath = "\"" + macPath;
        }

        if (macPath.EndsWith("\"") == false)
        {
            macPath += "\"";
        }

        string arguments = (openInsideFolder ? "" : "-R ") + macPath;

        System.Diagnostics.Process.Start("open", arguments);
    }

    private static void OpenInWin(string path)
    {
        bool openInsideFolder = false;
        string winPath = path.Replace("/", "\\");

        if (Directory.Exists(winPath) == true)
        {
            openInsideFolder = true;
        }

        System.Diagnostics.Process.Start("explorer.exe", (openInsideFolder ? "/root," : "/select,") + winPath);
    }
}
#endif