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


    public PlanetData StartPlanet { get; private set; }
    public PlanetData TargetPlanet { get; private set; }

    public int ShipCount { get; private set; }
    public PlayerData Owner { get; private set; }

    public int ArrivalTime { get; private set; }


    public TroopData() {
        Init(null, null, 0, 0);
    }

    public TroopData(PlanetData start, PlanetData target, int shipCount, int currentDay) {
        Init(start, target, shipCount, currentDay);
    }

    public void Init(PlanetData start, PlanetData target, int shipCount, int currentDay) {
        StartPlanet = start;
        TargetPlanet = target;
        ShipCount = shipCount;
        if (start == null) {
            Owner = null;
            ArrivalTime = 0;
        } else {
            Owner = start.Owner;
            ArrivalTime = (int)System.Math.Round(currentDay + start.GetSurfaceDistance(target) / TravelSpeed);
        }
    }

    public override string ToString() {
        string ownerName = "unknown";
        if (Owner != null) {
            ownerName = Owner.Name;
        }

        string startPlanetName = StartPlanet == null ? "unknown" : StartPlanet.Name;
        string targetPlanetName = TargetPlanet == null ? "unknown" : TargetPlanet.Name;

        return "Troop from Player \"" + ownerName + "\" with " + ShipCount + " ships from Planet \"" + startPlanetName + "\" to \"" + targetPlanetName + "\"";
    }

}
