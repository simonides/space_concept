using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 *  Planet Entity.
 *  Contains all information about a single planet.
 *  Can be serialised / deserialised to a file.
 */
[System.Serializable]
public class PlanetData {
    
    public PlayerData Owner { get; set; }               // The player who owns this planet

    public string Name { get; set; }                // Name of the planet

    public bool IsStartPlanet { get; set; }         // true: This planet can be used to spawn players

    public Vector2 Position { get; set; }

    float _diameter;
    public float Diameter {
        get { return _diameter; }
        set {
            if (Diameter < 0) { throw new UnityException("Invalid diameter: Must be greater than zero"); }
            _diameter = value;
        }
    }

    int _ships;
    public int Ships {
        get { return _ships; }
        set {
            if (Ships < 0) { throw new UnityException("Invalid ship amount: Must not be negative"); }
            _ships = value;
        }
    }

    int _hangarSize;
    public int HangarSize {
        get { return _hangarSize; }
        set {
            if (HangarSize < 0) { throw new UnityException("Invalid hangar size: Must not be negative"); }
            _hangarSize = value;
        }
    }

    int _factorySpeed;
    public int FactorySpeed {
        get { return _factorySpeed; }
        set {
            if (FactorySpeed < 0) { throw new UnityException("Invalid factory speed: Must not be negative"); }
            _factorySpeed = value;
        }
    }


    public PlanetData() {
        Name = GetRandomPlanetName();
        Position.Set(0,0);
        Diameter = 10;
        Ships = 10;
        HangarSize = 100;
        FactorySpeed = 10;
    }

    public PlanetData(Vector2 position, float diameter, int ships, int hangarSize, int factorySpeed, bool isStartPlanet) {
        this.Name = GetRandomPlanetName();
        this.Position = position;
        this.Diameter = diameter;
        this.Ships = ships;
        this.HangarSize = hangarSize;
        this.FactorySpeed = factorySpeed;
        this.IsStartPlanet = isStartPlanet;
    }



    public AttackEvaluation EvaluateIncomingTroop(TroopData troop) {
        if(troop.TargetPlanet != this) {
            throw new ArgumentException("Unable to evaluate incoming troop: The troop is not arriving at this planet. Me: " + Name + ", Troop: " + troop.ToString());
        }


        if(troop.Owner == Owner) {
            return EvaluateIncommingSupply(troop);
        }

        // TODO: Implement

        return new AttackEvaluation();
    }

    // Supply ships from other planet
    private AttackEvaluation EvaluateIncommingSupply(TroopData troop) {
        Debug.Assert(troop.Owner == Owner, "Invalid call");
        AttackEvaluation evaluation = new AttackEvaluation();
        //evaluation.type = EvaluationType.Supply;

        //TODO: implement

        return evaluation;
    }





    public float GetDistance(Vector2 ToPosition) {
        return (ToPosition - this.Position).magnitude;
    }

    //Returns the distance of this planet to any other position.
    //NOTE: Returns the distance to the planet's surface, not it's center
    public float GetSurfaceDistance(Vector2 position) {
        return GetDistance(position) - Diameter / 2;
    }

    //Returns the distance between two planets (their surfaces, not their centers)
    public float GetSurfaceDistance(PlanetData other) {
        return GetDistance(other.Position) - (other.Diameter + Diameter) / 2;
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
        return "Planet \"" + Name + "\"" + (IsStartPlanet ? "(startPlanet) " : "" )+ "at " + Position.ToString() + " belongs to " + ((Owner == null) ? "noone" : Owner.Name) + " , diameter = " + Diameter + " has: " + Ships + " (+" + FactorySpeed + " á turn) ships, with hangar size " + HangarSize;
    }
       
}
