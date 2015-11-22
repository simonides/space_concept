using UnityEngine;
using System.Collections.Generic;
using System;

public class AirTrafficControl : MonoBehaviour {

    // ****    CONFIGURATION    **** //
    public GameObject troopPrefab;

    // ****  ATTACHED OBJECTS   **** //
    GameStateData gameStateData;
    GameObject pooledGameObjectHolder;
    Space space;

    // ****     ALLOCATORS      **** //
    PoolAllocator<TroopData> troopDataPool;              // Pool allocator for the troop data
    PoolAllocator<GameObject> troopPool;                 // Pool allocator for the troop Objects

    // ****                     **** //

    AirTrafficData airTrafficData;        // entity
    List<GameObject> troops;              // troop objects (with attached Troop script)


    void Awake() {    
        pooledGameObjectHolder = GameObject.Find("PooledGameObjects");
        if (pooledGameObjectHolder == null) {
            throw new MissingComponentException("Unable to find PooledGameObjects There should be an empty game object that holds the pooled data.");
        }

        space = GameObject.Find("Space").GetComponent<Space>();
        if (space == null) {
            throw new MissingComponentException("Unable to find Space. The 'Space' game object also needs to be added to the level and have the space script attached.");
        }
    }

    void Start() {
        GameState gameState = Camera.main.GetComponent<GameState>();
        if (gameState == null) {
            throw new MissingComponentException("Unable to find GameState. The 'GameState' script needs to be attached to the main Camera.");
        }
        gameStateData = gameState.gameStateData;
        if (gameStateData == null) {
            throw new MissingReferenceException("The gameStateData has not been initialised yet.");
        }

        troops = new List<GameObject>();

        troopDataPool = new PoolAllocator<TroopData>(
            () => { return new TroopData(); },
            (TroopData troop) => { troop.Init(null, null, 0, 0); }
        );

        troopPool = new PoolAllocator<GameObject>(
            () => {
                GameObject gObj = GameObject.Instantiate(troopPrefab) as GameObject;
                return gObj;
            },
            (GameObject gObj) => {
                gObj.SetActive(false);
                gObj.name = "Pooled troop";
                gObj.transform.SetParent(pooledGameObjectHolder.transform, false);
            }
        );

        InitEventSubscriptions();
    }


    public void Init(AirTrafficData airTrafficData) {
        this.airTrafficData = airTrafficData;
        foreach(TroopData troopData  in airTrafficData.airTraffic) {
            InitGraphicalTroopMovement(troopData);
        }
    }
    

    void InitEventSubscriptions() {
        MessageHub.Subscribe<NewTroopMovementEvent>(NewTroopMovement);
        MessageHub.Subscribe<NextDayEvent>(NextDay);
    }



    private void NewTroopMovement(NewTroopMovementEvent evt) {
        TroopData troopData = troopDataPool.Get();
        troopData.Init(null, null, evt.ShipCount, gameStateData.CurrentDay);
        //troopData.Init(evt.StartPlanet.planetData, evt.TargetPlanet.planetData, evt.ShipCount, gameStateData.CurrentDay);
        airTrafficData.AddTroopMovement(troopData);

        InitGraphicalTroopMovement(troopData);

        Debug.Log("New troop movement confirmed: " + troopData.ToString());
    }

    private void InitGraphicalTroopMovement(TroopData troopData) {
        GameObject troopObject = troopPool.Get();
        Troop troop = troopObject.GetComponent<Troop>();
        if (troop == null) {
            throw new MissingComponentException("A pooled Troop-GameObect doesn't have a Troop-Component. Each Troop-GO should have a Troop Script attached.");
        }


        troopObject.transform.SetParent(space.transform);
        troop.Init(troopData);
        troopObject.SetActive(true);



        //TODO: initialise graphical representation

        troops.Add(troopObject);
    }


    private void NextDay(NextDayEvent evt) {
        //TODO: troop movements
    }
}
