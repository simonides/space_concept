using UnityEngine;
using System.Collections;

public class BigBang : MonoBehaviour {

    Space space;


    void Awake() {
        space = GameObject.Find("Space").GetComponent<Space>();
        if (space == null) {
            throw new MissingComponentException("Unable to find Space. The big bang doesn't have enough space to happen. The 'Space' game object also needs to be added to the level and the space script attached.");
        }
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

        space.Init(spaceData);
    }

	// Use this for initialization
	void Start () {
       


        Debug.Log("The Big Bang happened guys!");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
