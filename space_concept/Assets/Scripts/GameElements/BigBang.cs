using UnityEngine;
using System.Collections;
using Custom.Utility;
public class BigBang : MonoBehaviour {
    // ****    CONFIGURATION    **** //

    // ****  ATTACHED OBJECTS   **** //
    Space space;
    GameState gameState;
    // ****                     **** //




    void Awake() {
        space = GameObject.Find("Space").GetComponent<Space>();
        if (space == null) {
            throw new MissingComponentException("Unable to find Space. The big bang doesn't have enough space to happen. The 'Space' game object also needs to be added to the level and the space script attached.");
        }

        gameState = gameObject.GetComponent<GameState>();
        if (gameState == null) {
            throw new MissingComponentException("Unable to find GameState. The 'GameState' script needs to be attached to the same Gameobject as the BigBang.");
        }
        InitialiseGame();
    }



    


    // Use this for initialization
    void Start() {
    }


    void InitialiseGame() {
        Debug.Log("Initialising game...");
        if (SettingsController.GetInstance().loadMap == false) {
            InitialiseNewGame();
        } else {
            InitialiseGameFromSaveGame();
        }
        Debug.Log("The Big Bang happened guys!");
    }


    void InitialiseGameFromSaveGame() {
        Debug.Log("Loading save game...");

        //TODO: load GameStateData too...

        SpaceData spaceData = SettingsController.GetInstance().map;        
        SaveFileSerializer.XMLSave<SpaceData>(spaceData, "SaveGames", "Autosave.xml");
        
        space.Init(spaceData);
    }

    void InitialiseNewGame() {
        Debug.Log("Generating new game...");
        GameStateData gameStateData = new GameStateData();
        gameState.Init(gameStateData);

        SpaceData spaceData = GenerateDefaultMap();
        space.Init(spaceData);
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

        planet = new PlanetData(new Vector2(-500, 250), 90, 50, 10000, 100, true);
        planet.Name = "four";
        spaceData.AddPlanet(planet);

        planet = new PlanetData(new Vector2(500, -90), 90, 50, 10000, 100, true);
        planet.Name = "five";
        spaceData.AddPlanet(planet);

        planet = new PlanetData(new Vector2(1000, 150), 90, 50, 10000, 100, true);
        planet.Name = "six";
        spaceData.AddPlanet(planet);

        planet = new PlanetData(new Vector2(-20, 800), 90, 50, 10000, 100, true);
        planet.Name = "seven";
        spaceData.AddPlanet(planet);

        return spaceData;
    }
}
