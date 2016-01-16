using UnityEngine;
using System.Collections;
using System;

//[RequireComponent(typeof(SpriteRenderer))]
public class Troop : MonoBehaviour {
    
    public TroopData troopData { get; private set; }
    public float FlyAnimationSpeed = 50;

    // Graphical movement data (handled by AirTrafficControl):
    public Vector2 StartPosition;
    public Vector2 TargetPosition;

    // The position (progress) where this ship is flying towards to
    public float TargetProgress;

    float Progress;

    public void Init(int currentDay, TroopData troop) {
        troopData = troop;
        StartPosition.Set(0, 0);
        TargetPosition.Set(0, 0);
        Progress = 0;
        UpdatePosition(currentDay);   // sets the target progress

        this.name = GetNameForTroopGameObject(troopData);
    }

    public void UpdatePosition(int currentDay) {
        int remainingDays = troopData.ArrivalTime - currentDay;
        TargetProgress = 100 - (remainingDays * 100 / troopData.TravelTime);
        if(TargetProgress > 100) {
            Debug.LogWarning("Ships have not been destroyed, although they already reached the destination");
            TargetProgress = 100;
        }
    }

    public void Update() {
        Progress += Time.deltaTime * FlyAnimationSpeed;
        if(Progress > TargetProgress) {
            Progress = TargetProgress;
        }
        Vector3 position = GetTargetPosition();
        position.z = -15;   // In front of planets
        this.transform.localPosition = position;       
    }


    Vector2 GetTargetPosition() {
        Vector2 dir = TargetPosition - StartPosition;
        return StartPosition + (dir * Progress / 100);
    }

    private string GetNameForTroopGameObject(TroopData troopData) {
        try {
            return troopData.Owner.Name + " (" + troopData.ShipCount + "): " + troopData.StartPlanet.Name + " > " + troopData.TargetPlanet.Name;
        } catch (NullReferenceException) {
            return "Troop of " + troopData.ShipCount + " ships";
        }
    }



}
