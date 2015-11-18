using UnityEngine;
using System.Collections;
using System.IO;
using Custom.Utility;
using Custom.Base;

public class SettingsController : SingletonBase<SettingsController> {
    public SettingsData dataFile;
    public PlayerData playerFile;
    public SpaceData map = null;
    public bool loadMap = false;
    void Awake() {
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

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SaveData() {
        SaveFileSerializer.XMLSave<SettingsData>(dataFile, "Settings", "Settings.xml");
        SaveFileSerializer.XMLSave<PlayerData>(playerFile, "Settings", "Player.xml");
    }

    public bool LoadMap(string name) {
        if ((map = SaveFileSerializer.XMLLoad<SpaceData>("SaveGames", name + ".xml")) != null)
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
