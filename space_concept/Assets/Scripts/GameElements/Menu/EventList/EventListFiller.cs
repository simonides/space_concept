using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EventListFiller : MonoBehaviour
{
    // ****    CONFIGURATION    **** //

    // ****  ATTACHED OBJECTS   **** //
    GameObject pooledGameObjectHolder;

    // ****     ALLOCATORS      **** //
    PoolAllocator<GameObject> eventButtonPool;

    // ****                     **** //


    public EventButton prefab;

    public GameObject sampleButton;
    public List<AttackEvaluation> itemList;

    public Text NoItemsInListText;
    private int lastRandom = -1;


    public Transform contentPanel;


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
        pooledGameObjectHolder = GameObject.Find("PooledGameObjects");
        if (pooledGameObjectHolder == null)
        {
            throw new MissingComponentException("Unable to find PooledGameObjects There should be an empty game object that holds the pooled data.");
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
        Debug.Log("Listitems size: " + itemList.Count);

        // get items from the content panel
        var eventlistitems = contentPanel.GetComponentsInChildren<EventButton>();

        // put them back into the pool
        foreach (var item in eventlistitems)
        {
            eventButtonPool.PutBack(item.self_ref);
        }
        // detach them from the content panel
        contentPanel.transform.DetachChildren();

        AttackEvaluation atev = new AttackEvaluation();
        atev.Outcome = EvaluationOutcome.Lost;

        AttackEvaluation atev1 = new AttackEvaluation();
        atev1.Outcome = EvaluationOutcome.Neutral;

        AttackEvaluation atev2 = new AttackEvaluation();
        atev2.Outcome = EvaluationOutcome.Success;

        itemList.Add(atev);
        itemList.Add(atev1);
        itemList.Add(atev2);



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

            switch (item.Outcome)
            {
                case EvaluationOutcome.Lost:
                    {
                        eventBtn.typeColor.color = new Color(144, 0, 0, 255);
                        eventBtn.typeText.text = "D   A   N   G   E   R";
                        eventBtn.typeText.color = new Color(56, 0, 0, 255);
                    }
                    break;
                case EvaluationOutcome.Neutral:
                    {
                        eventBtn.typeColor.color = new Color(0, 119, 237, 255);
                        eventBtn.typeText.text = "N   E   U   T   R   A   L";
                        eventBtn.typeText.color = new Color(56, 0, 120, 255);
                    }
                    break;
                case EvaluationOutcome.Success:
                    {
                        eventBtn.typeColor.color = new Color(0, 119, 0, 255);
                        eventBtn.typeText.text = "S   U   C   C   E   S   S";
                        eventBtn.typeText.color = new Color(0, 36, 0, 255);
                    }
                    break;

                default:
                    break;

            }

            switch (item.Type)
            {

                case EvaluationType.Supply:           //A planet received supply ships
                    {
                        eventBtn.line1.text = "You recieved";
                        eventBtn.line2.text = "+" + item.IncomingShips + " ships";
                        eventBtn.line3.text = "";

                    }
                    break;
                case EvaluationType.AttackedPlanet: //The player attacked another planet
                    {
                        eventBtn.line1.text = "You recieved";
                        eventBtn.line2.text = "+" + item.IncomingShips + " ships";
                        eventBtn.line3.text = "";
                    }
                    break;
                case EvaluationType.GotAttacked: //A planet of the player got attacked by another planet
                    {
                        eventBtn.line1.text = "You recieved";
                        eventBtn.line2.text = "+" + item.IncomingShips + " ships";
                        eventBtn.line3.text = "";
                    }
                    break;
                case EvaluationType.CaptureViewer: //Another player attacked another planet and won - the ownership changed.
                    {
                        eventBtn.line1.text = "You recieved";
                        eventBtn.line2.text = "+" + item.IncomingShips + " ships";
                        eventBtn.line3.text = "";
                    }
                    break;
                case EvaluationType.AttackViewer:
                    {
                        eventBtn.line1.text = "You recieved";
                        eventBtn.line2.text = "+" + item.IncomingShips + " ships";
                        eventBtn.line3.text = "";
                    }
                    break;
                default:
                    break;

            }
            //item.Planet.
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
