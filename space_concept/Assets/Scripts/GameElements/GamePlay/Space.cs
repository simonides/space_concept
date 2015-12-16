using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/**
 * Space Object.
 * Is initialised with a Space Entity (whose properties should not be modified outside afterwards).
 * Contains display properties and is attached to a space object in the game.
 */
public class Space : MonoBehaviour {

    // ****    CONFIGURATION    **** //
    public const float BORDER_WIDTH = 8;            //Additional border on each side of the map which is added to the bounds. In this area are no planets
    public GameObject planetPrefab;
    public Texture2D backgroundTexture;            

    // ****  ATTACHED OBJECTS   **** //
    SpriteRenderer backgroundRenderer;

    // ****                     **** //




    Transform background;
    
    SpaceData spaceData;        // entity
    List<Planet> planets;       // planet objects

    
    public Rect bounds { get; private set; }     // Size/bounds of the cosmos

    void Awake() {
        planets = new List<Planet>();

        background = transform.Find("Background");
        if (background == null) {
            throw new MissingComponentException("Unable to find Background. Game Component should be child object of Space Object.");
        }
        
        backgroundRenderer = background.GetComponent<SpriteRenderer>();
        if (backgroundRenderer == null) {
            throw new MissingComponentException("Unable to find SpriteRenderer on Background. The game object (child of space) should have a sprite renderer for the background texture.");
        }
        
        Sprite sprite = Sprite.Create(backgroundTexture, new Rect(0, 0, backgroundTexture.width, backgroundTexture.height), new Vector2(0, 0), 1f);    // default sprite to have something visible. origin (pivot) = corner
        backgroundRenderer.sprite = sprite;
    }

    void Start() {
        MessageHub.Subscribe<NextDayEvent>(NextDay);
    }


    public void Init(SpaceData space) {
        spaceData = space;

        Rect bounds = space.bounds;

        bounds.xMin -= BORDER_WIDTH;
        bounds.yMin -= BORDER_WIDTH;
        bounds.xMax += BORDER_WIDTH;
        bounds.yMax += BORDER_WIDTH;
        
        //Debug.Log(bounds);
        this.bounds = bounds;

        foreach (PlanetData planet in spaceData.planets) {
            CreatePlanet(planet);
        }
        

        // Initialise the background texture:
        background.transform.localPosition = new Vector3(bounds.xMin, bounds.yMin, 10);
        Vector2 scaling = new Vector2(bounds.width / backgroundTexture.width, bounds.height / backgroundTexture.height);    //scaling of the background to be as big as the map
        
        //Update the textureRect according to the aspect ratio:
        float mapRatio = scaling.x / scaling.y;  
        float backgroundRatio = backgroundTexture.width / backgroundTexture.height;

        Rect textureRect;
        //textureRect = new Rect(0, 0, backgroundTexture.width, backgroundTexture.height);
        if (mapRatio > backgroundRatio) {       // clip top/bottom of texture
            float height = backgroundTexture.height / mapRatio;
            scaling.y = scaling.x;
            textureRect = new Rect(0, (backgroundTexture.height - height) / 2, backgroundTexture.width, height);
        } else {                                // clip left/right
            float width = backgroundTexture.width * mapRatio;
            scaling.x = scaling.y;
            textureRect = new Rect((backgroundTexture.width - width) / 2, 0, width, backgroundTexture.height);
        }
        background.transform.localScale = new Vector3(scaling.x, scaling.y, 1);

        Sprite sprite = Sprite.Create(backgroundTexture, textureRect, new Vector2(0, 0), 1);    //origin (pivot) = corner
        backgroundRenderer.sprite = sprite;
       
        //translate the whole Space gameobject so that it's origin is the map's center:
        Vector2 center = bounds.center;
        transform.localPosition = new Vector3(-center.x, -center.y, 0);
    }
    

    // Creates a planet object (GameObject) of the given planet using the prefab and adds it to the space game object.
    void CreatePlanet(PlanetData planet) {
        Debug.Log("Creating planet object for planet \"" + planet.Name + "\"");

        GameObject planetObject = Instantiate(planetPrefab) as GameObject;        //Create new planet
        planetObject.transform.parent = this.transform;                           //set as child of the map


        Planet planetScript = planetObject.GetComponent<Planet>();
        planetScript.Init(planet);
        
        planets.Add(planetScript);
    }

    private void NextDay(NextDayEvent evt) {
        int totalShipsProduced = 0;
        foreach(Planet p in planets) {
            totalShipsProduced += p.planetData.ProduceShips();
        }
        Debug.Log(totalShipsProduced + " ships have been produced in total on all planets.");
        MessageHub.Publish<EvaluationRequestEvent>(new EvaluationRequestEvent(this, evt));
    }

    // Returns the size of the map
    public Vector2 GetSize() {
        return bounds.size;        
    }

    //returns the center of the bounds rect in local space from the origin that does not have to be the bottom left corner.. it can be everywhere -> depends on the map file
     public Vector2 GetCenter() {
        return bounds.center;
    }

     public SpaceData GetData()
     {
         return spaceData;
     }
}
