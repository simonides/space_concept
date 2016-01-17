using UnityEngine;
using System.Collections;
using System.IO;
using Custom.Utility;
using Custom.Base;

public class SettingsController : SingletonBase<SettingsController> {
    public SettingsData dataFile;
    public PlayerData playerFile;
    public CollectedMapData map = null;
    public bool loadMap = false;
    public string mapName = "";
    public bool generateRandomMap = false;

    public int planetCount = 5;
    public int kiCount = 1;
    public int fogDistance = 0;

    override protected void Awake() {
        base.Awake(this);
        //at awake, check if settings.dat exists, if not, create it with default values
        //if ((dataFile = SaveFileSerializer.Load<SettingsData>("Settings", "Settings.dat")) == null) {
        //    dataFile = new SettingsData();
        //    SaveFileSerializer.Save<SettingsData>(dataFile, "Settings", "Settings.dat");
        //    Debug.Log("Settings.dat file did not exist, so it was created");
        //}
        if ((dataFile = SaveFileSerializer.XMLLoad<SettingsData>("Settings", "Settings.xml")) == null)
        {
            dataFile = new SettingsData();
            SaveFileSerializer.XMLSave<SettingsData>(dataFile, "Settings", "Settings.xml");
            Debug.Log("Settings.xml file did not exist, so it was created");
        }
        if ((playerFile = SaveFileSerializer.XMLLoad<PlayerData>("Settings", "Player.xml")) == null)
        {
            playerFile = new PlayerData();
            SaveFileSerializer.XMLSave<PlayerData>(playerFile, "Settings", "Player.xml");
            Debug.Log("Player.xml file did not exist, so it was created");
        }
        
    }

	
    public void SaveData() {
        SaveFileSerializer.XMLSave<SettingsData>(dataFile, "Settings", "Settings.xml");
        SaveFileSerializer.XMLSave<PlayerData>(playerFile, "Settings", "Player.xml");
    }

    public void SaveGame<T>(T file, string directory, string filename)
         where T : class, new()
    {
        if (directory == "")
        {
            SaveFileSerializer.XMLSave<T>(file, filename + ".xml");
        }
        else {
            SaveFileSerializer.XMLSave<T>(file, directory, filename + ".xml");
        }
        
        SaveFileSerializer.XMLSave<PlayerData>(playerFile, "Settings", "Player.xml");
    }

    public bool LoadMap(string name) {
        Debug.Log("Now Loading: "+name);
        if ((map = SaveFileSerializer.XMLLoad<CollectedMapData>("SaveGames", name + ".xml")) != null)
        {
            loadMap = true;
            return true;
        }
        else {
            loadMap = false;
            return false;
          //  throw new FileNotFoundException(name + ".xml not found");
        }
        
    }
}
