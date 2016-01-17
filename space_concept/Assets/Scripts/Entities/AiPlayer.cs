using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class AiPlayer {
    public PlayerData playerData { get; private set; }
    Space space;
    //SpaceData spaceData;
    public const float NO_POINTS = -10000;

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
        float shouldWaitForProductionPoints = getShouldWaitForProductionPoints(planetInfo.origin);

        // Get highest attack points...
        float shouldAttackPoints = NO_POINTS;
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
        float shouldSupportPoints = NO_POINTS;
        PlanetData shouldSupportPlanet = null;
        foreach (TactileInformation friend in tactileInfo) {
            if (friend == planetInfo) { continue; }
            float points = getSupportPoints(planetInfo, friend, TroopData.GetTravelTime(planetInfo.origin, friend.origin));
            if (shouldSupportPlanet == null || shouldSupportPoints < points) {
                shouldSupportPoints = points;
                shouldSupportPlanet = friend.origin;
            }
        }

        float factoryUpgradePoints = getFactoryUpgradePoints(planetInfo.origin);
        float hangarUpgradePoints = getHangarUpgradePoints(planetInfo.origin);
        float upgradePoints = Math.Max(factoryUpgradePoints, hangarUpgradePoints);
      
        if (shouldWaitForProductionPoints > shouldAttackPoints 
            && shouldWaitForProductionPoints > shouldSupportPoints
            && shouldWaitForProductionPoints > upgradePoints) {   //Wait for production
            Debug.Log("AI '" + playerData.Name + "', Planet '" + planetInfo.origin.Name + "':  Doing nothing.");
            return false;
        }

        if (upgradePoints > shouldAttackPoints
            && upgradePoints > shouldSupportPoints) {   //Upgrade
            if(factoryUpgradePoints >= hangarUpgradePoints) {
                Debug.Log("AI '" + playerData.Name + "', Planet '" + planetInfo.origin.Name + "':  Upgrading Factory.");
                return planetInfo.origin.UpgradeFactory();
            }
            Debug.Log("AI '" + playerData.Name + "', Planet '" + planetInfo.origin.Name + "':  Upgrading Hangar.");
            return planetInfo.origin.UpgradeHangar();
        }

        if (shouldAttackPoints >= shouldSupportPoints && shouldAttackPlanet != null) {
            float shipsForAttack = planetInfo.origin.Ships;
            if (shouldAttackPoints >= 25) { // Don't send all ships
                shipsForAttack = shouldAttackPoints * 1.2f;
                shipsForAttack = Mathf.Min(shipsForAttack, planetInfo.origin.Ships);
            }
            if(shipsForAttack <= 0) { return false; }          
            MessageHub.Publish(new NewTroopMovementEvent(this, space.getPlanet(planetInfo.origin), space.getPlanet(shouldAttackPlanet.planetData), (int)shipsForAttack));
            return true;    // Make another movement if possible
        } else if(shouldSupportPlanet != null) {
            float shipsForSupport = shouldSupportPoints + Math.Abs(shouldAttackPoints * 1.2f);
            shipsForSupport = Mathf.Min(shipsForSupport, planetInfo.origin.Ships);
            if (shipsForSupport <= 0) { return false; }
            MessageHub.Publish(new NewTroopMovementEvent(this, space.getPlanet(planetInfo.origin), space.getPlanet(shouldSupportPlanet), (int)shipsForSupport));
            return true;    // Make another movement if possible
        }
        return false;
    }


    float getAttackPoints(PlanetData origin, PlanetData target, int distanceInDays) {        // = ~ <difference of ships between planets
        Debug.Assert(origin.Owner != null);
        int shipsOnPlanetAtArrivalday = getEstimatedShipsOnPlanet(target, distanceInDays);
        float certanyFactor = 0;
        if (distanceInDays >= 2) {
            certanyFactor = (distanceInDays * distanceInDays * 0.015f);   // exp. curve
            certanyFactor = Math.Min(certanyFactor, 1); // [0..1]   1 day: 0.015,  ... 5 days: 0.375, 6 days: 0.54; 7 days: 0.738; 8 days: 96; 9 days: 1
        }
        float points = origin.Ships - shipsOnPlanetAtArrivalday;
        points -= certanyFactor * 200;        
        return points;
    }
    
    float getFactoryUpgradePoints(PlanetData planet) {        // = ~[-10..90]
        Debug.Assert(planet.Owner != null);
        if(planet.Ships < planet.GetFactoryUpgradeCosts()) {
            return NO_POINTS;
        }
        float fillGrade = (float)planet.Ships / (float)planet.HangarSize; // [0..1]
        if(fillGrade < 0.7f) {
            return NO_POINTS;
        }
        float points = fillGrade * fillGrade * 100; // exp. [0 .. 100]
        points += Math.Min((planet.GetNextFactoryUpgrade() - planet.FactorySpeed) * 0.5f, 150);   // factory speedup / 2
        points *= 0.6f;
        // points [0..50] + improvement/4 =   ~[0..200]
        return (points-20f) * 0.5f;
    }

    float getHangarUpgradePoints(PlanetData planet) {        //
        Debug.Assert(planet.Owner != null);
        if (planet.Ships < planet.GetHangarUpgradeCosts()) {
            return NO_POINTS;
        }
        float fillGrade = (float)planet.Ships / (float)planet.HangarSize; // [0..1]
        if (fillGrade < 0.6f) {
            return NO_POINTS;
        }
        float points = fillGrade * fillGrade * 100; // exp. [0 .. 100]
        points += Math.Min((planet.GetNextHangarUpgrade() - planet.HangarSize) * 0.1f, 50);   // hangar improvement / 2
        if (fillGrade >= 0.95) { points += 10; }
        points *= 0.6f;
        return (points - 30f) * 0.5f;
    }



    float getSupportPoints(TactileInformation origin, TactileInformation friend, int distanceInDays) {
        if(friend.threatFactor - origin.threatFactor < distanceInDays * 30) {
            return NO_POINTS;
        }
        float points = friend.threatFactor - origin.threatFactor;
        float helpfulFactor = 1f - (distanceInDays * distanceInDays * 0.02f);   // exp. curve
        helpfulFactor = Math.Min(helpfulFactor, 0); // [0..1]   1 day: 0.98, 2 days: 0.92, 3 days: 0.82 ... 7 days: 0.02; 8 days: 0
        points = points * helpfulFactor;
        //TODO: only help planet if it makes sense! (Give up if there is no hope)
        return points;
    }



    int getEstimatedShipsOnPlanet(PlanetData planetData, int futureInDays) {
        int shipsOnPlanetAtArrivalday = Math.Min(planetData.Ships + planetData.GetActualFactorySpeed() * futureInDays, Math.Max(planetData.Ships, planetData.HangarSize));
        //shipsOnPlanetAtArrivalday is the maximum number of ships that can be expected if the planet does not receive additional supplies
        return shipsOnPlanetAtArrivalday;
    }


    float getShouldWaitForProductionPoints(PlanetData planet) {        // = .. the less ships the planet has, the more points it has
        float fillGrade = (float)planet.Ships / (float)planet.HangarSize; // [0..1]
        float points = (1 - fillGrade) - 0.3f;      // [0.7 ... -0.3f]
        return (points * 30) * 0.5f;     // [10.5 ... -4.5]    
    }

    float getThreatFactor(PlanetData planet, TactileInformation info) { // = Ship difference enemy and this planet if an enemy attacks
        float threat = NO_POINTS;
        foreach(TactilePlanet enemy in info.enemies) {
            if(enemy.planetData.Owner == null) { continue; }
            threat = Math.Max(threat, getAttackPoints(enemy.planetData, planet, enemy.distance));
        }
        return threat;
    }
}
