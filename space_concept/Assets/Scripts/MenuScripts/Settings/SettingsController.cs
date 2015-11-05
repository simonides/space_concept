using UnityEngine;
using System.Collections;
using Custom.Utility;
using Custom.Base;

public class SettingsController : SingletonBase<SettingsController> {
    private SettingsData dataFile;

    void Awake() {
        base.Awake(this);
        //at awake, check if settings.dat exists, if not, create it with default values
        if ((dataFile = SaveFileSerializer.Load<SettingsData>("Settings", "Settings.dat")) == null) {
            dataFile = new SettingsData();
            SaveFileSerializer.Save<SettingsData>(dataFile, "Settings", "Settings.dat");
            Debug.Log("Settings.dat file did not exist, so it was created");
        }
        if ((dataFile = SaveFileSerializer.XMLLoad<SettingsData>("Settings", "Settings.xml")) == null)
        {
            dataFile = new SettingsData();
            SaveFileSerializer.XMLSave<SettingsData>(dataFile, "Settings", "Settings.xml");
            Debug.Log("Settings.xml file did not exist, so it was created");
        }
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
