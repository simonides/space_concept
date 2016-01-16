using UnityEngine;
using System.Collections.Generic;

public class TactileInformation {

    // Filled by SpaceData
    public PlanetData origin { get; private set; }

    public List<TactilePlanet> enemies { get; private set; }
    public List<TactilePlanet> friends { get; private set; }

    // Filled by AI
    public float threatFactor;

    
    public TactileInformation(PlanetData origin, List<TactilePlanet> enemies, List<TactilePlanet> friends) {
        this.origin = origin;
        this.enemies = enemies;
        this.friends = friends;
    }
    
}