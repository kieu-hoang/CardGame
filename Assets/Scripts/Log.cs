using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;


[System.Serializable]
public class Log: MonoBehaviour
{   
    public static string fileGameState = "gameState.txt";
    public static string fileOutput = "output.txt";
    public static string filePath1;
    public static string filePath2;
    // Start is called before the first frame update
    void Start()
    {
        // string name = DateTime.Now.ToString("yyyyMMddThhmmss");
        // file = name + ".txt";
        filePath1 = Path.Combine(Application.persistentDataPath, fileGameState);
        filePath2 = Path.Combine(Application.persistentDataPath, fileOutput);
        LoadData();
    }

    public void LoadData()
    {
        //string file = "save.txt";
        // string filePath = Path.Combine(Application.persistentDataPath, file);
        if (!File.Exists(filePath1))
        {
            File.WriteAllText(filePath1, "");
            Debug.Log("File saved, at path" + filePath1);
        }
        if (!File.Exists(filePath2))
        {
            File.WriteAllText(filePath2, "");
            Debug.Log("File saved, at path" + filePath2);
        }
        //gs = JsonUtility.FromJson<GameState>(File.ReadAllText(filePath));
    }

    public void SaveData()
    {
        // string file = "save.txt";
        // string filePath = Path.Combine(Application.persistentDataPath, file);
        //string json = JsonUtility.ToJson(gs);
        // string json = "Hello Trung!!!";
        // File.WriteAllText(filePath, json);
        // Debug.Log("File saved, at path" + filePath);
    }

    public static void SaveData1(string a)
    {
        File.AppendAllText(filePath1, a); //thêm vô file
    }

    public static void SaveData2(string a)
    {
        File.AppendAllText(filePath2, a); //thêm vô file
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

