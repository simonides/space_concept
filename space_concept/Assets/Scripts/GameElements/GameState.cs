using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviour {


    public int CurrentDay { get; private set; }     // Current day, starting at 1

    PoolAllocator<Troop> TroopPool;                 // Pool allocator for the troops

    List<Troop> TroopMovements;                     // All troops that are currently moving through the map



    void Start() {
        CurrentDay = 1;
        TroopPool = new PoolAllocator<Troop>(  ()=> new Troop()   );
    }
    
   
   

    void NewTroopMovement(TroopData troopData) {
        Troop troop = TroopPool.Get();
        troop.Init(troopData);
        //TODO: initialise graphical representation
        TroopMovements.Add(troop);
    }



    void NextDay() {
        ++CurrentDay;
        //TODO: go through all movements and evaluate them. Then push the used ones back into the pool
        
    }

}
