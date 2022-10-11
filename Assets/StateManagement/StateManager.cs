using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityEngine.Renderring.PostProcessing;
public class StateManager
{
    public static string PATH = Application.persistentDataPath+"/saves/";
    public static string DEFAULTS = Application.persistentDataPath+"/default/";
    // void Start(){
    //     Debug.Log("i run");
    //     SaveAsJson("newshit.dat");
    //     GetSaves();
    // }

    public static void SaveAsJson(string file_name, bool newGame=true,LevelData data = null){
        // Not a serious function for saving games... (can be used for saving new games)
        data = new LevelData();
        data.chapter = 1;
        data.name= "save2020";
        data.date= System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
        data.path = Path.Combine(PATH, file_name);
        data.isNew = true;
        WriteToFile(file_name, data.ToJson());
    }
    public static void SaveLevelData(LevelData data, string file_name){
        
        data.path = Path.Combine(PATH, file_name);
        WriteToFile(file_name, data.ToJson());
        WriteToFile("current.dat", data.ToJson(),Path.Combine(DEFAULTS)+"current.dat");
    }
    public static bool WriteToFile(string file_name, string data, string customPath = null){
        var fullPath = Path.Combine(PATH, file_name);
        if (customPath!=null){
            fullPath = customPath;
        }
        CheckDirs();
        
        try{
            File.WriteAllText(fullPath, data);
            return true;
        }catch (Exception e){
            Debug.LogError($"Failed to write to {fullPath} with exception {e}");
        }
        return false;
   
    }
    public static bool LoadFromFile(string file_name, out string result, bool path_only = false){
        var fullPath = Path.Combine(PATH, file_name);
        if (path_only){
            fullPath = file_name;
        }
        

        try{
            result = File.ReadAllText(fullPath);
            return true;
        }catch (Exception e){
            Debug.LogError($"Failed to read from {fullPath} with exception {e}");
            result ="";
            return false;
        }
    }

    
    private static SaveData PopulateJson(){
            return new SaveData();
    }
    public static void LoadState(string PATH, bool isNew = false){
        // LOAD SCENE HERE
        try{
        string json = "";
        LoadFromFile(PATH, out json, true);
        LevelData data = new LevelData ();
        data.LoadFromJson(json);

        SceneManager.LoadScene(data.chapter);
        }catch(Exception e){
            Debug.Log("Error loading state");
        }
    }
    public static LevelData[] GetSaves(){
        try{
            string [] files = System.IO.Directory.GetFiles(PATH);
            List<string> savePaths = new List<string>();
            foreach (string file in files)
            {
                string [] split = file.Split('.');
                if(split[split.Length - 1] == "dat"){
                    savePaths.Add(file);
                }
            }
            LevelData [] saves = new LevelData[savePaths.Count];
            
            for(int i = 0 ; i < savePaths.Count;i++){
                LevelData newData = new LevelData();
                string json;
                LoadFromFile(savePaths[i], out json);
                newData.LoadFromJson(json);
                saves[i] = newData;
            }
            return saves;
        }catch (Exception e){
            Debug.Log("Error getting data from saves folder");
            return new LevelData[]{};
        }


    }
    public static void DeleteState(string file_path){
       
        try{
            File.Delete(file_path);
        }catch(Exception e){
            Debug.Log("Error deleting the file at given path");
        }
    }
    public static void CheckDirs(){
        // check dirs, if they dont exist make them
         try
        {
            if (!Directory.Exists(PATH))
            {
                Directory.CreateDirectory(PATH);
            }
            if (!Directory.Exists(DEFAULTS))
            {
                Directory.CreateDirectory(DEFAULTS);
            }
        
        }
        catch (IOException ex)
        {
            Debug.Log(ex.Message);
        }
    }
    public static void ResumeState(){
        LoadState(Path.Combine(DEFAULTS+"current.dat"));
    }
    public static void StartNew(int diff, int chapter = 1){
        LevelData data = new LevelData();
        data.chapter = chapter;
        data.name= "new_game";
        data.date= System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
        data.path = Path.Combine(DEFAULTS)+"current.dat";
        data.isNew = true;
        data.difficulty = diff;
        WriteToFile("current.dat", data.ToJson(),Path.Combine(DEFAULTS)+"current.dat");
        SceneManager.LoadScene(chapter);
    }
    public static LevelData LoadCurrent(){
        try {
        string json = "";
        LoadFromFile(Path.Combine(DEFAULTS)+"current.dat", out json, true);
        LevelData data = new LevelData();
        data.LoadFromJson(json);
        return data;
        }
        catch{
            return null;
        }
    }
}
