using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/**
 *  Space Entity.
 *  Contains a list of planet entities and defines a single map.
 *  Can be serialised / deserialised to a file.
 */
[System.Serializable]
public class SpaceData {

    public List<PlanetData> planets { get; private set; }
    public Rect bounds { get; private set; }     // Size/bounds of the cosmos
    public int MaxPlayerCount { get; private set; }     // The maximum number of players on this map. Defined by the number of start planets in this map.


    public SpaceData() {
        planets = new List<PlanetData>();
        bounds = new Rect(0, 0, 0, 0);
        MaxPlayerCount = 0;
    }


    // Size of the whole map, starting at the most left/top planet to the most right/bottom planet inclusive their diameter.
    public Vector2 GetSize() {
        return bounds.size;
    }


    public void AddPlanet(PlanetData planet) {
        foreach (PlanetData otherPlanet in planets) {
            if (otherPlanet.Name.Equals(planet.Name)) {          //ID's must be unique
                throw new UnityException("Unable to create planet. There is already a planet with the name " + planet.Name);
            }
        }
        Debug.Log("Adding planet \"" + planet.Name + "\"");

        planets.Add(planet);
        if(planet.IsStartPlanet) {
            ++MaxPlayerCount;
        }

        //Update mapBounds:
        Rect newMapBounds = bounds;

        Vector2 position = planet.Position;
        float diameter = planet.Diameter;

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
            if (planet.Name.Equals(name)) {          //ID's must be unique
                return planet;
            }
        }
        return null;
    }

    public int getPlanetCount() {
        return planets.Count;
    }
}
