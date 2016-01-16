using UnityEngine;
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

        InitialiseGame();
    }

    void Start() {
        MessageHub.Publish<PlanetUpdateEvent>(new PlanetUpdateEvent(this));     // Update graphical planet representations
    }


    void InitialiseGame() {
        //Clean up from the menu
        AnimatedBackgroundDontDestroy.TryDestroySingleton();

        Debug.Log("Initialising game...");
        try {
            if (SettingsController.GetInstance().loadMap == false) {
                InitialiseNewGame();
            } else {
                InitialiseGameFromSaveGame();
            }
        } catch (MissingComponentException e) {
            Debug.LogWarning("Failed to communicate with SettingsController. \n" + e.ToString());
            InitialiseNewGame();
        }
        Debug.Log("The Big Bang happened guys!");
    }


    void InitialiseNewGame() {
        Debug.Log("Generating new game...");
        GameStateData gameStateData = new GameStateData();
        gameState.Init(gameStateData);

        AirTrafficData airTrafficData = new AirTrafficData();
        airTrafficControl.Init(gameStateData.CurrentDay, airTrafficData);

        SpaceData spaceData = GenerateRandomMap();// GenerateDefaultMap();
        space.Init(spaceData);

        // Handling players...
        PlaceNewPlayersOnMap("Human Player", Color.red, SettingsController.GetInstance().kiCount); 
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
            aiData.Name = PlayerData.GetRandomPlayerName();
            aiData.Color = Color.blue;  //TODO: predefined list of colors
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
        //PlayerListData playerListData = new PlayerListData(new PlayerData(), new List<AiPlayer>());    // Todo: load from save game
        PlayerListData playerListData = saving.map.playerListData;
        PlaceExistingPlayersOnMap(playerListData);
    }


    void PlaceExistingPlayersOnMap(PlayerListData playerListData) {
        playerManager.Init(playerListData);
    }






    SpaceData GenerateDefaultMap() {
        SpaceData spaceData = new SpaceData();
        PlanetData planet = new PlanetData(new Vector2(0, 0), 50, 50, 10000, 100, true);
        planet.Name = "first";
        spaceData.AddPlanet(planet);

        planet = new PlanetData(new Vector2(400, -40), 120, 50, 10000, 100, true);
        planet.Name = "second";
        spaceData.AddPlanet(planet);

        planet = new PlanetData(new Vector2(-200, 480), 90, 50, 10000, 100, true);
        planet.Name = "third";
        spaceData.AddPlanet(planet);

        planet = new PlanetData(new Vector2(-500, 1000), 90, 50, 10000, 100, true);
        planet.Name = "four";
        spaceData.AddPlanet(planet);

        planet = new PlanetData(new Vector2(500, -1000), 90, 50, 10000, 100, true);
        planet.Name = "five";
        spaceData.AddPlanet(planet);

        planet = new PlanetData(new Vector2(3000, 150), 90, 50, 10000, 100, true);
        planet.Name = "six";
        spaceData.AddPlanet(planet);

        planet = new PlanetData(new Vector2(-1000, 800), 90, 50, 10000, 100, true);
        planet.Name = "seven";
        spaceData.AddPlanet(planet);

        return spaceData;
    }
    SpaceData GenerateRandomMap() {
        SpaceData spaceData = new SpaceData();
        List<PlanetData> planets = new List<PlanetData>();//hold the list of planets to check if they are correctly disributed
        PlanetData planet;
        int numberOfPlantes = Random.Range(10, 30);//get random number of planets
        float minMapSizeX = -1200f;
        float minMapSizeY = -700f;
        float maxMapSizeX = 1200f;
        float maxMapSizeY = 700f;

        float minPlanetDistance = 200f;
        Vector2 newPosition = new Vector2();
        bool SizeXSatisfied = false, SizeYSatisfied = false;
        bool canPositionPlanet = true;

        Debug.Log("Planets to gen " + numberOfPlantes);
        int loopCounter = 0;
        for (int i = 0; i < numberOfPlantes; i++) {
            if (loopCounter >= 10) {

                numberOfPlantes--;
                loopCounter = 0;
            }
            Debug.Log("I is : " + i);
            loopCounter++;
            canPositionPlanet = true;
            newPosition.x = Random.Range(minMapSizeX, maxMapSizeX);
            newPosition.y = Random.Range(minMapSizeY, maxMapSizeY);
            Debug.Log("Run the Pos gen");
            foreach (PlanetData p in planets) {
                if (Vector2.Distance(newPosition, p.Position) < minPlanetDistance) {
                    canPositionPlanet = false;
                    break;
                }
            }
            if (!canPositionPlanet) {
                Debug.Log("Reducing i");
                i--;
                loopCounter++;
            } else {
                Debug.Log("Actually generated Planet");

                planet = new PlanetData(new Vector2(newPosition.x, newPosition.y), Random.Range(30f, 100f), 50, 10000, 100, true);
                planets.Add(planet);
            }
        }
        Debug.Log("looped: " + loopCounter);
        PlanetData top = planets[0], bottom = planets[0], left = planets[0], right = planets[0];

        foreach (PlanetData p in planets) {
            if (top.Position.y < p.Position.y) {
                top = p;
            }
            if (bottom.Position.y > p.Position.y) {
                bottom = p;
            }
            if (left.Position.x > p.Position.x) {
                left = p;
            }
            if (right.Position.x < p.Position.x) {
                right = p;
            }
        }
        if (Mathf.Abs(top.Position.y - bottom.Position.y) >= minMapSizeY) {
            SizeYSatisfied = true;
        }
        if (Mathf.Abs(right.Position.y - left.Position.y) >= minMapSizeX) {
            SizeXSatisfied = true;
        }


        if (!SizeYSatisfied) {
            float restNeededDistance = minMapSizeY - Mathf.Abs(top.Position.y - bottom.Position.y) / 2;
            int index = planets.IndexOf(top);//.Position.y + restNeededDistance;
            top.Position = new Vector2(top.Position.x, top.Position.y + restNeededDistance);
            planets[index] = top;

            index = planets.IndexOf(bottom);//.Position.y + restNeededDistance;
            bottom.Position = new Vector2(bottom.Position.x, bottom.Position.y - restNeededDistance);
            planets[index] = bottom;
        }

        if (!SizeXSatisfied) {
            float restNeededDistance = minMapSizeY - Mathf.Abs(right.Position.x - left.Position.x) / 2;
            int index = planets.IndexOf(right);//.Position.y + restNeededDistance;
            right.Position = new Vector2(right.Position.x + restNeededDistance, right.Position.y);
            planets[index] = right;

            index = planets.IndexOf(left);//.Position.y + restNeededDistance;
            left.Position = new Vector2(left.Position.x - restNeededDistance, left.Position.y);
            planets[index] = left;
        }
        int planetNum = 1;
        foreach (PlanetData p in planets) {
            p.Name = "Planet: " + planetNum;
            spaceData.AddPlanet(p);
            planetNum++;
        }
        Debug.Log("PLANEtS GENERATED: " + planetNum);
        return spaceData;
    }

}
