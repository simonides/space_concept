using UnityEngine;
using System.Collections.Generic;

public class GameStateData {

    public int CurrentDay { get; private set; }     // Current day, starting at 1
    public List<TroopData> troopMovements { get; set; }

    public GameStateData() {
        CurrentDay = 1;
        troopMovements = new List<TroopData>();
    }

    public void NextDay() {
        CurrentDay++;
    }

    public void AddTroopMovement(TroopData troopData) {
        if(troopData.ArrivalTime <= CurrentDay) {
            Debug.LogError("Added troopMovement whose arrivel time is in the past or the current day (" + troopData.ArrivalTime + ")");
        }
        troopMovements.Add(troopData);
    }
    
    public List<TroopData> GetTroopsForDay(int arrivalDay) {
        List<TroopData> troops = new List<TroopData>();

        foreach (TroopData troop in troopMovements) {
            if (troop.ArrivalTime < CurrentDay) {
                troops.Add(troop);
            }
        }

        return troops;
    }

    public void RemoveOldTroopMovements() {
        troopMovements.RemoveAll(troop => troop.ArrivalTime < CurrentDay);
    }
}
