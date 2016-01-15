using UnityEngine;
using System.Collections;
using TinyMessenger;

// This event is sent if the planet entities should update their graphical representations according to the planetData
public class PlanetUpdateEvent : ITinyMessage {
    public object Sender { get; private set; }

    public PlanetUpdateEvent(object sender) {
        Sender = sender;
    }
}
