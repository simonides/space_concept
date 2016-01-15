using UnityEngine;
using System.Collections;
using System;

public class BasicAI : AiPlayer {

    public override void PerformNextMovement() {
        Debug.Log("The AI " + Name + " does nothing, because it does not know what to do. This AI is stupid. It has no brain.");
        //throw new NotImplementedException();
    }
}
