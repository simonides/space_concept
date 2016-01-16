using UnityEngine;
using System.Collections;
using TinyMessenger;

public class NewTroopMovementEvent : ITinyMessage {

    public object Sender { get; private set; }
    public Planet StartPlanet { get; private set; }
    public Planet TargetPlanet { get; private set; }
    public int ShipCount { get; private set; }

    public NewTroopMovementEvent(object sender, Planet startPlanet, Planet targetPlanet, int shipCount) {
        Sender = sender;
        StartPlanet = startPlanet;
        TargetPlanet = targetPlanet;
        ShipCount = shipCount;
    }
    
}

