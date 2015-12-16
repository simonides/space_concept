using UnityEngine;
using System.Collections.Generic;
using TinyMessenger;

public class TroopEvaluationResultEvent : ITinyMessage {

    public object Sender { get; private set; }
    public List<AttackEvaluation> EvaluationData { get; private set; }

    public TroopEvaluationResultEvent(object sender, List<AttackEvaluation> evaluationData) {
        Sender = sender;
        EvaluationData = evaluationData;
    }
    
}
