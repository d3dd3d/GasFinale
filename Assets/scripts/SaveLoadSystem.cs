using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoadSystem
{
    public static string fileName;
    public static int width1 = 12;
    public static int width2 = 3;
    public static int height = 8;
    public static readonly string SAVE_PATH = Application.dataPath + "/Saves/";

    public static void init() {
        if (!Directory.Exists(SAVE_PATH)) {
            Directory.CreateDirectory(SAVE_PATH);
        }
    }

    public static void save(string filename, string saveString)
    {
        // if(!File.Exists(SAVE_PATH + filename))
        // {
        //     File.Create(SAVE_PATH + filename);
        // }
        
        File.WriteAllText(SAVE_PATH + filename, saveString);
    }

    public static string load(string filename)
    {
        if(File.Exists(SAVE_PATH + filename))
        {
            return File.ReadAllText(SAVE_PATH + filename);
        } else
        {
            return null;
        }
    }
    public static List<string> loadFiles()
    {
        string[] lols =  Directory.GetFiles(SAVE_PATH);
        List<string> lols2 = new List<string>();
        for (int i = 0;i<lols.Length;i++){
            if (!lols[i].EndsWith(".meta"))
                lols2.Add(lols[i].Substring(lols[i].LastIndexOf('/')+1));
        }
        return lols2;
    }
}