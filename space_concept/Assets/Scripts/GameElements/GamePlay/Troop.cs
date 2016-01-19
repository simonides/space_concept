using UnityEngine;
using System.Collections;
using System;

//[RequireComponent(typeof(SpriteRenderer))]
public class Troop : MonoBehaviour {

    // ****  ATTACHED OBJECTS   **** //
    Transform spaceshipTextureTransform;
    SpriteRenderer spaceshipRenderer;
    TextMesh shipcountText;
    // ****                     **** //

    public Animator animator;

    public TroopData troopData { get; private set; }
    private float FlyAnimationSpeed = 1f;

    private Vector3 temporaryMoveToPos;

    //private Vector2 oldPosition;
    //private Vector2 newPosition;
    //float positionProgress; // 0-1

    public void Awake() {
        if(spaceshipTextureTransform != null || this.transform.childCount == 0) {
            return;
        }
        spaceshipTextureTransform = this.transform.FindChild("SpriteHolder");
        if (spaceshipTextureTransform == null) {
            throw new MissingComponentException("Unable to find child of name SpriteHolder on Troop.");
        }
        spaceshipRenderer = spaceshipTextureTransform.gameObject.GetComponent<SpriteRenderer>();
        if (spaceshipRenderer == null) {
            throw new MissingComponentException("Unable to find SpriteRenderer on Troop. The game object 'SpriteHolder' should have a sprite renderer attached.");
        }
        shipcountText = GetComponentInChildren<TextMesh>();
        if (shipcountText == null) {
            throw new MissingComponentException("Unable to find TextMesh on Troop to print ship count.");
        }
    }

    // Set me after the object has been enabled!
    public void SetSprite()
    {
        if (troopData.ShipCount > 50)
        {
            animator.SetInteger("ShipType", 1);
            Debug.Log("Ship type: " + animator.GetInteger("ShipType"));
        }
        else
        {
            animator.SetInteger("ShipType", 0);
        }
    }

    public void Init(int currentDay, TroopData troop) {

        troopData = troop;
        //Vector2 direction = troopData.TargetPlanet.Position - troopData.StartPlanet.Position;
        //direction.normalized * troopData.
        this.gameObject.SetActive(true);
        this.transform.localPosition = troop.StartPlanet.Position;


        UpdatePosition(currentDay);   // sets the target progress
        //todo!


        this.name = GetNameForTroopGameObject(troopData);

        shipcountText.text = "" + troop.ShipCount;
        spaceshipTextureTransform.rotation = Quaternion.FromToRotation(Vector3.up, troop.TargetPlanet.Position - troop.StartPlanet.Position);
        this.transform.localScale = new Vector3(15, 15, 1);
    }

    public void UpdatePosition(int currentDay) {

        int daysRemaining = troopData.ArrivalTime - currentDay;
        int daysTraveled = troopData.TravelTime- daysRemaining;
        float distanceBetweenPlanets = (troopData.TargetPlanet.Position - troopData.StartPlanet.Position).magnitude;

        temporaryMoveToPos
            = Vector3.MoveTowards(troopData.StartPlanet.Position,troopData.TargetPlanet.Position,
                (distanceBetweenPlanets / troopData.TravelTime) * daysTraveled);
        temporaryMoveToPos.z = -15f;
        Debug.Log(temporaryMoveToPos);



        //oldPosition = this.transform.localPosition;
        //positionProgress = 0;

        //int remainingDays = troopData.ArrivalTime - currentDay;

        //float targetProgress = 1f - ((float)remainingDays / troopData.TravelTime);  //  [0..1]
        //if ( targetProgress > 1)
        //{
        //    Debug.LogWarning("Ships have not been destroyed, although they already reached the destination");
        //    targetProgress = 1;
        //}
        //Debug.Assert(targetProgress >= 0 && targetProgress <= 1);

        //Vector2 direction = troopData.TargetPlanet.Position - troopData.StartPlanet.Position;
        //Vector2 direction_norm = direction.normalized;
        //Vector2 reversedir = direction_norm * troopData.TargetPlanet.Diameter;

        //Debug.Assert(reversedir.magnitude <= direction.magnitude);
       
        //newPosition = oldPosition  + (direction) * targetProgress;
    }

    public void Update() {

        //_shipsToMoveInUpdate[i].GraphicalOutput.transform.localPosition = Vector3.Lerp(_shipsToMoveInUpdate[i].GraphicalOutput.transform.localPosition,
        //                                                     _shipsToMoveInUpdate[i].TempMoveToPosition, ANIMATIONS_SPEED * Time.smoothDeltaTime);


        Vector3 currentPosition = this.transform.localPosition;
        Debug.Log("current pos: " + currentPosition);
        currentPosition = Vector3.Lerp(currentPosition, temporaryMoveToPos, FlyAnimationSpeed * Time.smoothDeltaTime);
        //Vector2 targetVec = temporaryMoveToPos - currentPosition;
        //if (targetVec.magnitude > 10f)
        //{
        //    Debug.Log("Arrived");
        //    return;
        //}

        this.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, -15);
        //this.transform.localPosition = new Vector3(temporaryMoveToPos.x, temporaryMoveToPos.y, -15);

        ////todo figure out how this should work
        //float diff = Mathf.Abs(_shipsToMoveInUpdate[i].GraphicalOutput.transform.localPosition.sqrMagnitude - _shipsToMoveInUpdate[i].Destination.position.sqrMagnitude);
        //if (diff < 1)  // Close enough
        //{
        //    if (_shipsToMoveInUpdate[i].ArrivalDay <= StateManager.CurrentDay)
        //    {
        //        Destroy(_shipsToMoveInUpdate[i].GraphicalOutput);
        //    }
        //    _shipsToMoveInUpdate.RemoveAt(i);
        //}

        //float deltaInSec = Time.deltaTime;
        //if(deltaInSec > 0.05) {
        //    deltaInSec = 0.005f;
        //}
        //positionProgress += deltaInSec * FlyAnimationSpeed;

        //if(positionProgress >= 1){
        //    positionProgress = 1;
        //    //oldPosition = newPosition;
        //}

        //Vector3 position = Vector2.Lerp(oldPosition, newPosition, positionProgress );
        //position.z = -15;   // In front of planets
        //this.transform.localPosition = position;       
    }


    private string GetNameForTroopGameObject(TroopData troopData) {
        try {
            return troopData.Owner.Name + " (" + troopData.ShipCount + "): " + troopData.StartPlanet.Name + " > " + troopData.TargetPlanet.Name;
        } catch (NullReferenceException) {
            return "Troop of " + troopData.ShipCount + " ships";
        }
    }



}
