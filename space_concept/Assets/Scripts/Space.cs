﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/**
 * Space Object.
 * Is initialised with a Space Entity (whose properties should not be modified outside afterwards).
 * Contains display properties and is attached to a space object in the game.
 */
public class Space : MonoBehaviour {

    // ****    CONFIGURATION    **** //
    public const float BORDER_WIDTH = 0;            //Additional border on each side of the map which is added to the bounds. In this area are no planets
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

    }


    public void Init(SpaceData space) {
        spaceData = space;

        Rect bounds = space.bounds;
        bounds.xMin -= BORDER_WIDTH;
        bounds.yMin -= BORDER_WIDTH;
        bounds.xMax += BORDER_WIDTH;
        bounds.yMax += BORDER_WIDTH;

        foreach(PlanetData planet in spaceData.planets) {
            CreatePlanet(planet);
        }
        
        Debug.Log(bounds);

        // Initialise the background texture:
        background.transform.localPosition = new Vector3(bounds.xMin, bounds.yMin, 0);
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
    }




    // Creates a planet object (GameObject) of the given planet using the prefab and adds it to the space game object.
    void CreatePlanet(PlanetData planet) {
        Debug.Log("Creating planet object for planet \"" + planet.name + "\"");

        GameObject planetObject = Instantiate(planetPrefab) as GameObject;        //Create new planet
        planetObject.transform.parent = this.transform;                           //set as child of the map


        Planet planetScript = planetObject.GetComponent<Planet>();
        planetScript.Init(planet);

        planetObject.name = "Planet '" + planet.name + "'";
        planetObject.transform.position = planet.position;
        planetObject.transform.localScale = new Vector3(planet.diameter, planet.diameter, 1);       
        //TODO: set background sprite of planet here

        planets.Add(planetScript);
    }

    
    

    // Returns the size of the map
    public Vector2 GetSize() {
        return bounds.size;
    }


}
