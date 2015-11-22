using UnityEngine;
using System.Collections.Generic;
using TinyMessenger;

public class TroopEvaluationResultEvent : ITinyMessage {

    public object Sender { get; private set; }
    public List<PlanetEventEvaluationResult> EvaluationData { get; private set; }

    public TroopEvaluationResultEvent(object sender, List<PlanetEventEvaluationResult> evaluationData) {
        Sender = sender;
        EvaluationData = evaluationData;
    }
    
}
