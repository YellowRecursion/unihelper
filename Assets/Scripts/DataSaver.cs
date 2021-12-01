using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataSaver
{
    private static string path;
    private static string format;

    private static StreamReader streamReader;
    private static StreamWriter streamWriter;

    private static bool inited;
    private static bool fileOpened;
    private static bool writeMode;


    private static void Init()
    {
        if (Application.isEditor)
            path = Path.Combine(DirectoryGetter.Get(), $"DATA_EDITOR_{Encryption.GetDeviceNumber()}");
        else
            path = Path.Combine(DirectoryGetter.Get(), $"DATA_NOTEDITOR_{Encryption.GetDeviceNumber()}");

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        format = DirectoryGetter.format;
        inited = true;
    }

    public static void OpenToWrite(string file, bool append)
    {
        Open(file, true, append);
    }
    public static void OpenToWrite(string file)
    {
        Open(file, true, false);
    }
    public static bool OpenToRead(string file)
    {
        return Open(file, false, false);
    }
    private static bool Open(string file, bool write, bool append)
    {
        if (!inited) Init();
        if (fileOpened) 
        {
            Debug.LogError("DataSaver > Open: file is already open");
            return false;
        }

        writeMode = write;
        string p = Path.Combine(path, Encryption.SimpleEncrypt(file) + format);

        if (writeMode)
        {
            streamWriter = new StreamWriter(p, append);
        }
        else
        {
            if (!File.Exists(p))
            {
                return false;
            }
            streamReader = new StreamReader(p);
        }

        fileOpened = true;
        return true;
    }

    public static bool EndOfRead()
    {
        return streamReader.EndOfStream;
    }

    public static void Close()
    {
        if (!fileOpened)
        {
            Debug.LogError("DataSaver > Close: file not open");
            return;
        }
        fileOpened = false;
        if (writeMode)
        {
            streamWriter.Close();
        }
        else
        {
            streamReader.Close();
        }
    }

    public static void WriteLine(object obj)
    {
        if (!fileOpened)
        {
            Debug.LogError("DataSaver > WriteLine: file not open");
            return;
        }
        if (!writeMode)
        { 
            Debug.LogError("DataSaver > WriteLine: write mode disabled"); 
            return;
        }
        if (obj != null)
            streamWriter.WriteLine(Encryption.Encrypt(obj.ToString()));
        else
            streamWriter.WriteLine(Encryption.Encrypt(null));
    }

    public static string ReadLine()
    {
        if (streamReader.EndOfStream)
        {
            Debug.LogError("DataSaver > ReadLine: EndOfStream");
            //RepairSaves();
            return "";
        }
        return Encryption.Decode(streamReader.ReadLine());
    }

    public static int ReadLineInt()
    {
        int value = 0;
        try
        {
            value = System.Convert.ToInt32(Encryption.Decode(streamReader.ReadLine()));
        }
        catch
        {
            RepairSaves();
        }
        return value;
    }

    public static int ReadLineInt(int min, int max)
    {
        int value = ReadLineInt();
        if (value < min || value > max)
        {
            RepairSaves();
        }
        return value;
    }

    public static bool ReadLineBool()
    {
        bool value = false;
        try
        {
            value = System.Convert.ToBoolean(Encryption.Decode(streamReader.ReadLine()));
        }
        catch
        {
            RepairSaves();
        }
        return value;
    }

    public static float ReadLineFloat()
    {
        float value = 0;
        try
        {
            value = System.Convert.ToSingle(Encryption.Decode(streamReader.ReadLine()));
        }
        catch
        {
            RepairSaves();
        }
        return value;
    }

    public static System.DateTime ReadLineDateTime()
    {
        System.DateTime value = System.DateTime.MinValue;
        try
        {
            value = System.DateTime.Parse(Encryption.Decode(streamReader.ReadLine()));
        }
        catch
        {
            RepairSaves();
        }
        return value;
    }

    private static void RepairSaves()
    {
        if (fileOpened) Close();
        Debug.LogError("DataSaver: The save is broken");
        //GameData.RepairGame();
    }
}
