using UnityEngine;
using System.Collections;
using TinyMessenger;

public class EvaluationRequestEvent : ITinyMessage {

    public object Sender { get; private set; }
    public NextDayEvent nextDayEvent { get; private set; }

    public EvaluationRequestEvent(object sender, NextDayEvent nextDayEvent) {
        Sender = sender;
        this.nextDayEvent = nextDayEvent;
    }

    public int GetCurrentDay() {
        return this.nextDayEvent.GetCurrentDay();
    }
}
