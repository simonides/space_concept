using UnityEngine;
using System.Collections;
using System;

//[RequireComponent(typeof(SpriteRenderer))]
public class Troop : MonoBehaviour
{

    // ****  ATTACHED OBJECTS   **** //
    Transform spaceshipTextureTransform;
    SpriteRenderer spaceshipRenderer;
    TextMesh shipcountText;
    // ****                     **** //

    public Animator animator;

    public TroopData troopData { get; private set; }
    private float FlyAnimationSpeed = .8f;

    private Vector3 temporaryMoveToPos;

    //private Vector2 oldPosition;
    //private Vector2 newPosition;
    //float positionProgress; // 0-1

    public void Awake()
    {
        if (spaceshipTextureTransform != null || this.transform.childCount == 0)
        {
            return;
        }
        spaceshipTextureTransform = this.transform.FindChild("SpriteHolder");
        if (spaceshipTextureTransform == null)
        {
            throw new MissingComponentException("Unable to find child of name SpriteHolder on Troop.");
        }
        spaceshipRenderer = spaceshipTextureTransform.gameObject.GetComponent<SpriteRenderer>();
        if (spaceshipRenderer == null)
        {
            throw new MissingComponentException("Unable to find SpriteRenderer on Troop. The game object 'SpriteHolder' should have a sprite renderer attached.");
        }
        shipcountText = GetComponentInChildren<TextMesh>();
        if (shipcountText == null)
        {
            throw new MissingComponentException("Unable to find TextMesh on Troop to print ship count.");
        }
    }

    // Set me after the object has been enabled!
    public void SetSprite()
    {
        if (troopData.ShipCount < 10)
        {
            animator.SetInteger("ShipType", 0);
        }
        else if (troopData.ShipCount < 100)
        {
            animator.SetInteger("ShipType", 1);
        }
        else if (troopData.ShipCount < 500)
        {
            animator.SetInteger("ShipType", 2);
        }
        else if (troopData.ShipCount < 1000)
        {
            animator.SetInteger("ShipType", 3);
        }
        else if (troopData.ShipCount < 2000)
        {
            animator.SetInteger("ShipType", 4);
        } else {
            animator.SetInteger("ShipType", 5);
        }
    }

    public void Init(int currentDay, TroopData troop)
    {

        troopData = troop;
        this.transform.localScale = new Vector3(15, 15, 1);
        this.transform.localPosition = troop.StartPlanet.Position;

        UpdatePosition(currentDay);   // sets the target progress
        //todo!


        this.name = GetNameForTroopGameObject(troopData);

        shipcountText.text = "" + troop.ShipCount;
        spaceshipTextureTransform.rotation = Quaternion.FromToRotation(Vector3.up, troop.TargetPlanet.Position - troop.StartPlanet.Position);
    }

    public void UpdatePosition(int currentDay)
    {

        int daysRemaining = troopData.ArrivalTime - currentDay;
        int daysTraveled = troopData.TravelTime - daysRemaining;
        float distanceBetweenPlanets = (troopData.TargetPlanet.Position - troopData.StartPlanet.Position).magnitude;

        temporaryMoveToPos
            = Vector3.MoveTowards(troopData.StartPlanet.Position, troopData.TargetPlanet.Position,
                (distanceBetweenPlanets / troopData.TravelTime) * daysTraveled);
        temporaryMoveToPos.z = -15f;
        Debug.Log("speed: " + FlyAnimationSpeed);
    }

    public void Update()
    {
        float deltaTime = Time.smoothDeltaTime;
        if (Time.smoothDeltaTime > 0.051f)
        {
            deltaTime = 0.005f;
        }
        Vector3 currentPosition = this.transform.localPosition;
        currentPosition = Vector3.Lerp(currentPosition, temporaryMoveToPos, FlyAnimationSpeed * deltaTime);

        Vector2 targetVec = temporaryMoveToPos - currentPosition;
        float shipSpeed = targetVec.magnitude;
        animator.SetFloat("ShipSpeed", shipSpeed);
        if (shipSpeed < 5.1f)
        {
            animator.SetFloat("ShipSpeed", 0);
            return;
        }
        this.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, -15);
    }


    private string GetNameForTroopGameObject(TroopData troopData)
    {
        try
        {
            return troopData.Owner.Name + " (" + troopData.ShipCount + "): " + troopData.StartPlanet.Name + " > " + troopData.TargetPlanet.Name;
        }
        catch (NullReferenceException)
        {
            return "Troop of " + troopData.ShipCount + " ships";
        }
    }



}
