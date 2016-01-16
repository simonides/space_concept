using UnityEngine;
using System.Collections;

public class MapGenerator {
    static int rnd(int min, int max) {
        return Random.Range(min, max);
    }


    static public SpaceData GenerateDefaultMap() {
        SpaceData spaceData = new SpaceData();
        // pos, diameter, ships, hangar, factory, isStartPlanet
        spaceData.AddPlanet(new PlanetData(new Vector2(0, 0), 60, 100, 150, 15, true));
        spaceData.AddPlanet(new PlanetData(new Vector2(1500, 1000), 60, 100, 150, 15, true));
        spaceData.AddPlanet(new PlanetData(new Vector2(1500, 0), 60, 100, 150, 15, true));
        spaceData.AddPlanet(new PlanetData(new Vector2(0, 1000), 60, 100, 150, 15, true));
        
        spaceData.AddPlanet(new PlanetData(new Vector2(600, 420), 220, 2200, rnd(250, 380), rnd(150, 250), false));
        spaceData.AddPlanet(new PlanetData(new Vector2(900, 580), 220, 2200, rnd(250, 380), rnd(150, 250), false));
        spaceData.AddPlanet(new PlanetData(new Vector2(750, 500), 50, 0, rnd(15, 25), rnd(5, 15), false));

        spaceData.AddPlanet(new PlanetData(new Vector2(640, 550), 40, rnd(1500, 2500), rnd(10, 40), 0, false));
        spaceData.AddPlanet(new PlanetData(new Vector2(860, 450), 40, rnd(1500, 2500), rnd(10, 40), 0, false));

        spaceData.AddPlanet(new PlanetData(new Vector2(80, 200), 80, rnd(40, 60), rnd(110, 140), rnd(15, 20), false));
        spaceData.AddPlanet(new PlanetData(new Vector2(300, 80), 100, rnd(40, 60), rnd(170, 200), rnd(10, 15), false));

        spaceData.AddPlanet(new PlanetData(new Vector2(80, 800), 80, rnd(40, 60), rnd(110, 140), rnd(15, 20), false));
        spaceData.AddPlanet(new PlanetData(new Vector2(300, 920), 100, rnd(40, 60), rnd(170, 200), rnd(10, 15), false));

        spaceData.AddPlanet(new PlanetData(new Vector2(1420, 200), 80, rnd(40, 60), rnd(110, 140), rnd(15, 20), false));
        spaceData.AddPlanet(new PlanetData(new Vector2(1200, 80), 100, rnd(40, 60), rnd(170, 200), rnd(10, 15), false));

        spaceData.AddPlanet(new PlanetData(new Vector2(1420, 800), 80, rnd(40, 60), rnd(110, 140), rnd(15, 20), false));
        spaceData.AddPlanet(new PlanetData(new Vector2(1200, 902), 100, rnd(40, 60), rnd(170, 200), rnd(10, 15), false));


        spaceData.AddPlanet(new PlanetData(new Vector2(250, 500), 120, rnd(180, 250), 180, 45, false));
        spaceData.AddPlanet(new PlanetData(new Vector2(1250, 500), 120, rnd(180, 250), 180, 45, false));

        spaceData.AddPlanet(new PlanetData(new Vector2(750, 50), 140, rnd(200, 350), 250, 65, false));
        spaceData.AddPlanet(new PlanetData(new Vector2(750, 950), 140, rnd(200, 350), 250, 65, false));
        
        return spaceData;
    }
}
