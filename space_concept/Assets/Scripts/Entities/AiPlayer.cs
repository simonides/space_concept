using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class AiPlayer {
    public PlayerData playerData { get; private set; }
    Space space;
    //SpaceData spaceData;

    //seralization needs a default public constructor
    public AiPlayer() {
    }

    public AiPlayer(PlayerData playerData, Space space) {
        this.playerData = playerData;
        this.space = space;
        //this.spaceData = space.spaceData;
        playerData.IsHumanPlayer = false;
    }

    List<TactileInformation> tactileInfo;

    public void PerformNextMovement() {
        PrepareTactileInformation();

        foreach (TactileInformation planet in tactileInfo) {
            while (PerformMovementOnPlanet(planet))
                ;
        }
    }

    // Sets up tactileInformation for each owned planet
    void PrepareTactileInformation() {
        tactileInfo = new List<TactileInformation>();
        foreach (PlanetData planet in playerData.GetOwnedPlanets()) {
            TactileInformation info = space.spaceData.GetTactileInformation(planet);
            info.threatFactor = getThreatFactor(planet, info);
            tactileInfo.Add(info);            
        }
    }
    

    // Sums up points for all kinds of movement and does one of them
    bool PerformMovementOnPlanet(TactileInformation planetInfo) {
        if(planetInfo.origin.Ships <= 1) {
            return false;
        }
        // Get points for doing nothing...
        float shouldWaitForProductionPoints = getShouldWaitForProductionPoints(planetInfo.origin) -200;

        // Get highest attack points...
        float shouldAttackPoints = 0;
        TactilePlanet shouldAttackPlanet = null;
        foreach (TactilePlanet enemy in planetInfo.enemies) {
            Debug.Assert(planetInfo.origin.Owner == playerData);
            float points = getAttackPoints(planetInfo.origin, enemy.planetData, enemy.distance);
            if(shouldAttackPlanet == null || shouldAttackPoints < points) {
                shouldAttackPoints = points;
                shouldAttackPlanet = enemy;
            }
        }

        // Get points for supporting friends...
        float shouldSupportPoints = 0;
        PlanetData shouldSupportPlanet = null;
        foreach (TactileInformation friend in tactileInfo) {
            if (friend == planetInfo) { continue; }
            float points = getSupportPoints(planetInfo, friend, TroopData.GetTravelTime(planetInfo.origin, friend.origin));
            if (shouldSupportPlanet == null || shouldSupportPoints < points) {
                shouldAttackPoints = points;
                shouldSupportPlanet = friend.origin;
            }
        }

        // TODO: upgrade-points

        if(shouldWaitForProductionPoints > shouldAttackPoints 
            && shouldWaitForProductionPoints > shouldSupportPoints) {   //Wait for production
            Debug.Log("AI '" + playerData.Name + "', Planet '" + planetInfo.origin.Name + "':  Doing nothing.");
            return false;
        }

        if(shouldAttackPoints > shouldSupportPoints && shouldAttackPlanet != null) {
            float shipsForAttack = Mathf.Max(shouldAttackPoints * 1.2f, planetInfo.origin.Ships);            
            MessageHub.Publish(new NewTroopMovementEvent(this, space.getPlanet(planetInfo.origin), space.getPlanet(shouldAttackPlanet.planetData), (int)shipsForAttack));
            return true;    // Make another movement if possible
        } else if(shouldSupportPlanet != null) {
            float shipsForSupport = Mathf.Max(shouldSupportPoints * 2.2f, planetInfo.origin.Ships);
            MessageHub.Publish(new NewTroopMovementEvent(this, space.getPlanet(planetInfo.origin), space.getPlanet(shouldSupportPlanet), (int)shipsForSupport));
            return true;    // Make another movement if possible
        }
        return false;
    }


    float getAttackPoints(PlanetData origin, PlanetData target, int distanceInDays) {        // = ~ <difference of ships between planets
        Debug.Assert(origin.Owner != null);
        int shipsOnPlanetAtArrivalday = getEstimatedShipsOnPlanet(target, distanceInDays);
        float certanyFactor = 1f - (distanceInDays * distanceInDays * 0.02f);   // exp. curve
        certanyFactor = Math.Min(certanyFactor, 0); // [0..1]   1 day: 0.98, 2 days: 0.92, 3 days: 0.82 ... 7 days: 0.02; 8 days: 0

        float points = origin.Ships - shipsOnPlanetAtArrivalday;
        if (points > 0) {    // reduce points
            points *= certanyFactor;
        } else {
            points *= (2 - certanyFactor);
        }
        return points;
    }

    float getSupportPoints(TactileInformation origin, TactileInformation friend, int distanceInDays) {
        float points = friend.threatFactor - origin.threatFactor;
        float helpfulFactor = 1f - (distanceInDays * distanceInDays * 0.02f);   // exp. curve
        helpfulFactor = Math.Min(helpfulFactor, 0); // [0..1]   1 day: 0.98, 2 days: 0.92, 3 days: 0.82 ... 7 days: 0.02; 8 days: 0
        points = points * helpfulFactor;
        //TODO: only help planet if it makes sense!
        return points;
    }



    int getEstimatedShipsOnPlanet(PlanetData planetData, int futureInDays) {
        int shipsOnPlanetAtArrivalday = Math.Min(planetData.Ships + planetData.GetActualFactorySpeed() * futureInDays, planetData.HangarSize);
        //shipsOnPlanetAtArrivalday is the maximum number of ships that can be expected if the planet does not receive additional supplies
        return shipsOnPlanetAtArrivalday;
    }


    float getShouldWaitForProductionPoints(PlanetData planet) {        // = [-50 .. +50] .. the less ships the planet has, the more points it has
        float fillGrade = planet.Ships / planet.HangarSize; // [0..1]
        float points = (1 - fillGrade) - 0.5f;      // [0.5 ... -0.5f]
        return points * 50;        
    }

    float getThreatFactor(PlanetData planet, TactileInformation info) { // = Ship difference enemy and this planet if an enemy attacks
        float threat = -10000;
        foreach(TactilePlanet enemy in info.enemies) {
            if(enemy.planetData.Owner == null) { continue; }
            threat = Math.Max(threat, getAttackPoints(enemy.planetData, planet, enemy.distance));
        }
        return threat;
    }
}
