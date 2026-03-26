using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class SaveSystem
{
    public static void SavePlayer (PrototypePlayerMovementControls player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerControllerData data = new PlayerControllerData(player);


        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerControllerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.save";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream (path, FileMode.Open);

            PlayerControllerData data = binaryFormatter.Deserialize(stream) as PlayerControllerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" +  path);
            return null;
        }
    }
}
