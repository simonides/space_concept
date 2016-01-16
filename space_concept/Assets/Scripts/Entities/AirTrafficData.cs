using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AirTrafficData {
    public List<TroopData> airTraffic { get; set; }

    public AirTrafficData() {
        airTraffic = new List<TroopData>();
    }



    public void AddTroopMovement(TroopData troopData) {
        //Debug.Assert(troopData.ArrivalTime > CurrentDay);
        airTraffic.Add(troopData);
    }

    public List<TroopData> GetTroopsForDay(int arrivalDay) {
        List<TroopData> troops = new List<TroopData>();

        foreach (TroopData troop in airTraffic) {
            if (troop.ArrivalTime <= arrivalDay) {
                troops.Add(troop);
            }
        }
        return troops;
    }

    public void RemoveOldTroopMovements(int dayToRemove) {
        airTraffic.RemoveAll(troop => troop.ArrivalTime <= dayToRemove);
    }

    public bool DoesPlayerHaveSomeFlyingTroops(PlayerData player) {
        foreach (TroopData troop in airTraffic) {
            if (troop.Owner == player) {
                return true;
            }
        }
        return false;
    }

}