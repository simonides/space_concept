using UnityEngine;
using System.Collections.Generic;

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




    static public SpaceData GenerateRandomMap(int planetCount) {
        SpaceData spaceData = new SpaceData();
        //List<PlanetData> planets = new List<PlanetData>();//hold the list of planets to check if they are correctly disributed

        PlanetData planet;

        float minPlanetDistance = 150f;

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
            planet = new PlanetData(new Vector2(newPosition.x, newPosition.y), Random.Range(40f, 150f), ships, hangarSize, factorySpeed, true);
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
