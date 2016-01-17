using UnityEngine;
using System.Collections.Generic;

public class MapGenerator {
    static int rnd(int min, int max) {
        return Random.Range(min, max);
    }

    static int rnd(int min, int max, int mod) {
        int val = Random.Range(min, max);
        return val - (val % mod);
    }

    static public SpaceData GenerateDefaultMap() {
        SpaceData spaceData = new SpaceData();

        
        // pos, diameter, ships, hangar, factory, isStartPlanet
        spaceData.AddPlanet(new PlanetData(new Vector2(0, 0), 18, 100, 150, 15, true));
        spaceData.AddPlanet(new PlanetData(new Vector2(1500, 1000), 18, 100, 150, 15, true));
        spaceData.AddPlanet(new PlanetData(new Vector2(1500, 0), 18, 100, 150, 15, true));
        spaceData.AddPlanet(new PlanetData(new Vector2(0, 1000), 18, 100, 150, 15, true));
        
        spaceData.AddPlanet(new PlanetData(new Vector2(600, 420), 66, 2200, rnd(250, 380, 10), rnd(150, 250, 10), false));
        spaceData.AddPlanet(new PlanetData(new Vector2(900, 580), 66, 2200, rnd(250, 380, 10), rnd(150, 250, 10), false));
        //spaceData.AddPlanet(new PlanetData(new Vector2(750, 500), 15, 0, rnd(15, 25, 5), rnd(5, 15, 5), false));

        spaceData.AddPlanet(new PlanetData(new Vector2(640, 550), 12, rnd(1500, 2500, 100), rnd(10, 40, 5), 0, false));
        spaceData.AddPlanet(new PlanetData(new Vector2(860, 450), 12, rnd(1500, 2500, 100), rnd(10, 40, 5), 0, false));

        spaceData.AddPlanet(new PlanetData(new Vector2(80, 200), 24, rnd(40, 60, 10), rnd(110, 140, 10), rnd(15, 20, 5), false));
        spaceData.AddPlanet(new PlanetData(new Vector2(300, 80), 30, rnd(40, 60, 10), rnd(170, 200, 10), rnd(10, 15, 5), false));

        spaceData.AddPlanet(new PlanetData(new Vector2(80, 800), 24, rnd(40, 60, 10), rnd(110, 140, 10), rnd(15, 20, 5), false));
        spaceData.AddPlanet(new PlanetData(new Vector2(300, 920), 30, rnd(40, 60, 10), rnd(170, 200, 10), rnd(10, 15, 5), false));

        spaceData.AddPlanet(new PlanetData(new Vector2(1420, 200), 24, rnd(40, 60, 10), rnd(110, 140, 10), rnd(15, 20, 5), false));
        spaceData.AddPlanet(new PlanetData(new Vector2(1200, 80), 30, rnd(40, 60, 10), rnd(170, 200, 10), rnd(10, 15, 5), false));

        spaceData.AddPlanet(new PlanetData(new Vector2(1420, 800), 24, rnd(40, 60, 10), rnd(110, 140, 10), rnd(15, 20, 5), false));
        spaceData.AddPlanet(new PlanetData(new Vector2(1200, 902), 30, rnd(40, 60, 10), rnd(170, 200, 10), rnd(10, 15, 5), false));


        spaceData.AddPlanet(new PlanetData(new Vector2(250, 500), 36, rnd(180, 250, 10), 180, 45, false));
        spaceData.AddPlanet(new PlanetData(new Vector2(1250, 500), 36, rnd(180, 250, 10), 180, 45, false));

        spaceData.AddPlanet(new PlanetData(new Vector2(750, 50), 42, rnd(200, 350, 10), 250, 65, false));
        spaceData.AddPlanet(new PlanetData(new Vector2(750, 950), 42, rnd(200, 350, 10), 250, 65, false));
        
        return spaceData;
    }




    static public SpaceData GenerateRandomMap(int planetCount) {
        SpaceData spaceData = new SpaceData();
        //List<PlanetData> planets = new List<PlanetData>();//hold the list of planets to check if they are correctly disributed

        PlanetData planet;

        float minPlanetDistance = 70;

        float spacePerPlanet = 500;
        float spaceArea = planetCount * spacePerPlanet;
        Vector2 size = new Vector2(spaceArea /9, spaceArea / 16 );
        Rect mapSize = new Rect(new Vector2(0, 0), size);

   
        Vector2 newPosition = new Vector2();
        bool canPositionPlanet = true;
        for (int i = 0; i < planetCount; i++) {
            int timeout = 100;
            do {
                newPosition.x = Random.Range(mapSize.xMin, mapSize.xMax);
                newPosition.y = Random.Range(mapSize.yMin, mapSize.yMax);
                canPositionPlanet = true;
                foreach (PlanetData p in spaceData.planets) {
                    if (Vector2.Distance(newPosition, p.Position) < minPlanetDistance) {
                        canPositionPlanet = false;
                        break;
                    }
                }
                timeout--;
            } while (!canPositionPlanet && timeout > 0);
            int hangarSize = Random.Range(30, 500);
            int ships = (int)((float)hangarSize * Random.Range(0.05f, 0.4f));
            int factorySpeed = System.Math.Min((int)((float)hangarSize * Random.Range(0.2f, 0.45f)), 100);
            planet = new PlanetData(new Vector2(newPosition.x, newPosition.y), Random.Range(15, 58f), ships, hangarSize, factorySpeed, true);
            spaceData.AddPlanet(planet);            
        }

        Vector2 minMapSize = mapSize.size / 2;  // M
        Vector2 upsizingFactor = new Vector2(spaceData.GetSize().x / minMapSize.x, spaceData.GetSize().y / minMapSize.y);

        if (upsizingFactor.x < 1) {
            upsizingFactor.x = 1;
        }
        if (upsizingFactor.y < 1) {
            upsizingFactor.y = 1;
        }

        foreach (PlanetData p in spaceData.planets) {
             p.Position = new Vector2(p.Position.x * upsizingFactor.x, p.Position.y * upsizingFactor.y);
        }
        spaceData.ForceBoundRecalculation();

        return spaceData;
    }


}
