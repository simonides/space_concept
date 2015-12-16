using UnityEngine;
using System.Collections;
using TinyMessenger;



public enum EvaluationType {
    Supply,           //A planet received supply ships
    AttackedPlanet,   //The player attacked another planet
    GotAttacked,      //A planet of the player got attacked by another planet
    CaptureViewer,    //Another player attacked another planet and won - the ownership changed.
    AttackViewer,     //Another player attacked another planet
};



public enum EvaluationOutcome {
    Success,        //Good for the player
    Neutral,        //The planet got neutral. Neither good, nor bad
    Lost,           //Bad for the player
};




public class AttackEvaluation {
    public EvaluationType Type { get; private set; }
    public EvaluationOutcome Outcome { get; private set; }

    
    public PlayerData ShipOwner { get; private set; }
    public PlayerData PlanetOwner { get; private set; }

    public int IncomingShips { get; private set; }                      
    public int ShipsOnPlanetAfterEvent { get; private set; }         
    public int LostShips { get; private set; }

    
    public float Importance { get; private set; }                       //0: not important; 50: normal; 100: important;

    
    public Planet planet;
    public PlayerData originalOwner;



    public static AttackEvaluation Supply(TroopData troop, int ShipsOnPlanetAfterEvent, int LostShips) {
        AttackEvaluation evaluation = new AttackEvaluation();
        evaluation.Type = EvaluationType.Supply;
        evaluation.Outcome = EvaluationOutcome.Success;
        evaluation.ShipOwner = troop.Owner;
        evaluation.PlanetOwner = troop.Owner;
        evaluation.IncomingShips = troop.ShipCount;
        evaluation.ShipsOnPlanetAfterEvent = ShipsOnPlanetAfterEvent;
        evaluation.LostShips = LostShips;
        //  evaluation.planet = troop.TargetPlanet;
        //TODO: imeplement
        return evaluation;
    }
}
