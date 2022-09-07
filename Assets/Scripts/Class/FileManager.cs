using System;
using System.IO;
using UnityEngine;

namespace Class
{
    public class FileManager
    {
        public static string saveFileName = "SaveData.dat";
        public static string saveFileFullPath = Path.Combine(Application.persistentDataPath, saveFileName);

        public static bool IsSaveFileExist()
        {
            return File.Exists(saveFileFullPath);
        }

        public static bool WriteToSaveFile(string data)
        {
            return WriteToFile(saveFileFullPath, data);
        }

        public static string ReadFromSaveFile()
        {
            string result;
            ReadFromFile(saveFileFullPath, out result);
            return result;
        }

        public static bool DeleteSaveFile()
        {
            if (File.Exists(saveFileFullPath))
            {
                try
                {
                    File.Delete(saveFileFullPath);
                    return true;
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to save data to {saveFileFullPath} with exception {e}");
                    return false;
                }
            }

            return true;
        }

        public static bool WriteToFile(string filename, string data)
        {
            string fullPath = Path.Combine(Application.persistentDataPath, filename);
            try
            {
                File.WriteAllText(fullPath, data);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save data to {fullPath} with exception {e}");
                return false;
            }
        }

        public static bool ReadFromFile(string filename, out string result)
        {
            string fullPath = Path.Combine(Application.persistentDataPath, filename);
            try
            {
                result = File.ReadAllText(fullPath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to read data from, {fullPath} with exception {e}");
                result = "";
                return false;
            }
        }
    }
}