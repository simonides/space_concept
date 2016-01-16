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

    static List<string> predefinedPlanetNames;

    static public string GetUniqueRandomPlanetName() {
        int idx = Random.Range(0, predefinedPlanetNames.Count);
        string name = predefinedPlanetNames[idx];
        predefinedPlanetNames.RemoveAt(idx);
        return name;
    }

    public SpaceData() {
        planets = new List<PlanetData>();
        bounds = new Rect(0, 0, 0, 0);
        MaxPlayerCount = 0;
        predefinedPlanetNames = new List<string> {  "Aarseth", "Abdulla", "Adriana", "Asphaug", "Balaton", "Behaim", "Briggs", "Byrd", "Chaubal", "Cipolla", "Cyrus", "Decatur", "Dimpna", "Dumont",
                                                                    "Echnaton", "Elbrus", "Elisa", "Erminia", "Fanynka", "Figneria", "Frobel", "Galinskij", "Ganguly", "Giocasilli", "Granule", "Hanakusa", "Harada",
                                                                    "Hjorter", "Humecronyn", "Iglika", "Ikaunieks", "Isoda", "Jansky", "Kabtamu", "Kalinin", "Koikeda", "Landoni", "Lebedev", "Licitra", "Lyubov",
                                                                    "Miknaitis", "Namba", "Orchiston", "Pandion", "Penttila", "Quero", "Radmall", "Ruetsch", "Serra", "Shustov", "Siurana", "Smaklosa", "Szalay",
                                                                    "Tenmu", "Tietjen", "Trombka", "Tytgat", "Velichko", "Vulpius", "Wupatki", "Xanthus", "Yarilo", "Zajonc", "Zeissia", "Zykina" };
    }

    // Size of the whole map, starting at the most left/top planet to the most right/bottom planet inclusive their diameter.
    public Vector2 GetSize() {
        return bounds.size;
    }



    public void AddPlanet(PlanetData planet) {
        if(planet.Name == "") {
            planet.Name = GetUniqueRandomPlanetName();
        }
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

        UpdateBoundsWithNewPlanet(planet, planets.Count == 1);      
    }

    public void ForceBoundRecalculation() {
        bool first = true;
        foreach (PlanetData planet in planets) {
            UpdateBoundsWithNewPlanet(planet, first);
            first = false;
        }
    }

    void UpdateBoundsWithNewPlanet(PlanetData planet, bool isFirstPlanet) {
        Rect newMapBounds = bounds;

        Vector2 position = planet.Position;
        float diameter = planet.Diameter;

        if (position.x - diameter / 2 < bounds.xMin || isFirstPlanet) {
            newMapBounds.xMin = position.x - diameter / 2;
        }
        if (position.y - diameter / 2 < bounds.yMin || isFirstPlanet) {
            newMapBounds.yMin = position.y - diameter / 2;
        }
        if (position.x + diameter / 2 > bounds.xMax || isFirstPlanet) {
            newMapBounds.xMax = position.x + diameter / 2;
        }
        if (position.y + diameter / 2 > bounds.yMax || isFirstPlanet) {
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

    public TactileInformation GetTactileInformation(PlanetData origin) {
        List<TactilePlanet> enemies = new List<TactilePlanet>();
        List<TactilePlanet> friends = new List<TactilePlanet>();
        List<TactilePlanet> neutral = new List<TactilePlanet>();

        foreach (PlanetData planet in planets) {
            if (planet == origin) { continue; }
            if (planet.Owner == origin.Owner) {
                friends.Add(new TactilePlanet(planet, TroopData.GetTravelTime(origin, planet)));
            } else {
                enemies.Add(new TactilePlanet(planet, TroopData.GetTravelTime(origin, planet)));
            }
        }
        //enemies.Sort((TactilePlanet a, TactilePlanet b) => { return a.distance - b.distance; });
        //friends.Sort((TactilePlanet a, TactilePlanet b) => { return a.distance - b.distance; });
        //neutral.Sort((TactilePlanet a, TactilePlanet b) => { return a.distance - b.distance; });
        return new TactileInformation(origin, enemies, friends);
    }
}
