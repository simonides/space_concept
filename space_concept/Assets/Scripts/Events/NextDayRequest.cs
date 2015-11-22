using UnityEngine;
using System.Collections;
using TinyMessenger;
using System;


// The user wants the next day to be started (DOES NOT START A NEW DAY)
// This event needs to be approved by the GameState object (eg. to avoid multiple button clicks)


public class NextDayRequest : TinyMessageBase {

    public NextDayRequest(object sender)
        : base(sender)  {
    }

}
