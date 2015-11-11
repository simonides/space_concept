using UnityEngine;
using System.Collections;

/**
 *  Troop Entity.
 *  Contains all information about a single troop movement from one planet to another.
 *  Can be serialised / deserialised to a file.
 */
public class TroopData {

    // ****    CONFIGURATION    **** //
    public int TravelSpeed = 10;
    // ****                     **** //


    public PlanetData StartPlanet { get; private set;}
    public PlanetData TargetPlanet { get; private set; }

    public int ShipCount { get; private set; }
    public PlayerData Owner { get; private set; }

    public int ArrivalTime { get; private set; }    

    TroopData(PlanetData start, PlanetData target, int shipCount) {
        StartPlanet = start;
        TargetPlanet = target;
        ShipCount = shipCount;
        Owner = start.Owner;
        ArrivalTime = (int)System.Math.Round(0 + start.GetSurfaceDistance(target) / TravelSpeed);       // TODO: replace 0 with current day
    }


}
