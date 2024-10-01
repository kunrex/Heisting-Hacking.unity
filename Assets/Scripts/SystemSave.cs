using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SystemSave : MonoBehaviour
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/GameData/";

    public static void SavePLayer(float t1, float t2, float t3)
    {
        if (!Directory.Exists(SAVE_FOLDER))
            Directory.CreateDirectory(SAVE_FOLDER);

        string path = SAVE_FOLDER + "/LevelData.txt";

        SaveData data = new SaveData(t1, t2, t3);

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }


    public static SaveData loadPlayer()
    {
        string path = SAVE_FOLDER + "/LevelData.txt";

        if (Directory.Exists(SAVE_FOLDER))
        {
            string saveString = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(saveString);
            return data;
        }
        else
            return null;
    }
}
