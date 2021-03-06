﻿using UnityEngine;
using System.Collections;
using System;
using TinyMessenger;

public class GameSaver : MonoBehaviour {
    Space space;
    AirTrafficControl airTrafficControl;
    PlayerManager playerManager;
    GameState gameState;
    CollectedMapData mapData;

    TinyMessageSubscriptionToken SaveGameEventSubscription, AutoSaveGameEventSubscription;
    // Use this for initialization
    void Awake () {
        Debug.Assert(SaveGameEventSubscription == null && AutoSaveGameEventSubscription == null);
        SaveGameEventSubscription= MessageHub.Subscribe<SaveGameEvent>(SaveGame);
        AutoSaveGameEventSubscription = MessageHub.Subscribe<AutoSaveGameEvent>(AutoSaveGame);

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
        playerManager = GameObject.Find("PlayerManagement").GetComponent<PlayerManager>();
        if (playerManager == null)
        {
            throw new MissingComponentException("Unable to find playerManager.");
        }
	}

    void OnDestroy() {
        MessageHub.Unsubscribe<SaveGameEvent>(SaveGameEventSubscription);
        MessageHub.Unsubscribe<AutoSaveGameEvent>(AutoSaveGameEventSubscription);
    }

    public void SaveGame(SaveGameEvent event_)
    {
        Debug.Log("Save Game... " + event_.Content);
      
        var settingsController = SettingsController.GetInstance();
        mapData.gameStateData = gameState.gameStateData;//get the data
        mapData.airTrafficData = airTrafficControl.GetData();
        mapData.spaceData = space.GetData();
        mapData.playerListData = playerManager.PlayerListData;
        settingsController.SaveGame<CollectedMapData>(mapData, "SaveGames", event_.Content);
    }

    public void AutoSaveGame(AutoSaveGameEvent event_)
    {
        Debug.Log("Autosave Game...");
        var settingsController = SettingsController.GetInstance();
        mapData.gameStateData = gameState.gameStateData;//get the data
        mapData.airTrafficData = airTrafficControl.GetData();
        mapData.spaceData = space.GetData();
        mapData.playerListData = playerManager.PlayerListData;
        settingsController.SaveGame<CollectedMapData>(mapData, "SaveGames", "Autosave");

    }
}
