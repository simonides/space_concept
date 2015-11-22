//using UnityEngine;
//using System.Collections;
//using TinyMessenger;

//public class NewTroopMovementEvent : ITinyMessage {

//    public object Sender { get; private set; }

//    public Planet StartPlanet { get; private set; }
//    public Planet TargetPlanet { get; private set; }
//    public int ShipCount { get; private set; }

//    private NewTroopMovement() {
//        Reset();
//    }

//    private void Reset() {
//        Sender = null;
//        StartPlanet = null;
//        TargetPlanet = null;
//        ShipCount = 0;
//    }

//    private void Init(object sender, Planet startPlanet, Planet targetPlanet, int shipCount) {
//        Sender = sender;
//        StartPlanet = startPlanet;
//        TargetPlanet = targetPlanet;
//        ShipCount = shipCount;
//    }



//    static PoolAllocator<NewTroopMovement> poolAllocator;
//    private static void InitPoolAllocator() {
//        poolAllocator = new PoolAllocator<NewTroopMovement>(
//                () => { return new NewTroopMovement(); }
//        );
//    }

//    public static NewTroopMovement New(object sender, Planet startPlanet, Planet targetPlanet, int shipCount) {
//        if(poolAllocator == null) {
//            InitPoolAllocator();
//        }
//        NewTroopMovement obj = poolAllocator.Get();
//        obj.Init(sender, startPlanet, targetPlanet, shipCount);
//        return obj;
//    }

//    public void Completed() {
//        Reset();
//        poolAllocator.PutBack(this);
//    }

//}

using UnityEngine;
using System.Collections;
using TinyMessenger;

public class NewTroopMovementEvent : ITinyMessage {

    public object Sender { get; private set; }
    public Planet StartPlanet { get; private set; }
    public Planet TargetPlanet { get; private set; }
    public int ShipCount { get; private set; }

    public NewTroopMovementEvent(object sender, Planet startPlanet, Planet targetPlanet, int shipCount) {
        Sender = sender;
        StartPlanet = startPlanet;
        TargetPlanet = targetPlanet;
        ShipCount = shipCount;
    }
    
}

