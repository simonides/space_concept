using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class Troop : MonoBehaviour {
    
    TroopData troopData;

    public void Init(TroopData troop) {
        troopData = troop;

        this.name = GetNameForTroopGameObject(troopData);
        this.transform.localScale = new Vector3(30, 30, 1);
    }

    

    private string GetNameForTroopGameObject(TroopData troopData) {
        try {
            return troopData.Owner.Name + " (" + troopData.ShipCount + "): " + troopData.StartPlanet.Name + " > " + troopData.TargetPlanet.Name;
        } catch (NullReferenceException) {
            return "Troop of " + troopData.ShipCount + " ships";
        }
    }

}
