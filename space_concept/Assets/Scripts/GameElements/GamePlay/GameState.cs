using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyMessenger;
using System;

public class GameState : MonoBehaviour {

    // ****    CONFIGURATION    **** //
    public int MINIMUM_DAY_DURATION_IN_MS = 300;    // A next-day-switch can not happen faster than this variable

    // ****  ATTACHED OBJECTS   **** //
    // ****                     **** //

    public GameStateData gameStateData { get; private set; }               // entity
    DateTime lastDayChange = DateTime.Now;     // The time when the current day started



    void Start() {
        InitEventSubscriptions();
    }


    public void Init(GameStateData gameState) {
        gameStateData = gameState;
    }


    void InitEventSubscriptions() {
        MessageHub.Subscribe<NextDayRequestEvent>(NextDayRequest);
    }

    void NextDayRequest(NextDayRequestEvent evt) {
        TimeSpan time = DateTime.Now.Subtract(lastDayChange);
        if (time.TotalMilliseconds > MINIMUM_DAY_DURATION_IN_MS) {
            lastDayChange = DateTime.Now;
            gameStateData.NextDay();
            UnityEngine.Debug.Log("Good morning! We have day " + gameStateData.CurrentDay + " now!");
            MessageHub.Publish(new NextDayEvent(this, gameStateData.CurrentDay));
        } else {
            Debug.LogWarning("You clicked too fast. I don't like that. I was programmed to ignore fast clicks.");
        }
    }
    
}
