using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 *  Planet Entity.
 *  Contains all information about a single planet.
 *  Can be serialised / deserialised to a file.
 */
public class PlanetData {

    //TODO: user class
    private Object owner = null;                        // The player who owns this planet

    public string name { get; set; }                    // Name of the planet

    public Vector2 position { get; set; }

    float _diameter;
    public float diameter {
        get { return _diameter; }
        set {
            if (diameter < 0) { throw new UnityException("Invalid diameter: Must be greater than zero"); }
            _diameter = value;
        }
    }

    int _ships;
    public int ships {
        get { return _ships; }
        set {
            if (ships < 0) { throw new UnityException("Invalid ship amount: Must not be negative"); }
            _ships = value;
        }
    }

    int _hangarSize;
    public int hangarSize {
        get { return _hangarSize; }
        set {
            if (hangarSize < 0) { throw new UnityException("Invalid hangar size: Must not be negative"); }
            _hangarSize = value;
        }
    }

    int _factorySpeed;
    public int factorySpeed {
        get { return _factorySpeed; }
        set {
            if (factorySpeed < 0) { throw new UnityException("Invalid factory speed: Must not be negative"); }
            _factorySpeed = value;
        }
    }


    public PlanetData() {
        name = GetRandomPlanetName();
        position.Set(0,0);
        diameter = 10;
        ships = 10;
        hangarSize = 100;
        factorySpeed = 10;
    }

    public PlanetData(Vector2 position, float diameter, int ships, int hangarSize, int factorySpeed) {
        this.name = GetRandomPlanetName();
        this.position = position;
        this.diameter = diameter;
        this.ships = ships;
        this.hangarSize = hangarSize;
        this.factorySpeed = factorySpeed;
    }


    public float GetDistance(Vector2 ToPosition) {
        return (ToPosition - this.position).magnitude;
    }

    //Returns the distance of this planet to any other position.
    //NOTE: Returns the distance to the planet's surface, not it's center
    public float GetSurfaceDistance(Vector2 position) {
        return GetDistance(position) - diameter / 2;
    }

    //Returns the distance between two planets (their surfaces, not their centers)
    public float GetSurfaceDistance(PlanetData other) {
        return GetDistance(other.position) - (other.diameter + diameter) / 2;
    }


    static List<string> predefinedPlanetNames = new List<string> {  "Aarseth", "Abdulla", "Adriana", "Asphaug", "Balaton", "Behaim", "Briggs", "Byrd", "Chaubal", "Cipolla", "Cyrus", "Decatur", "Dimpna", "Dumont",
                                                                    "Echnaton", "Elbrus", "Elisa", "Erminia", "Fanynka", "Figneria", "Frobel", "Galinskij", "Ganguly", "Giocasilli", "Granule", "Hanakusa", "Harada",
                                                                    "Hjorter", "Humecronyn", "Iglika", "Ikaunieks", "Isoda", "Jansky", "Kabtamu", "Kalinin", "Koikeda", "Landoni", "Lebedev", "Licitra", "Lyubov",
                                                                    "Miknaitis", "Namba", "Orchiston", "Pandion", "Penttila", "Quero", "Radmall", "Ruetsch", "Serra", "Shustov", "Siurana", "Smaklosa", "Szalay",
                                                                    "Tenmu", "Tietjen", "Trombka", "Tytgat", "Velichko", "Vulpius", "Wupatki", "Xanthus", "Yarilo", "Zajonc", "Zeissia", "Zykina" };

    string GetRandomPlanetName() {
        return predefinedPlanetNames[UnityEngine.Random.Range(0, predefinedPlanetNames.Count)];
    }



    public override string ToString() {
        Vector2 position = this.position;
        float diameter = this.diameter;
        return "Planet \"" + name + "\" at " + position.ToString() + " belongs to " + ((owner == null) ? "noone" : "unknown"/*TODO: owner.name*/) + " , diameter = " + diameter + " has: " + ships + " (+" + factorySpeed + " á turn) ships, with hangar size " + hangarSize;
    }
    
    
}
