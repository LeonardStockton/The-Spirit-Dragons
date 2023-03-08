//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SavingSystem 
{
    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter=new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.saveData";
        FileStream stream = new FileStream(path,FileMode.Create);
        PlayerData _Data = new PlayerData(player);

        formatter.serialize(stream,_Data);
        stream.close();
    }
    public static playerData PlayerLoad()
    {
        string path = Application.persistentDataPath + "/player.saveData";
        if (fileExists(path))
        {
            BinaryFormatter formatter= new BinaryFormatter();
            FileStream stream= new FileStream(path,FileMode.Open);
            playerData data = formatter.Deserialize(stream) as playerData;
            stream.Close();
        }
        else
        {
            Debug.LogError("Save File Not Found" + path);
            return null;
        }
    }
}
