using UnityEngine;
using System.Collections;

public class TactilePlanet {
    public PlanetData planetData;
    public int distance;	

    public TactilePlanet(PlanetData planetData, int distance) {
        this.planetData = planetData;
        this.distance = distance;
    }
}
