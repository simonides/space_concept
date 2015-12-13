using UnityEngine;
using System.Collections;
using System;

public class GameSaver : MonoBehaviour {
    Space space;
    AirTrafficControl airTrafficControl;
    GameState gameState;
    CollectedMapData mapData;

	// Use this for initialization
	void Awake () {
        MessageHub.Subscribe<SaveGameEvent>(SaveGame);	
        MessageHub.Subscribe<AutoSaveGameEvent>(AutoSaveGame);

        mapData = new CollectedMapData();

        space = GameObject.Find("Space").GetComponent<Space>();
        if (space == null)
        {
            throw new MissingComponentException("Unable to find Space. The big bang doesn't have enough space to happen. The 'Space' game object also needs to be added to the level and have the space script attached.");
        }

        airTrafficControl = GameObject.Find("AirTrafficControl").GetComponent<AirTrafficControl>();
        if (space == null)
        {
            throw new MissingComponentException("Unable to find AirTrafficControl. There can't be any troops flying around without an global AirTrafficControl GameObject that has an AirTrafficControl Script attached.");
        }

        gameState = GameObject.Find("2D_MainCam").GetComponent<GameState>();
        if (gameState == null)
        {
            throw new MissingComponentException("Unable to find GameState. The 'GameState' script needs to be attached to the same Gameobject as the BigBang.");
        }
	}

    public void SaveGame(SaveGameEvent event_)
    {
        Debug.Log("Save Game... " + event_.Content);
      
        var settingsController = SettingsController.GetInstance();
        mapData.gameStateData = gameState.gameStateData;//get the data
        mapData.airTrafficData = airTrafficControl.GetData();
        mapData.spaceData = space.GetData();

        settingsController.SaveGame<CollectedMapData>(mapData, "SaveGames", event_.Content);
    }

    public void AutoSaveGame(AutoSaveGameEvent event_)
    {
        Debug.Log("Autosave Game...");
        var settingsController = SettingsController.GetInstance();
        mapData.gameStateData = gameState.gameStateData;//get the data
        mapData.airTrafficData = airTrafficControl.GetData();
        mapData.spaceData = space.GetData();

        settingsController.SaveGame<CollectedMapData>(mapData, "SaveGames", "Autosave");

    }
}
