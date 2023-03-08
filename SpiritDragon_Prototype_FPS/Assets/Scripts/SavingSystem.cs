//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SavingSystem 
{
    public static void SavePlayer(playerController player)
    {
        BinaryFormatter formatter=new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.saveData";
        FileStream stream = new FileStream(path,FileMode.Create);
        playerData _Data = new playerData(player);

        formatter.Serialize(stream,_Data);
        stream.Close();
    }
    public static playerData PlayerLoad()
    {
        string path = Application.persistentDataPath + "/player.saveData";
        if (File.Exists(path))
        {
            BinaryFormatter formatter= new BinaryFormatter();
            FileStream stream= new FileStream(path,FileMode.Open);
            playerData data = formatter.Deserialize(stream) as playerData;
            stream.Close();
            return data;

        }
        else
        {
            Debug.LogError("Save File Not Found" + path);
            return null;
        }
    }
}
