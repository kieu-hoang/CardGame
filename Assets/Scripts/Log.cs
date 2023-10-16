using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;


[System.Serializable]
public class Log: MonoBehaviour
{
    public GameState gs;
    public static string file;

    public static string filePath;
    // Start is called before the first frame update
    void Start()
    {
        string name = DateTime.Now.ToString("yyyyMMddThhmmss");
        gs = AI.currentGame;
        file = name + ".txt";
        filePath = Path.Combine(Application.persistentDataPath, file);
        LoadData();
    }

    public void LoadData()
    {
        //string file = "save.txt";
        // string filePath = Path.Combine(Application.persistentDataPath, file);
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
        }
        //gs = JsonUtility.FromJson<GameState>(File.ReadAllText(filePath));
    }

    public void SaveData()
    {
        // string file = "save.txt";
        // string filePath = Path.Combine(Application.persistentDataPath, file);
        //string json = JsonUtility.ToJson(gs);
        string json = "Hello Trung!!!";
        File.WriteAllText(filePath, json);
        Debug.Log("File saved, at path" + filePath);
    }

    public static void SaveData(string a)
    {
        File.AppendAllText(filePath, a);
    }

    // Update is called once per frame
    // private void Update()
    // {
    //     if (Input.GetKeyUp(KeyCode.Space))
    //     {
    //         SaveData();
    //     }
    // }
}

