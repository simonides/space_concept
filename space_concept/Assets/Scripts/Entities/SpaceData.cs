using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/**
 *  Space Entity.
 *  Contains a list of planet entities and defines a single map.
 *  Can be serialised / deserialised to a file.
 */
public class SpaceData {

    public List<PlanetData> planets { get; private set; }
    public Rect bounds { get; private set; }     // Size/bounds of the cosmos



    public SpaceData() {
        planets = new List<PlanetData>();
        bounds = new Rect(0, 0, 0, 0);
    }


    // Size of the whole map, starting at the most left/top planet to the most right/bottom planet inclusive their diameter.
    public Vector2 GetSize() {
        return bounds.size;
    }


    public void AddPlanet(PlanetData planet) {
        foreach (PlanetData otherPlanet in planets) {
            if (otherPlanet.name.Equals(planet.name)) {          //ID's must be unique
                throw new UnityException("Unable to create planet. There is already a planet with the name " + planet.name);
            }
        }
        Debug.Log("Adding planet \"" + planet.name + "\"");

        planets.Add(planet);


        //Update mapBounds:
        Rect newMapBounds = bounds;

        Vector2 position = planet.position;
        float diameter = planet.diameter;

        if (position.x - diameter / 2 < bounds.xMin) {
            newMapBounds.xMin = position.x - diameter / 2;
        }
        if (position.y - diameter / 2 < bounds.yMin) {
            newMapBounds.yMin = position.y - diameter / 2;
        }
        if (position.x + diameter / 2 > bounds.xMax) {
            newMapBounds.xMax = position.x + diameter / 2;
        }
        if (position.y + diameter / 2 > bounds.yMax) {
            newMapBounds.yMax = position.y + diameter / 2;
        }
        bounds = newMapBounds;
    }

    public PlanetData GetPlanet(string name) {
        foreach (PlanetData planet in planets) {
            if (planet.name.Equals(name)) {          //ID's must be unique
                return planet;
            }
        }
        return null;
    }
}
