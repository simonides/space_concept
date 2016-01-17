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
    public const float UPGRADE_FACTOR = 1.5f;       //Multiplication factor for upgrade steps (both hangar + factory)
    public const int UPGRADE_STEP = 5;              //Upgrade steps: the different upgrade-levels are always "modulo UPGRADE_STEP == 0". --> if step is 5, the different levels can only be 5, 10, 15, 20, ...  (Note: the fist level is loaded from the mapfile and can be anything)
    public const int FACTORY_UPGRADE_COSTS = 5;     //The number of ships that are required to increase the speed by 1.
    public const int HANGAR_UPGRADE_COSTS = 1;      //The number of ships that are required to increase the hangar by 1.


    public PlayerData Owner { get; private set; }               // The player who owns this planet

    public string Name { get; set; }                // Name of the planet

    public bool IsStartPlanet { get; set; }         // true: This planet can be used to spawn players

    public Vector2 Position { get; set; }

    public string TextureName { get; set; }
    public string TextureFXName { get; set; }

    float _diameter;
    public float Diameter
    {
        get { return _diameter; }
        set
        {
            if (Diameter < 0) { throw new UnityException("Invalid diameter: Must be greater than zero"); }
            _diameter = value;
        }
    }

    int _ships;
    public int Ships
    {
        get { return _ships; }
        set
        {
            if (Ships < 0) { throw new UnityException("Invalid ship amount: Must not be negative"); }
            _ships = value;
        }
    }

    int _hangarSize;
    public int HangarSize
    {
        get { return _hangarSize; }
        set
        {
            if (HangarSize < 0) { throw new UnityException("Invalid hangar size: Must not be negative"); }
            _hangarSize = value;
        }
    }

    int _factorySpeed;
    public int FactorySpeed
    {
        get { return _factorySpeed; }
        set
        {
            if (FactorySpeed < 0) { throw new UnityException("Invalid factory speed: Must not be negative"); }
            _factorySpeed = value;
        }
    }
    
    public void SetOwner(PlayerData owner) {
        PlayerData oldOwner = this.Owner;
        this.Owner = owner;
        if (oldOwner != null) {
            oldOwner.RemovePlanetFromOwnership(this);
        }
        if(this.Owner != null) {
            this.Owner.AddPlanetToOwnership(this);
        }
    }

    public PlanetData() {
        Name = "";
        Position.Set(0, 0);
        Diameter = 10;
        Ships = 10;
        HangarSize = 100;
        FactorySpeed = 10;
    }

    public PlanetData(Vector2 position, float diameter, int ships, int hangarSize, int factorySpeed, bool isStartPlanet) {
        this.Name = "";
        this.Position = position;
        this.Diameter = diameter;
        this.Ships = ships;
        this.HangarSize = hangarSize;
        this.FactorySpeed = factorySpeed;
        this.IsStartPlanet = isStartPlanet;
        this.TextureName = "";
        this.TextureFXName = "";
    }

    //Upgrading costs and steps:
    public int GetFactoryUpgradeCosts() {
        int upgradeAmount = GetNextFactoryUpgrade() - FactorySpeed;
        return upgradeAmount * FACTORY_UPGRADE_COSTS;
    }
    public int GetNextFactoryUpgrade() {
        int next = Mathf.CeilToInt(FactorySpeed * UPGRADE_FACTOR);
        next += (UPGRADE_STEP - (next % UPGRADE_STEP)) % UPGRADE_STEP;
        return next;
    }
    public int GetHangarUpgradeCosts() {
        int upgradeAmount = GetNextHangarUpgrade() - HangarSize;
        return upgradeAmount * HANGAR_UPGRADE_COSTS;
    }
    public int GetNextHangarUpgrade() {
        int next = Mathf.CeilToInt(HangarSize * UPGRADE_FACTOR);
        next += (UPGRADE_STEP - (next % UPGRADE_STEP)) % UPGRADE_STEP;
        return next;
    }

    public bool UpgradeFactory() {
        int costs = GetFactoryUpgradeCosts();
        if (costs > Ships) {
            Debug.Log("Unable to upgrade Factory - Can't afford expenses");
            return false;
        }
        Ships -= costs;
        FactorySpeed = GetNextFactoryUpgrade();
        Debug.Log("Upgraded Factory of planet " + Name);
        return true;
    }

    public bool UpgradeHangar() {
        int costs = GetHangarUpgradeCosts();
        if (costs > Ships) {
            Debug.Log("Unable to upgrade Hangar - Can't afford expenses");
            return false;
        }
        Ships -= costs;
        HangarSize = GetNextHangarUpgrade();
        Debug.Log("Upgraded Hangar of planet " + Name);
        return true;
    }

    public int GetActualFactorySpeed() {
        if (Owner == null) {
            return FactorySpeed / 2;
        }
        return FactorySpeed;
    }

    public int ProduceShips() {
        int speed = GetActualFactorySpeed();
        int empty = HangarSize - Ships;
        if (empty < 0) { empty = 0; }
        if (empty > speed) {
            Ships += speed;
            return speed;
        } else {
            Ships += empty;
            Debug.Log("Hangar full, factory can't produce at full speed");
            return empty;
        }
    }


    public AttackEvaluation EvaluateIncomingTroop(TroopData troop) {
        if (troop.TargetPlanet != this) {
            throw new ArgumentException("Unable to evaluate incoming troop: The troop is not arriving at this planet. Me: " + Name + ", Troop: " + troop.ToString());
        }

        if (troop.Owner == Owner) {
            return EvaluateIncommingSupply(troop);
        }

        return EvaluateIncommingAttack(troop);
    }





    // Supply ships from other planet
    private AttackEvaluation EvaluateIncommingSupply(TroopData troop) {
        Debug.Assert(troop.Owner == Owner, "Invalid call");

        int remainingSpace = HangarSize - Ships;  //Remaining space
        if (remainingSpace < 0) { remainingSpace = 0; }

        int lostShips = 0;

        if (remainingSpace < troop.ShipCount) {          //Some ships will get lost
            Ships += remainingSpace;
            lostShips = Mathf.RoundToInt((troop.ShipCount - remainingSpace) * 0.5f);     //All other ships will get a 50% penality
            Ships += troop.ShipCount - remainingSpace - lostShips;
        } else {
            Ships += troop.ShipCount;
        }

        AttackEvaluation evaluation = AttackEvaluation.Supply(troop, Ships, lostShips);
        return evaluation;
    }

    // Attacking ships from other planet
    private AttackEvaluation EvaluateIncommingAttack(TroopData troop) {
        Debug.Assert(troop.Owner != Owner, "Invalid call");

        bool gotCaptured = (Ships <= troop.ShipCount);
        if (Owner == null && Ships == troop.ShipCount) { // Planet was neutral and stayed neutral. But there are no more ships on this planet anymore.
            gotCaptured = false;
        }

        PlayerData oldOwner = Owner;
        PlayerData newOwner = (gotCaptured ? troop.Owner : oldOwner);
        if(Ships == troop.ShipCount) {
            newOwner = null;
        }

        int lostShipsByOwner = 0;
        int lostShipsByAttacker = 0;
        int lostShipsByLanding = 0;

        if (Ships <= troop.ShipCount) { // planet is lost
            lostShipsByOwner = Ships;
            lostShipsByAttacker = Ships;
            // check for landing - maybe the attacker won, but not all ships have space to land:
            lostShipsByLanding = Math.Max((int)(Mathf.RoundToInt((troop.ShipCount - Ships - HangarSize) * 0.5f)), 0);     //50% of all ships that hadn't enough space lost
        } else {
            lostShipsByOwner = troop.ShipCount;
            lostShipsByAttacker = troop.ShipCount;
        }

        // Perform actions:
        if (gotCaptured) {
            SetOwner(newOwner);
            Ships = troop.ShipCount - lostShipsByAttacker - lostShipsByLanding;
        } else {
            Ships -= lostShipsByOwner;
        }

        Debug.Assert(lostShipsByAttacker + lostShipsByLanding <= troop.ShipCount);
        AttackEvaluation evaluation = AttackEvaluation.Attack(troop, oldOwner, newOwner, Ships, lostShipsByOwner, lostShipsByAttacker, lostShipsByLanding);
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


    

    public override string ToString() {
        return "Planet \"" + Name + "\"" + (IsStartPlanet ? "(startPlanet) " : "") + "at " + Position.ToString() + " belongs to " + ((Owner == null) ? "noone" : Owner.Name) + " , diameter = " + Diameter + " has: " + Ships + " (+" + FactorySpeed + " á turn) ships, with hangar size " + HangarSize;
    }

}
