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
    public EvaluationOutcome Outcome { get; /*private*/ set; } // temporal for testing the setter is enabled


    public PlayerData ShipOwner { get; private set; }
    public PlayerData PlanetOwner { get; private set; }

    public int IncomingShips { get; private set; }
    public int ShipsOnPlanetAfterEvent { get; private set; }
    public int LostShips { get; private set; }


    public float Importance { get; private set; }                       //0: not important; 50: normal; 100: important;


    public PlanetData Planet { get; private set; }
    public PlayerData OriginalOwner { get; private set; }



    public static AttackEvaluation Supply(TroopData troop, int shipsOnPlanetAfterEvent, int lostShips) {
        AttackEvaluation evaluation = new AttackEvaluation();
        evaluation.Type = EvaluationType.Supply;
        if (lostShips > 0) {
            evaluation.Outcome = EvaluationOutcome.Lost;
            evaluation.Importance = Mathf.Lerp(50, 100, Mathf.Min(1, lostShips / 500));
        } else {
            evaluation.Outcome = EvaluationOutcome.Success;
            evaluation.Importance = 40;
        }
        evaluation.ShipOwner = troop.Owner;
        evaluation.PlanetOwner = troop.Owner;
        evaluation.IncomingShips = troop.ShipCount;
        evaluation.ShipsOnPlanetAfterEvent = shipsOnPlanetAfterEvent;
        evaluation.LostShips = lostShips;
        evaluation.Planet = troop.TargetPlanet;
        evaluation.OriginalOwner = troop.Owner;
        Debug.Log("A supply of " 
            + evaluation.IncomingShips 
            + " reached planet "
            + evaluation.Planet.Name + " - " + lostShips 
            + " ships were lost due to full hangar");
        return evaluation;
    }




    public static AttackEvaluation Attack(TroopData troop, PlayerData oldOwner, PlayerData newOwner, int shipsOnPlanetAfterEvent, int lostShipsByOwner, int lostShipsByAttacker, int lostShipsByLanding) {
        AttackEvaluation evaluation = new AttackEvaluation();
        if (troop.Owner.IsHumanPlayer) {
            evaluation.Type = EvaluationType.AttackedPlanet;
            evaluation.LostShips = lostShipsByAttacker + lostShipsByLanding;
            if (newOwner == troop.Owner) {                   // Captured
                if (lostShipsByLanding > 0) {
                    evaluation.Outcome = EvaluationOutcome.Neutral;     // Bitterer Beigeschmack
                } else {
                    evaluation.Outcome = EvaluationOutcome.Success;
                }
                evaluation.Importance = 80;
            } else {
                evaluation.Outcome = EvaluationOutcome.Lost;
                evaluation.Importance = 55;
            }
        } else if (oldOwner != null && oldOwner.IsHumanPlayer) {
            evaluation.Type = EvaluationType.GotAttacked;
            evaluation.LostShips = lostShipsByOwner;
            if (newOwner == troop.Owner) {                   // Captured
                evaluation.Outcome = EvaluationOutcome.Lost;
                evaluation.Importance = 80;
            } else {
                evaluation.Outcome = EvaluationOutcome.Success;
                evaluation.Importance = 60;
            }
       } else {
            if (oldOwner != newOwner) {
                evaluation.Type = EvaluationType.CaptureViewer;
                evaluation.Importance = 15;
            } else {
                evaluation.Type = EvaluationType.AttackViewer;
                evaluation.Importance = 5;
            }
            evaluation.LostShips = lostShipsByOwner + lostShipsByAttacker + lostShipsByLanding;     // A viewer is only intersted in the total number of ships destroyed
            evaluation.Outcome = EvaluationOutcome.Neutral;
        }
                
        evaluation.ShipOwner = troop.Owner;
        evaluation.PlanetOwner = newOwner;
        evaluation.IncomingShips = troop.ShipCount;
        evaluation.ShipsOnPlanetAfterEvent = shipsOnPlanetAfterEvent;
                
        evaluation.Planet = troop.TargetPlanet;
        evaluation.OriginalOwner = oldOwner;

        Debug.Log("An attack of " + evaluation.IncomingShips 
            + " ships reached planet " + evaluation.Planet.Name 
            + " - Losses:  owner = " + lostShipsByOwner 
            + ", attacker = " + (lostShipsByAttacker + lostShipsByLanding) 
            + "; New owner: " + (newOwner == null ? "neutral" : newOwner.Name) 
            + "");
        return evaluation;
    }
}
