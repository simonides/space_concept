using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyMessenger;
using System;

public class GameState : MonoBehaviour {

    // ****    CONFIGURATION    **** //
    public GameObject troopPrefab;
    public int MINIMUM_DAY_DURATION_IN_MS = 300;    // A next-day-switch can not happen faster than this variable

    // ****  ATTACHED OBJECTS   **** //

    // ****     ALLOCATORS      **** //
    PoolAllocator<TroopData> TroopDataPool;              // Pool allocator for the troop data
    PoolAllocator<GameObject> TroopPool;                 // Pool allocator for the troop Objects

    // ****                     **** //

    GameStateData gameStateData;        // entity
    List<Troop> troops;                 // troop objects

    DateTime lastDayChange = DateTime.Now;     // The time when the current day started


    void Awake() {        
    }


    void Start() {
        TroopDataPool = new PoolAllocator<TroopData>(
            () => { return new TroopData(); });

        TroopPool = new PoolAllocator<GameObject>(
        () => {
            GameObject gObj = GameObject.Instantiate(troopPrefab) as GameObject;
            gObj.SetActive(false);
            //gObj.transform.SetParent(objectPool, false);
            return gObj;
        }
        );

        InitEventSubscriptions();
    }


    public void Init(GameStateData gameState) {
        gameStateData = gameState;
    }


    

    void InitEventSubscriptions() {
        MessageHub.Subscribe<NextDayRequest>(NextDayRequest);
        MessageHub.Subscribe<NextDay>(NextDay);
    }



    void NextDayRequest(NextDayRequest evt) {
        TimeSpan time = DateTime.Now.Subtract(lastDayChange);
        if (time.TotalMilliseconds > MINIMUM_DAY_DURATION_IN_MS) {
            MessageHub.Publish(new NextDay(this));
        }
    } 

    private void NextDay(NextDay evt) {
        lastDayChange = DateTime.Now;
        gameStateData.NextDay();
        UnityEngine.Debug.Log("Good morning! We have day " + gameStateData.CurrentDay + " now!");
    }


    //void NewTroopMovement(TroopData troopData) {
    //    Troop troop = TroopPool.Get();
    //    troop.Init(troopData);
    //    //TODO: initialise graphical representation
    //    TroopMovements.Add(troop);
    //}
    


}
