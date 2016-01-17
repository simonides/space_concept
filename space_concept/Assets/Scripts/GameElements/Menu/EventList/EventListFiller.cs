using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
/*

Wenn man verstärkung bekommt (von sich selbst):     EvaluationEventType = Supply
    You received
     +134 ships                                 usedShips-lostShips

Nur, Wenn der Hangar voll is:                       EvaluationEventOutcome = Lost
    Hangar Full!
    6 ships lost                                lostShips

Immer sichtbar:
 250 ships available                            shipsOnPlanet

Wenn man selber jemand anderen angegriffen hat:          evaluationEventType = AttackedPlanet
     You attacked
      playername                                otherPlayer.  Wenn null, dann den Text "a neutral planet" ausgeben
         with
       150 ships                                usedShips

Wenn man verloren hat ODER ein neutraler planet neutral bleibt:         EvaluationEventOutcome = Lost
         Lost!
There are no survivors.

Wenn der Planet neutral wurde:                    EvaluationEventOutcome = Neutral
         Lost!
The planet is now neutral

Wenn der Planet erobert wurde:                  evaluationEventOutcome = success
        Victory!
   80 ships survived.                         shipsOnPlanet



Wenn man angefriffen wird und überlebt hat:     evaluationEventType = GotAttacked, EvaluationEventOutcome = Success
      playername                             otherPlayer
     attacked with
       200 ships                              usedShips

       Survived!
  12 ships remaining                          shipsOnPlanet








Wenn man angefriffen wird und der Planet neutral wurde:       evaluationEventType = GotAttacked, EvaluationEventOutcome = Neutral
      playername                              otherPlayer
     attacked with
       200 ships                              usedShips

         Lost!
The planet is now neutral.



Wenn man angegriffen wird, und der Planet jetzt dem Gegner gehört:      evaluationEventType = GotAttacked, evaluationEventOutcome = Lost
      playername                            otherPlayer
       attacked

        Lost!
There are no survivors.




Wenn ein anderer Spieler einen anderen Planeten angegriffen hat, und gewonnen hat:              evaluationEventType = AttackViewer, evaluationEventOutcome = Lost
  playername                              otherPlayer
   attacked
  playername                              otherAttackedPlayer  (Wenn null: den string "neutral planet" ausgeben)
   and won.


Wenn ein anderer Spieler einen anderen Planeten angegriffen hat, und der jetzt neutral ist:      evaluationEventType = AttackViewer, evaluationEventOutcome = Neutral;
  playername                              otherPlayer
   attacked
  playername                            otherAttackedPlayer

The planet is now neutral.


*/
public class EventListFiller : MonoBehaviour
{
    // ****    CONFIGURATION    **** //

    // ****  ATTACHED OBJECTS   **** //
    GameObject pooledGameObjectHolder;


    // ****     ALLOCATORS      **** //
    PoolAllocator<GameObject> eventButtonPool;

    // ****                     **** //
    public GameObject Space;
    private Space space;

    public EventButton prefab;

    public GameObject sampleButton;
    public List<AttackEvaluation> itemList;

    public Text NoItemsInListText;
    private int lastRandom = -1;


    public Transform contentPanel;

    private SpriteAtlasAccesser planetAtlas;


    private string[] itemsEmptyTexts =
    {
    "Nothing happend. Oh, wait. Something did happen. Actually Simon dropped his bass.",
    "Nothing happened. Except: A bag of rice fell.",
    "Yo, what's up bro? Nothin' happened today. Send some ships, yo!",
    "Wazzaaapp! Nothing. Not a single ship arrived.",
    "Heeellooo? Is there anyone? It's so boring today, nothin's happenin.",
    "Nothing happened.The rooster crowed and noone heard it.",
    "Your enemy wanted to send some ships today. He forgot, so nothing happened.",
    "If you don't send some ships, nothing will ever happen... Nice."
    };


    void Awake()
    {
        space = Space.GetComponent<Space>();
        if (space == null)
        {
            throw new MissingComponentException("Unable to find Space.");
        }
        pooledGameObjectHolder = GameObject.Find("PooledGameObjects");
        if (pooledGameObjectHolder == null)
        {
            throw new MissingComponentException("Unable to find PooledGameObjects There should be an empty game object that holds the pooled data.");
        }
        planetAtlas = gameObject.GetComponent<SpriteAtlasAccesser>();
        if (planetAtlas == null)
        {
            throw new MissingComponentException("Unable to find SpriteAtlasAccesser component.");
        }

    }


    // Use this for initialization
    void Start()
    {
        eventButtonPool = new PoolAllocator<GameObject>(
            () =>
            {
                GameObject gObj = GameObject.Instantiate(sampleButton) as GameObject;
                return gObj;
            },
            (GameObject gObj) =>
            {
                gObj.SetActive(false);
                gObj.name = "Pooled EventButton";
                gObj.transform.SetParent(pooledGameObjectHolder.transform, false);
            }
            );
    }

    private void PopulateList()
    {
        SetNothingHappenedText();
        if (itemList == null) { return; }
        //Debug.Log("Listitems size: " + itemList.Count);

        // get items from the content panel
        var eventlistitems = contentPanel.GetComponentsInChildren<EventButton>();

        // put them back into the pool
        foreach (var item in eventlistitems)
        {
            eventButtonPool.PutBack(item.self_ref);
        }
        // detach them from the content panel
        contentPanel.transform.DetachChildren();

        //AttackEvaluation atev = new AttackEvaluation();
        //atev.Outcome = EvaluationOutcome.Lost;

        //AttackEvaluation atev1 = new AttackEvaluation();
        //atev1.Outcome = EvaluationOutcome.Neutral;

        //AttackEvaluation atev2 = new AttackEvaluation();
        //atev2.Outcome = EvaluationOutcome.Success;

        //itemList.Add(atev);
        //itemList.Add(atev1);
        //itemList.Add(atev2);



        // add new objects
        foreach (var item in itemList)
        {
            GameObject listItem = eventButtonPool.Get();
            listItem.SetActive(true);
            EventButton eventBtn = listItem.GetComponent<EventButton>();
            if (eventBtn == null)
            {
                Debug.Log("eventbutton was null");
                listItem.transform.SetParent(contentPanel, false);
                continue;
            }

            switch (item.Outcome){
                case EvaluationOutcome.Lost: {
                        eventBtn.typeColor.color = new Color32(144, 0, 0, 255);
                        eventBtn.typeText.text = "D   A   N   G   E   R";
                        eventBtn.typeText.color = new Color32(56, 0, 0, 255);
                    }
                    break;

                case EvaluationOutcome.Neutral:{
                        eventBtn.typeColor.color = new Color32(0, 119, 237, 255);
                        eventBtn.typeText.text = "I    N    F    O";
                        eventBtn.typeText.color = new Color32(56, 0, 120, 255);
                    }
                    break;

                case EvaluationOutcome.Success:{
                        eventBtn.typeColor.color = new Color32(0, 119, 0, 255);
                        eventBtn.typeText.text = "S  U  C  C  E  S  S";
                        eventBtn.typeText.color = new Color32(0, 36, 0, 255);
                    }
                    break;

                default:
                    break;

            }

            switch (item.Type)
            {

                case EvaluationType.Supply:           //A planet received supply ships
                    {
                        eventBtn.line1.text = "Incoming Support!";
                        eventBtn.line2.text = "You received " + item.IncomingShips + " ships.";
                        eventBtn.line3.text = item.ShipsOnPlanetAfterEvent + " ships are on the planet";
                        if (item.LostShips > 0){    // overwrite last line
                            eventBtn.line3.text = item.LostShips + " ships were lost due to full hangar";
                        }
                    }
                    break;
                case EvaluationType.AttackedPlanet:
                    { //The player attacked another planet
                        string originalPlayerName = (item.OriginalOwner == null ? "a neutral planet" : item.OriginalOwner.Name);
                        eventBtn.line1.text = "You attacked " + originalPlayerName;
                        eventBtn.line2.text = "with " + item.IncomingShips + " ships.";
                        switch (item.Outcome)
                        {
                            case EvaluationOutcome.Lost: { eventBtn.line3.text = "There are no survivors."; } break;
                            case EvaluationOutcome.Neutral: { eventBtn.line3.text = "The planet is now neutral."; } break;
                            case EvaluationOutcome.Success: { eventBtn.line3.text = "Captured! " + item.ShipsOnPlanetAfterEvent + " ships survived."; } break;
                            default: break;
                        }
                    }
                    break;
                case EvaluationType.GotAttacked: //A planet of the player got attacked by another player
                    {
                        string playerAttacked = item.ShipOwner.Name + " attacked";
                        string attackedWith = " with " + item.IncomingShips + " ships";

                        eventBtn.line2.text = "Lost!";

                        switch (item.Outcome){
                            case EvaluationOutcome.Lost:{
                                    attackedWith = ".";
                                    eventBtn.line3.text = "There are no survivors.";
                                }
                                break;
                            case EvaluationOutcome.Neutral:{
                                    eventBtn.line3.text = "The planet is now neutral.";
                                }
                                break;
                            case EvaluationOutcome.Success:{
                                    eventBtn.line2.text = "Survived!";  // Overwrite
                                    eventBtn.line3.text = item.ShipsOnPlanetAfterEvent + " ships survived.";
                                }
                                break;
                            default: break;
                        }
                        eventBtn.line1.text = playerAttacked + attackedWith;
                    }
                    break;
                case EvaluationType.CaptureViewer:
                    { //Another player attacked another planet and won - the ownership changed.
                        var otherAttackedPlayer = item.OriginalOwner == null ? "a neutral planet" : item.PlanetOwner.Name;

                        eventBtn.line1.text = item.ShipOwner.Name + " attacked";
                        eventBtn.line2.text = otherAttackedPlayer;
                        eventBtn.line3.text = "and captured it.";
                    } break;
                case EvaluationType.AttackViewer:
                    {
                        string shipOwner = "A neutral planet";
                        if (item.ShipOwner != null) {
                            shipOwner = item.ShipOwner.Name;
                        }
                        if (item.ShipOwner == item.OriginalOwner) {
                            eventBtn.line1.text = shipOwner + " sent support ships";
                            eventBtn.line2.text = "to a planet.";
                            eventBtn.line3.text = "";
                        } else {
                            eventBtn.line1.text = shipOwner + " attacked";
                            eventBtn.line2.text = "" + (item.OriginalOwner == null ? "a neutral planet" : item.OriginalOwner.Name) + ".";
                            eventBtn.line3.text = "";
                        }
                    }
                    break;
                default:
                    break;

            }

            //space.getPlanet(item.Planet);
            //item.Planet.TextureName


           // item.Planet
            Debug.Log(item.Planet.TextureName);
            Debug.Log(planetAtlas.GetSprite(item.Planet.TextureName).name);
            eventBtn.planetSprite.sprite = planetAtlas.GetSprite(item.Planet.TextureName);
            Debug.Log(eventBtn.planetSprite.sprite.name);
            //eventBtn.name = item.planetName;
            //eventBtn.icon.sprite = item.icon;
            //eventBtn.button.onClick = item.thingToDo;

            listItem.transform.SetParent(contentPanel, false);

        }
    }

    public void Fill(List<AttackEvaluation> events)
    {
        itemList = events;
        PopulateList();
        // delete gameobjects
        // clear old list 
        // set new list 
        // generate new buttons

    }

    private void SetNothingHappenedText()
    {
        if (itemList == null || itemList.Count == 0)
        {
            int index = Random.Range(0, itemsEmptyTexts.GetLength(0) - 1);
            if (lastRandom == index)
            {
                index = Random.Range(0, itemsEmptyTexts.GetLength(0) - 1);
            }
            lastRandom = index;
            NoItemsInListText.text = itemsEmptyTexts[index];
        }
        else
        {
            NoItemsInListText.text = "";
        }

    }
}
