using UnityEngine;
using System.Collections;

public class BigBang : MonoBehaviour {

    Space space;


    void Awake() {
        space = GameObject.Find("Space").GetComponent<Space>();
        if (space == null) {
            throw new MissingComponentException("Unable to find Space. The big bang doesn't have enough space to happen. The 'Space' game object also needs to be added to the level and the space script attached.");
        }
        Debug.Log("HIER");
    }

	// Use this for initialization
	void Start () {
        SpaceData spaceData = new SpaceData();
        PlanetData planet = new PlanetData(new Vector2(0,0), 100, 50, 10000, 100);
        spaceData.AddPlanet(planet);
        space.Init(spaceData);


        Debug.Log("The Big Bang happened guys!");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
