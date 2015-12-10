using UnityEngine;
using System.Collections;
using System;

public class GameSaver : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        MessageHub.Subscribe<SaveGameEvent>(SaveGame);	
        MessageHub.Subscribe<AutoSaveGameEvent>(AutoSaveGame);	
	}

    public void SaveGame(SaveGameEvent event_)
    {
        Debug.Log("Save Game... " + event_.Content);
    }

    void AutoSaveGame(AutoSaveGameEvent event_)
    {
        Debug.Log("Autosave Game...");
    }
}
