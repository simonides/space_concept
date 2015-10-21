using UnityEngine;
using System.Collections;

public class BigBang : MonoBehaviour {

    Space space;


    void Awake() {
        space = GameObject.Find("Space").GetComponent<Space>();
        if (space == null) {
            throw new MissingComponentException("Unable to find Space. The big bang doesn't have enough space to happen. The 'Space' game object also needs to be added to the level and the space script attached.");
        }
    }

	// Use this for initialization
	void Start () {
        SpaceData spaceData = new SpaceData();
        PlanetData planet = new PlanetData(new Vector2(0,0), 50, 50, 10000, 100);
        planet.name = "first";
        spaceData.AddPlanet(planet);
        
        //planet = new PlanetData(new Vector2(400, -40), 70, 50, 10000, 100);
        //planet.name = "second";
        //spaceData.AddPlanet(planet);

        //planet = new PlanetData(new Vector2(-200, 80), 70, 50, 10000, 100);
        //planet.name = "third";
        //spaceData.AddPlanet(planet);


        space.Init(spaceData);


        Debug.Log("The Big Bang happened guys!");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
