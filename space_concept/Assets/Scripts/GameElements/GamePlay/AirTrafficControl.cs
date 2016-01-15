﻿using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

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

    // Troop deletion:
    float deleteDelay;              // Number of seconds when the troopsToDelete will finally be deleted
    List<Troop> troopsToDelete;


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

    void Start(){ 
        GameState gameState = Camera.main.GetComponent<GameState>();
        if (gameState == null) {
            throw new MissingComponentException("Unable to find GameState. The 'GameState' script needs to be attached to the main Camera.");
        }
        gameStateData = gameState.gameStateData;
        if (gameStateData == null) {
            throw new MissingReferenceException("The gameStateData has not been initialised yet.");
        }

        //PreparePools();

        InitEventSubscriptions();
    }

    void PreparePools() {
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
    }

    void Update() {
        deleteDelay -= Time.deltaTime;
        if(deleteDelay < 0 && troopsToDelete != null) {
            DisposeOfTroops(troopsToDelete);
        }
    }

    public void Init(int currentDay, AirTrafficData airTrafficData) {
        PreparePools();

        this.airTrafficData = airTrafficData;
        foreach(TroopData troopData  in airTrafficData.airTraffic) {
            InitGraphicalTroopMovement(currentDay, troopData);
        }
    }
    

    void InitEventSubscriptions() {
        MessageHub.Subscribe<NewTroopMovementEvent>(NewTroopMovement);
        MessageHub.Subscribe<EvaluationRequestEvent>(EvaluateAttacks);
    }


    // Returns all troops that arrive on the given day
    private List<Troop> GetTroopsForDay(int arrivalDay) {
        List<Troop> troopsForDay = new List<Troop>();
        foreach(GameObject troopGO in troops) {
            Troop troop = troopGO.GetComponent<Troop>();
            if (troop.troopData.ArrivalTime <= arrivalDay) {
                troopsForDay.Add(troop);
            }
        }
        return troopsForDay;
    }






    private void NewTroopMovement(NewTroopMovementEvent evt) {
        TroopData troopData = troopDataPool.Get();
        troopData.Init(evt.StartPlanet.planetData, evt.TargetPlanet.planetData, evt.ShipCount, gameStateData.CurrentDay);
        airTrafficData.AddTroopMovement(troopData);

        InitGraphicalTroopMovement(gameStateData.CurrentDay, troopData);

        Debug.Log("New troop movement confirmed: " + troopData.ToString());
    }

    private void InitGraphicalTroopMovement(int currentDay, TroopData troopData) {
        GameObject troopObject = troopPool.Get();
        Troop troop = troopObject.GetComponent<Troop>();
        if (troop == null) {
            throw new MissingComponentException("A pooled Troop-GameObect doesn't have a Troop-Component. Each Troop-GO should have a Troop Script attached.");
        }

        troop.Init(currentDay, troopData);

        PlanetData startPlanet = troopData.StartPlanet;
        PlanetData targetPlanet = troopData.TargetPlanet;

        Vector2 direction = (targetPlanet.Position - startPlanet.Position);
        direction.Normalize();

        troopObject.transform.SetParent(space.transform);
        troopObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        troopObject.transform.localScale = new Vector3(15, 15, 1);

        Vector3 initialPosition = startPlanet.Position;
        initialPosition.z = -15;   // In front of planets
        troopObject.transform.localPosition = initialPosition;

        Vector2 startPosition = initialPosition;
        startPosition += direction* startPlanet.Diameter / 2;
        troop.StartPosition = startPosition;

        Vector2 targetPosition = targetPlanet.Position;
        //targetPosition -= direction * targetPlanet.Diameter / 2;
        troop.TargetPosition = targetPosition;
        
        troopObject.SetActive(true);
        troops.Add(troopObject);
    }



    private void EvaluateAttacks(EvaluationRequestEvent evt) {
        int currentDay = evt.GetCurrentDay();

        // 1. Move troops:
        AnimateTroopObjects(currentDay);

        // 2. Avaluate troops:
        List<Troop> todaysTroops = GetTroopsForDay(currentDay);
        Debug.Log("Day " + currentDay + ": " + todaysTroops.Count() + " troops arrived.");


        var evaluation = PerformTroopEvaluation(todaysTroops);
        PublishTroopEvaluation(evaluation);

        foreach (Troop troop in todaysTroops) {
            troops.Remove(troop.gameObject);
        }
        MarkForDeletion(todaysTroops);        
    }

    private void AnimateTroopObjects(int currentDay) {
        foreach (GameObject troopGO in troops) {
            Debug.Assert(troopGO != null);
            Troop troop = troopGO.GetComponent<Troop>();
            troop.UpdatePosition(currentDay);
        }
    }

    private List<AttackEvaluation> PerformTroopEvaluation(List<Troop> todaysTroops) {
        Dictionary<PlanetData, AttackEvaluation> evaluation = new Dictionary<PlanetData, AttackEvaluation>();   // Order by planet, so that they are grouped

        foreach(Troop troop in todaysTroops) {
            TroopData data = troop.troopData;
            AttackEvaluation eval = data.TargetPlanet.EvaluateIncomingTroop(data);
            evaluation.Add(data.TargetPlanet, eval);
        }

        return evaluation.Values.ToList();
    }

    private void PublishTroopEvaluation(List<AttackEvaluation> evaluations) {
        TroopEvaluationResultEvent evt = new TroopEvaluationResultEvent(this, evaluations);
        MessageHub.Publish<TroopEvaluationResultEvent>(evt);
        MessageHub.Publish<PlanetUpdateEvent>(new PlanetUpdateEvent(this));     // Update graphical planet representations
    }

    private void MarkForDeletion(List<Troop> troopsForDeletion) {
        if (troopsToDelete != null) {
            DisposeOfTroops(troopsToDelete);
        }
        troopsToDelete = troopsForDeletion;
        deleteDelay = 5; // delete in 5 seconds
    }

    private void DisposeOfTroops(List<Troop> troopsToDelete) {
        foreach (Troop troop in troopsToDelete) {
            troop.gameObject.SetActive(false);
            troopDataPool.PutBack(troop.troopData);
            troopPool.PutBack(troop.gameObject);
        }
        troopsToDelete.Clear();
    }

    public AirTrafficData GetData() {
        return airTrafficData;
    }
}
