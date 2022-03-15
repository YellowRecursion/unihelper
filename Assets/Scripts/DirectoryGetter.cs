using UnityEngine;
using System.IO;

public class DirectoryGetter
{
    public const string format = ".tds";
    private static string p = string.Empty;

    public static string Get()
    {
        /*if (Application.isMobilePlatform)
        {
            if (p == string.Empty)
            {
                try
                {
                    // Android/data/com.GreenBoxstudio.TanksDenfence/files
                    string[] sp = Application.persistentDataPath.Split('/');
                    foreach (var s in sp)
                    {
                        if (s == "Android")
                        {
                            break;
                        }
                        else
                        {
                            p += s + "/";
                        }
                    }
                    CreateGameFolder();
                    string pdp = Application.persistentDataPath + (p[p.Length - 1] == '/' ? "" : "/") + "tdData/";
                    if (Directory.Exists(pdp))
                    {
                        p = pdp;
                    }
                    return p;
                }
                catch
                {
                    Debug.LogError("DirectoryGetter: Cant get path");
                    p = Application.persistentDataPath;
                    CreateGameFolder();
                    return p;
                }
            }
            else
            {
                return p;
            }
        }
        else
        {
            p = Application.persistentDataPath;
            CreateGameFolder();
            return p;
        }*/

        if (string.IsNullOrEmpty(p))
        {
            p = Application.persistentDataPath;
            CreateGameFolder();
            Debug.Log("Data Directory: " + p);
        }
        
        return p;
    }
    private static void CreateGameFolder()
    {
        p = p + (p[p.Length - 1] == '/' ? "" : "/") + "tdData";
        if (!Directory.Exists(p))
        {
            DirectoryInfo di = Directory.CreateDirectory(p);
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
        }
    }
}
