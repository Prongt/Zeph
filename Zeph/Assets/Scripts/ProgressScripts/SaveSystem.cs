using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Save system serializer and deserializer
/// </summary>
public static class SaveSystem
{
   public static void SaveLevel(LevelProgress curLevel)
   {
      BinaryFormatter formatter = new BinaryFormatter	();
      string path = Application.persistentDataPath + "/level.grannus";
      FileStream stream = new FileStream(path, FileMode.Create);

      LevelData level = new LevelData(curLevel);

      formatter.Serialize(stream, level);
      stream.Close();
   }

   public static LevelData LoadLevel()
   {
      string path = Application.persistentDataPath + "/level.grannus";
      if (File.Exists(path))
      {
         BinaryFormatter formatter = new BinaryFormatter();
         FileStream stream = new FileStream(path, FileMode.Open);

         LevelData level = formatter.Deserialize(stream) as LevelData;
         
         
         return level;
      }
      else
      {
         Debug.LogError("File doesn't exist at " + path);
         return null;
      }
   }
   
}
