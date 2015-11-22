using UnityEngine;
using System.Collections.Generic;

public class AirTrafficControl : MonoBehaviour {

    // ****    CONFIGURATION    **** //
    public GameObject troopPrefab;

    // ****  ATTACHED OBJECTS   **** //
    GameStateData gameStateData;
    GameObject pooledGameObjectHolder;

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
            () => { return new TroopData(); }
        );

        troopPool = new PoolAllocator<GameObject>(
            () => {
                GameObject gObj = GameObject.Instantiate(troopPrefab) as GameObject;
                gObj.SetActive(false);
                gObj.transform.SetParent(pooledGameObjectHolder.transform, false);
                return gObj;
            }
        );

        InitEventSubscriptions();
    }


    public void Init(AirTrafficData airTrafficData) {
        this.airTrafficData = airTrafficData;
    }




    void InitEventSubscriptions() {
        MessageHub.Subscribe<NextDayEvent>(NextDay);
        MessageHub.Subscribe<NewTroopMovementEvent>(NewTroopMovement);
    }

    private void NextDay(NextDayEvent evt) {
        //TODO: troop movements
    }

    private void NewTroopMovement(NewTroopMovementEvent evt) {
        TroopData troopData = troopDataPool.Get();
        troopData.Init(null, null, evt.ShipCount, gameStateData.CurrentDay);
        //troopData.Init(evt.StartPlanet.planetData, evt.TargetPlanet.planetData, evt.ShipCount, gameStateData.CurrentDay);
        airTrafficData.AddTroopMovement(troopData);

        GameObject troopObject = troopPool.Get();
        Troop troop = troopObject.GetComponent<Troop>();
        if (troop == null) {
            throw new MissingComponentException("A pooled Troop-GameObect doesn't have a Troop-Component. Each Troop-GO should have a Troop Script attached.");
        }

        troop.Init(troopData);

        //TODO: initialise graphical representation

        troops.Add(troopObject);

        Debug.Log("New troop movement confirmed: " + troopData.ToString());
    }
}
