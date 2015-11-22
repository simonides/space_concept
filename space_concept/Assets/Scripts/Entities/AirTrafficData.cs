using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AirTrafficData {
    public List<TroopData> airTraffic { get; set; }

    public AirTrafficData() {
        airTraffic = new List<TroopData>();
    }



    public void AddTroopMovement(TroopData troopData) {
        //if (troopData.ArrivalTime <= CurrentDay) {
        //    Debug.LogError("Added troopMovement whose arrivel time is in the past or the current day (" + troopData.ArrivalTime + ")");
        //}
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
}