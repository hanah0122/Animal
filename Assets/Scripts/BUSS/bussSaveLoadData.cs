using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class bussSaveLoadData
{
    private static bool doing = false;
    private static string path = Application.persistentDataPath + "/game.data";

    public static void saveGame(bussGame game, float _startTime, int timeMenu)
    {
        if (!doing)
        {
            doing = true;
            try
            {
                // File.Delete(path);
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, File.Exists(path) ? FileMode.Truncate : FileMode.CreateNew);
                bussData data = new bussData(game, _startTime, timeMenu);
                formatter.Serialize(stream, data);
                stream.Close();
                doing = false;
            }
            catch
            {
            }
        }
    }

    public static bussData loadGame()
    {
        string path = Application.persistentDataPath + "/game.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            bussData data = formatter.Deserialize(stream) as bussData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }

    public static bool CheckData()
    {
        return File.Exists(path);
    }

    public static bool DelFile()
    {
        try
        {
            File.Delete(path);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
