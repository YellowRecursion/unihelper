using UnityEngine;

public class Encryption
{
    private static int deviceNumber = -1;

    public static string SimpleEncrypt(string s)
    {
        if (s == null) return s;
        string result = "";
        for (int i = 0; i < s.Length; i++)
        {
            result += (char)(s[i] + 100);
        }
        return result;
    }
    public static string SimpleDecode(string s)
    {
        if (s == null) return "null";
        if (s == "null") return "null";
        string result = "";
        for (int i = 0; i < s.Length; i++)
        {
            result += (char)(s[i] - 100);
        }
        return result;
    }

    public static string Encrypt(object obj)
    {
        if (obj == null) return null;
        GetDeviceNumber();
        string s = "";
        if (obj != null) s = obj.ToString();
        string result = "";
        result += (char)UnityEngine.Random.Range('a', 'z');
        for (int i = 0; i < s.Length; i++)
        {
            result += (char)(s[i] + deviceNumber);
        }
        result += UnityEngine.Random.Range(0, 9).ToString();
        return result;
    }

    public static string Decode(string s)
    {
        if (s == null) return "null";
        if (s == "null") return "null";
        GetDeviceNumber();
        string result = "";
        for (int i = 1; i < s.Length-1; i++)
        {
            result += (char)(s[i] - deviceNumber);
        }
        return result;
    }

    public static string Decode(string s, int customNumber)
    {
        if (s == null) return "null";
        if (s == "null") return "null";
        GetDeviceNumber();
        string result = "";
        for (int i = 1; i < s.Length - 1; i++)
        {
            result += (char)(s[i] - customNumber);
        }
        return result;
    }

    public static int GetDeviceNumber()
    {
        if (deviceNumber == -1)
        {
            string id = UnityEngine.SystemInfo.deviceModel;
            for (int i = 0; i < id.Length; i++)
            {
                deviceNumber += (int)id[i];
            }
            deviceNumber += 100;
            deviceNumber = System.Convert.ToInt32("" + deviceNumber.ToString()[0] + deviceNumber.ToString()[1]);
            deviceNumber += 60;
            deviceNumber *= 3;

            Debug.Log("Device number: " + deviceNumber);
        }
        return deviceNumber;
    }
}