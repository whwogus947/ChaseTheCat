using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Com2usGameDev
{
    public class FileDataService : IDataService
{
    ISerializer serializer;
    string dataPath;
    string fileExtension;

    public FileDataService(ISerializer serializer)
    {
        this.dataPath = Application.persistentDataPath;
        this.fileExtension = "json";
        this.serializer = serializer;
    }

    string GetPathToFile(string fileName)
    {
        return Path.Combine(dataPath, string.Concat(fileName, ".", fileExtension));
    }

    public void Save(FlotData data, bool overwrite = true)
    {
        string fileLocation = GetPathToFile(data.Name);

        if (!overwrite && File.Exists(fileLocation))
        {
            throw new IOException($"The file '{data.Name}.{fileExtension}' already exists and cannot be overwritten.");
        }

        File.WriteAllText(fileLocation, serializer.Serialize(data));
    }

    public List<string> GetFileNames()
    {
        List<string> jsonFiles = new();
        
        if (Directory.Exists(dataPath))
        {
            string[] files = Directory.GetFiles(dataPath, "*.json");
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                jsonFiles.Add(fileName);
            }
        }
        else
        {
            Debug.LogError("Directory does not exist: " + dataPath);
        }
        return jsonFiles;
    }

    public FlotData Load(string name)
    {
        string fileLocation = GetPathToFile(name);

        if (!File.Exists(fileLocation))
        {
            throw new ArgumentException($"No persisted FlotData with name '{name}'");
        }

        return serializer.Deserialize<FlotData>(File.ReadAllText(fileLocation));
    }

    public void Delete(string name)
    {
        string fileLocation = GetPathToFile(name);

        if (File.Exists(fileLocation))
        {
            File.Delete(fileLocation);
        }
    }

    public void DeleteAll()
    {
        foreach (string filePath in Directory.GetFiles(dataPath))
        {
            File.Delete(filePath);
        }
    }

    public IEnumerable<string> ListSaves()
    {
        foreach (string path in Directory.EnumerateFiles(dataPath))
        {
            if (Path.GetExtension(path) == fileExtension)
            {
                yield return Path.GetFileNameWithoutExtension(path);
            }
        }
    }
}
}
