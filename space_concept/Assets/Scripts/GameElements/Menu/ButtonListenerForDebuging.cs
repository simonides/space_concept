﻿using UnityEngine;
using System.Collections;

public class ButtonListenerForDebuging : MonoBehaviour {
    public GameObject space;

    public void NextDay()
    {
        MessageHub.Publish(new NextDayRequestEvent(this));
    }

    public void SendSomeShips()
    {
        Planet[] planets = space.GetComponentsInChildren<Planet>();
        MessageHub.Publish(new NewTroopMovementEvent(this, planets[0], planets[1], 42));
    }

    public void ShowEventList()
    {
       MessageHub.Publish(new ShowEventListEvent(this));
    }

    public void HideEventList()
    {
       MessageHub.Publish(new HideEventListEvent(this));
    }

    public void OptionMenu()
    {
        MessageHub.Publish(new ShowPauseMenuEvent(this));
    }
}
