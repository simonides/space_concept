using UnityEngine;
using System.Collections;
using TinyMessenger;

// Starts a new day

public class NextDayEvent : GenericTinyMessage<int> {

    public NextDayEvent(object sender, int currentDay)
        : base(sender, currentDay)
    {
        // We now have a public string property called Content
    }

    public int GetCurrentDay() {
        return this.Content;
    }
}
