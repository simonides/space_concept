using UnityEngine;
using System;

[System.Serializable]
public class GameStateData {

    public int CurrentDay { get; private set; }     // Current day, starting at 1

    public GameStateData() {
        CurrentDay = 1;
    }

    public void NextDay() {
        CurrentDay++;
    }

}
