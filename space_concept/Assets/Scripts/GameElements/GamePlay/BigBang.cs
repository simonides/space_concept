﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Custom.Utility;
public class BigBang : MonoBehaviour {
    // ****    CONFIGURATION    **** //

    // ****  ATTACHED OBJECTS   **** //
    Space space;
    AirTrafficControl airTrafficControl;
    GameState gameState;
    PlayerManager playerManager;
    // ****                     **** //



    void Awake() {
        space = GameObject.Find("Space").GetComponent<Space>();
        if (space == null) {
            throw new MissingComponentException("Unable to find Space. The big bang doesn't have enough space to happen. The 'Space' game object also needs to be added to the level and have the space script attached.");
        }

        airTrafficControl = GameObject.Find("AirTrafficControl").GetComponent<AirTrafficControl>();
        if (airTrafficControl == null) {
            throw new MissingComponentException("Unable to find AirTrafficControl. There can't be any troops flying around without an global AirTrafficControl GameObject that has an AirTrafficControl Script attached.");
        }

        gameState = gameObject.GetComponent<GameState>();
        if (gameState == null) {
            throw new MissingComponentException("Unable to find GameState. The 'GameState' script needs to be attached to the same Gameobject as the BigBang.");
        }

        playerManager = GameObject.Find("PlayerManagement").GetComponent<PlayerManager>();
        if (playerManager == null) {
            throw new MissingComponentException("Unable to find PlayerManager. This script has to be attached to the global PlayerManagement game object.");
        }

    }

    void Start() {
        InitialiseGame();
        CheckForGameEnd();
        MessageHub.Subscribe<TroopEvaluationResultEvent>((TroopEvaluationResultEvent evt) => CheckForGameEnd());     // Check for game end
    }


    void InitialiseGame() {
        //Clean up from the menu
        AnimatedBackgroundDontDestroy.TryDestroySingleton();

        Debug.Log("Initialising game...");
        try {
            if (SettingsController.GetInstance().loadMap == false) {
                InitialiseNewGame(SettingsController.GetInstance().generateRandomMap);                
            } else {
                InitialiseGameFromSaveGame();
            }
        } catch (MissingComponentException e) {
            Debug.LogWarning("Failed to communicate with SettingsController. \n" + e.ToString());
            InitialiseNewGame(true);
        }
        MessageHub.Publish<PlanetUpdateEvent>(new PlanetUpdateEvent(this));     // Update graphical planet representations
        Debug.Log("Player name: '" + playerManager.PlayerListData.HumanPlayer.Name + "', color: '" + playerManager.PlayerListData.HumanPlayer.Color.ToString() + "'");
        Debug.Log("The Big Bang happened guys!");
    }


    void InitialiseNewGame(bool generateRandomMap) {
        Debug.Log("Generating new game...");
        GameStateData gameStateData = new GameStateData();
        gameState.Init(gameStateData);

        AirTrafficData airTrafficData = new AirTrafficData();
        airTrafficControl.Init(gameStateData.CurrentDay, airTrafficData);

        SpaceData spaceData;
        if (generateRandomMap) {
            spaceData = MapGenerator.GenerateRandomMap(SettingsController.GetInstance().planetCount);
        } else {
            spaceData = MapGenerator.GenerateDefaultMap();
        }
        space.Init(spaceData);

        // Handling players...
        //TODO: use color and name from start szene
        string playerName = "Human Player";
        Color playerColor = Color.black;
        PlaceNewPlayersOnMap(playerName, playerColor, SettingsController.GetInstance().kiCount); 
    }



    void PlaceNewPlayersOnMap(string humanPlayerName, Color humanPlayerColor, int aiCount) {
        PlayerData humanPlayer = new PlayerData();
        humanPlayer.Name = humanPlayerName;
        humanPlayer.Color = humanPlayerColor;
        Planet home = space.getRandomEmptyStartPlanet();
        if (home == null) {
            Debug.LogError("Failed to set home planet for human player. There are no start planets on the map");
        }
        home.planetData.SetOwner(humanPlayer);

        var aiPlayers = new List<AiPlayer>();
        for (int i = 0; i < aiCount; ++i) {
            PlayerData aiData = new PlayerData();
            AiPlayer player = new AiPlayer(aiData, space);
          
            home = space.getRandomEmptyStartPlanet();
            if (home == null) {
                Debug.LogError("Failed to set home planet for ai. There are no start planets on the map");
                break;
            }
            aiPlayers.Add(player);
            home.planetData.SetOwner(aiData);
        }

        PlayerListData playerListData = new PlayerListData(humanPlayer, aiPlayers);
        playerManager.Init(playerListData);
    }

    void InitialiseGameFromSaveGame() {
        Debug.Log("Loading save game...");
        var saving = SettingsController.GetInstance();

        GameStateData gameStateData = saving.map.gameStateData;
        gameState.Init(gameStateData);

        AirTrafficData airTrafficData = saving.map.airTrafficData;
        airTrafficControl.Init(gameStateData.CurrentDay, airTrafficData);

        SpaceData spaceData = saving.map.spaceData;
        space.Init(spaceData);

        // Handling players...
        PlayerListData playerListData = saving.map.playerListData;
        PlaceExistingPlayersOnMap(playerListData);
    }


    void PlaceExistingPlayersOnMap(PlayerListData playerListData) {
        playerManager.Init(playerListData);
    }







    // checks if the game has been finished
    public void CheckForGameEnd() {
        WinnerData winnerData = CheckForGameEnd(gameState.gameStateData.CurrentDay);
        if (winnerData == null) {
            return;
        }
        MessageHub.Publish<GameFinishedEvent>(new GameFinishedEvent(this, winnerData));
    }

    WinnerData CheckForGameEnd(int currentDay) {
        var playerList = playerManager.PlayerListData;

        if (playerList.HumanPlayer.GetNumberOfOwnedPlanets() == 0) {
            if (!airTrafficControl.airTrafficData.DoesPlayerHaveSomeFlyingTroops(playerList.HumanPlayer)) {
                return new WinnerData(currentDay, playerList);    // Player died
            }
        }
        foreach (AiPlayer ai in playerList.AiPlayers) {
            if (ai.playerData.GetNumberOfOwnedPlanets() > 0) {
                return null;
            }
            if (airTrafficControl.airTrafficData.DoesPlayerHaveSomeFlyingTroops(ai.playerData)) {
                return null;
            }
        }
        return new WinnerData(currentDay, playerList);    // No AI survived
    }
    
}
