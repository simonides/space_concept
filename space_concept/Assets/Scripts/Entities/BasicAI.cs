using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class BasicAI : AiPlayer {
    public int thisVariableDoesNothingAndIsOnlyNeededNowBecauseOtherwiseSerialisationWouldntWork;

    public override void PerformNextMovement() {
        Debug.Log("The AI " + Name + " does nothing, because it does not know what to do. This AI is stupid. It has no brain.");
        //throw new NotImplementedException();
    }
}
